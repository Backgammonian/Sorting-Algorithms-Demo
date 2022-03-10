using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SortingAlgorithmsDemo.Algorithms;

namespace SortingAlgorithmsDemo
{
    public class MainWindowViewModel : ObservableObject
    {
        private bool _isRunning;
        private int _amount;
        private double _xStep;
        private double _valueStep;
        private double _gradualGrayColorStep;
        private const int _minAmount = 10;
        private BitmapImage _canvas;
        private DirectBitmap _bitmap;
        private const int _width = 800;
        private const int _height = 600;
        private readonly DispatcherTimer _timer;
        private readonly List<SortingUnit> _units;
        private readonly List<SortingExchange> _exchanges;
        private int _currentExchange;
        private ColorSchemes _colorScheme;

        public MainWindowViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            _timer.Tick += TimerTick;

            _units = new List<SortingUnit>();
            _exchanges = new List<SortingExchange>();
            _currentExchange = 0;

            ApplyNewAmountCommand = new RelayCommand(ApplyNewAmount);
            StartCommand = new RelayCommand(Start);
            EndCommand = new RelayCommand(End);
            ShuffleCommand = new RelayCommand(Shuffle);
            ReverseCommand = new RelayCommand(Reverse);
            AllUniqueCommand = new RelayCommand(MakeAllUnique);
            FewUniqueCommand = new RelayCommand(MakeFewUnique);
            PartiallyOrderedCommand = new RelayCommand(MakePartiallyOrdered);
            ShowSortingAlgorithmInfoCommand = new RelayCommand(ShowInfo);

            SortingAlgorithmsList = new ObservableCollection<SortingAlgorithms>
            {
                SortingAlgorithms.Bubble,
                SortingAlgorithms.Insertion,
                SortingAlgorithms.Merge
            };

            _bitmap = new DirectBitmap(_width, _height);

            IsRunning = false;
            Amount = "30";

            MakeUnits();
            Render();
        }

        public ICommand ApplyNewAmountCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand ShuffleCommand { get; }
        public ICommand ReverseCommand { get; }
        public ICommand AllUniqueCommand { get; }
        public ICommand FewUniqueCommand { get; }
        public ICommand PartiallyOrderedCommand { get; }
        public ICommand ShowSortingAlgorithmInfoCommand { get; }
        public SortingAlgorithms SelectedSortingAlgorithm { get; set; }
        public int CurrentAmount { get; private set; }
        public ObservableCollection<SortingAlgorithms> SortingAlgorithmsList { get; }

        public ColorSchemes SelectedColorScheme
        {
            get => _colorScheme;
            set
            {
                SetProperty(ref _colorScheme, value);

                Render();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                SetProperty(ref _isRunning, value);

                OnPropertyChanged(nameof(IsNotRunning));
            }
        }

        public bool IsNotRunning
        {
            get => !IsRunning;
        }

        public BitmapImage Canvas
        {
            get => _canvas;
            private set => SetProperty(ref _canvas, value);
        }

        public string Amount
        {
            get => _amount.ToString();
            set
            {
                if (int.TryParse(value, out int amount) &&
                    amount >= _minAmount)
                {
                    SetProperty(ref _amount, amount);

                    _xStep = _width / Convert.ToDouble(_amount);
                    _valueStep = _height / Convert.ToDouble(_amount);
                    _gradualGrayColorStep = 255.0 / Convert.ToDouble(_amount);

                    CurrentAmount = _amount;
                    OnPropertyChanged(nameof(CurrentAmount));
                }
            }
        }

        private void ApplyNewAmount()
        {
            MakeUnits();
            Render();
        }

        private void MakeUnits()
        {
            _units.Clear();
            for (int i = 0; i < _amount; i++)
            {
                var value = Convert.ToInt32(_valueStep * i);
                var color = Utils.GetRandomColor();
                var grayShadeComponent = Convert.ToInt32(_gradualGrayColorStep * i);
                var grayGradientColor = Color.FromArgb(grayShadeComponent, grayShadeComponent, grayShadeComponent);

                _units.Add(new SortingUnit(value, color, grayGradientColor));
            }
        }

        private void MakeFewUniqueUnits()
        {
            var randomValues = new List<int>();
            for (int i = 0; i < Utils.Next(3, 30); i++)
            {
                randomValues.Add(Utils.Next(0, _height));
            }

            _units.Clear();
            for (int i = 0; i < _amount; i++)
            {
                var color = Utils.GetRandomColor();
                var value = randomValues[Utils.Next(0, randomValues.Count)];

                _units.Add(new SortingUnit(value, color));
            }

            _units.BubbleSortAscending(InnerSwap);

            for (int i = 0; i < _units.Count; i++)
            {
                var grayShadeComponent = Convert.ToInt32(_gradualGrayColorStep * i);
                var grayGradientColor = Color.FromArgb(grayShadeComponent, grayShadeComponent, grayShadeComponent);

                _units[i].SetGraduatedGrayColor(grayGradientColor);
            }
        }

        private void Start()
        {
            Debug.WriteLine("Method: " + SelectedSortingAlgorithm);
            Debug.WriteLine("Color scheme: " + SelectedColorScheme);

            IsRunning = true;

            _exchanges.Clear();
            _currentExchange = 0;

            switch (SelectedSortingAlgorithm)
            {
                case SortingAlgorithms.Bubble:
                    _units.BubbleSortAscending(Swap);
                    break;
                case SortingAlgorithms.Insertion:
                    break;
                case SortingAlgorithms.Merge:
                    break;
            }

            _timer.Start();
        }

        private void End()
        {
            Stop();
        }

        private void Stop()
        {
            IsRunning = false;
            _timer.Stop();

            //отобразить конечное значение послед-ти
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_currentExchange == _exchanges.Count)
            {
                Stop();

                return;
            }

            var position1 = _exchanges[_currentExchange].FirstIndex;
            var position2 = _exchanges[_currentExchange].SecondIndex;

            InnerSwap(position1, position2);

            var unit1 = _units[position1];
            var unit2 = _units[position2];

            var x1 = Convert.ToInt32(_xStep * position1);
            var x2 = Convert.ToInt32(_xStep * position2);

            _bitmap.FillRectangle(x1, 0, (int)_xStep, _height, Color.White);
            _bitmap.FillRectangle(x2, 0, (int)_xStep, _height, Color.White);

            switch (SelectedColorScheme)
            {
                case ColorSchemes.Random:
                    _bitmap.FillRectangle(x2, _height - unit2.Value, (int)_xStep, unit2.Value, unit2.Color);
                    _bitmap.FillRectangle(x1, _height - unit1.Value, (int)_xStep, unit1.Value, unit1.Color);
                    break;
                case ColorSchemes.GraduatedGray:
                    _bitmap.FillRectangle(x2, _height - unit2.Value, (int)_xStep, unit2.Value, unit2.GraduatedGrayColor);
                    _bitmap.FillRectangle(x1, _height - unit1.Value, (int)_xStep, unit1.Value, unit1.GraduatedGrayColor);
                    break;
            }

            _currentExchange += 1;

            UpdateCanvas();
        }

        private void InnerSwap(int i, int j)
        {
            var t = _units[i];
            _units[i] = _units[j];
            _units[j] = t;
        }

        private void Swap(int i, int j)
        {
            /*var t = _units[i];
            _units[i] = _units[j];
            _units[j] = t;*/

            _exchanges.Add(new SortingExchange(i, j));
        }

        private void Shuffle()
        {
            for (int i = 0; i < _amount; i++)
            {
                var oldPosition = Utils.Next(0, _amount);
                var newPosition = Utils.Next(0, _amount);

                InnerSwap(oldPosition, newPosition);
            }

            Render();
        }

        private void Reverse()
        {
            _units.BubbleSortDescending(InnerSwap);

            Render();
        }

        private void MakeAllUnique()
        {
            MakeUnits();

            Render();
        }

        private void MakeFewUnique()
        {
            MakeFewUniqueUnits();

            Render();
        }

        private void MakePartiallyOrdered()
        {
            _units.BubbleSortAscending(InnerSwap);

            for (int i = 0; i < _amount / 2; i++)
            {
                var oldPosition = Utils.Next(0, _amount);
                var newPosition = Utils.Next(0, _amount);

                InnerSwap(oldPosition, newPosition);
            }

            Render();
        }

        private void ShowInfo()
        {
            switch (SelectedSortingAlgorithm)
            {
                case SortingAlgorithms.Bubble:
                    MessageBox.Show(Properties.Resources.BubbleSortInfo, "Sorting algorithm info: " + SelectedSortingAlgorithm, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case SortingAlgorithms.Insertion:
                    break;
                case SortingAlgorithms.Merge:
                    break;
            }
        }

        private void UpdateCanvas()
        {
            using MemoryStream memory = new MemoryStream();
            _bitmap.Bitmap.Save(memory, ImageFormat.Bmp);
            memory.Position = 0;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            Canvas = bitmapImage;
        }

        private void Render()
        {
            _bitmap.Clear(Color.White);

            for (int i = 0; i < _units.Count; i++)
            {
                var x = Convert.ToInt32(_xStep * i);

                switch (SelectedColorScheme)
                {
                    case ColorSchemes.Random:
                        _bitmap.FillRectangle(x, _height - _units[i].Value, (int)_xStep, _units[i].Value, _units[i].Color);
                        break;
                    case ColorSchemes.GraduatedGray:
                        _bitmap.FillRectangle(x, _height - _units[i].Value, (int)_xStep, _units[i].Value, _units[i].GraduatedGrayColor);
                        break;
                }
            }

            UpdateCanvas();
        }
    }
}
