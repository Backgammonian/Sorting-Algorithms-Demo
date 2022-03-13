﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
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
        private const int _width = 800;
        private const int _height = 600;
        private const int _minAmount = 10;
        private const int _maxAmount = _width;
        private bool _isRunning;
        private int _amount;
        private double _xStep;
        private double _valueStep;
        private double _gradualGrayColorStep;
        private BitmapImage _canvas;
        private DirectBitmap _bitmap;
        private readonly DispatcherTimer _timer;
        private readonly List<SortingUnit> _units;
        private readonly List<SortingUnit> _unitsCopy;
        private readonly List<SortingExchange> _exchanges;
        private int _currentExchange;
        private readonly List<SortingPlacement> _placements;
        private int _currentPlacement;
        private ColorSchemes _colorScheme;
        private readonly Color _solidColor;
        private readonly Color _sortedSolidColor;
        private SortingAlgorithms _sortingAlgorithm;
        private string _message;

        public MainWindowViewModel()
        {
            _solidColor = Color.DarkBlue;
            _sortedSolidColor = Color.DarkGreen;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            _timer.Tick += TimerTick;

            _units = new List<SortingUnit>();
            _unitsCopy = new List<SortingUnit>();
            _exchanges = new List<SortingExchange>();
            _currentExchange = 0;
            _placements = new List<SortingPlacement>();
            _currentPlacement = 0;

            ApplyNewAmountCommand = new RelayCommand(ApplyNewAmount);
            StartCommand = new RelayCommand(Start);
            EndCommand = new RelayCommand(End);
            ShuffleCommand = new RelayCommand(Shuffle);
            ReverseCommand = new RelayCommand(Reverse);
            AllUniqueCommand = new RelayCommand(MakeAllUnique);
            FewUniqueCommand = new RelayCommand(MakeFewUnique);
            PartiallyOrderedCommand = new RelayCommand(MakePartiallyOrdered);

            SortingAlgorithmsList = new ObservableCollection<SortingAlgorithms>
            {
                SortingAlgorithms.Bubble,
                SortingAlgorithms.Insertion,
                SortingAlgorithms.Merge,
                SortingAlgorithms.Selection,
                SortingAlgorithms.Shaker,
                SortingAlgorithms.Shell,
                SortingAlgorithms.Quick,
                SortingAlgorithms.Stooge,
                SortingAlgorithms.Pancake,
                SortingAlgorithms.Gnome,
                SortingAlgorithms.Counting,
                SortingAlgorithms.Radix,
                SortingAlgorithms.Comb,
                SortingAlgorithms.OddEven,
                SortingAlgorithms.Tree,
                SortingAlgorithms.Heap,
                SortingAlgorithms.Cycle,
                SortingAlgorithms.Bitonic
            };

            _bitmap = new DirectBitmap(_width, _height);

            IsRunning = false;
            Amount = "100";
            SelectedSortingAlgorithm = SortingAlgorithms.Bubble;

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
        public int CurrentAmount { get; private set; }
        public ObservableCollection<SortingAlgorithms> SortingAlgorithmsList { get; }

        public SortingAlgorithms SelectedSortingAlgorithm
        {
            get => _sortingAlgorithm;
            set
            {
                SetProperty(ref _sortingAlgorithm, value);

                Debug.WriteLine("SelectedSortingAlgorithm: " + SelectedSortingAlgorithm);

                switch (SelectedSortingAlgorithm)
                {
                    case SortingAlgorithms.Bubble:
                        Message = Properties.Resources.BubbleSortInfo;
                        break;

                    case SortingAlgorithms.Insertion:
                        Message = Properties.Resources.InsertionSortInfo;
                        break;

                    case SortingAlgorithms.Merge:
                        Message = Properties.Resources.MergeSortInfo;
                        break;

                    case SortingAlgorithms.Selection:
                        Message = Properties.Resources.SelectionSortInfo;
                        break;

                    case SortingAlgorithms.Shaker:
                        Message = Properties.Resources.ShakerSortInfo;
                        break;

                    case SortingAlgorithms.Shell:
                        Message = Properties.Resources.ShellSortInfo;
                        break;

                    case SortingAlgorithms.Quick:
                        Message = Properties.Resources.QuickSortInfo;
                        break;

                    case SortingAlgorithms.Stooge:
                        Message = Properties.Resources.StoogeSortInfo;
                        break;

                    case SortingAlgorithms.Pancake:
                        Message = Properties.Resources.PancakeSortInfo;
                        break;

                    case SortingAlgorithms.Gnome:
                        Message = Properties.Resources.GnomeSortInfo;
                        break;

                    case SortingAlgorithms.Counting:
                        Message = Properties.Resources.CountingSortInfo;
                        break;

                    case SortingAlgorithms.Radix:
                        Message = Properties.Resources.RadixSortInfo;
                        break;

                    case SortingAlgorithms.Comb:
                        Message = Properties.Resources.CombSortInfo;
                        break;

                    case SortingAlgorithms.OddEven:
                        Message = Properties.Resources.OddEvenSortInfo;
                        break;

                    case SortingAlgorithms.Tree:
                        Message = Properties.Resources.TreeSortInfo;
                        break;

                    case SortingAlgorithms.Heap:
                        Message = Properties.Resources.HeapSortInfo;
                        break;

                    case SortingAlgorithms.Cycle:
                        Message = Properties.Resources.CycleSortInfo;
                        break;

                    case SortingAlgorithms.Bitonic:
                        Message = Properties.Resources.BitonicSortInfo;
                        break;
                }
            }
        }

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
                if (int.TryParse(value, out int amount))
                {
                    amount = amount < _minAmount ? _minAmount : amount > _maxAmount ? _maxAmount : amount;

                    SetProperty(ref _amount, amount);

                    _xStep = _width / Convert.ToDouble(_amount);
                    _valueStep = _height / Convert.ToDouble(_amount);
                    _gradualGrayColorStep = 255.0 / Convert.ToDouble(_amount);

                    CurrentAmount = _amount;
                    OnPropertyChanged(nameof(CurrentAmount));
                }
            }
        }

        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
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

                _units.Add(new SortingUnit(value, color, grayGradientColor, i));
            }
        }

        private void MakeFewUniqueUnits()
        {
            var randomValues = new List<int>();
            for (int i = 0; i < Utils.Next(3, 20); i++)
            {
                randomValues.Add(Utils.Next(0, _height));
            }
            randomValues.Sort();

            var eachTypeAmount = _amount / randomValues.Count;
            var additionalUnits = _amount % randomValues.Count;
            var type = 0;

            _units.Clear();
            var currentPosition = 0;
            var smallIncrement = 0.001;

            for (int i = 0; i < _amount; i++)
            {
                if (type == randomValues.Count)
                {
                    for (int k = 0; k < additionalUnits; k++)
                    {
                        var color = Utils.GetRandomColor();
                        var value = randomValues[type - 1];

                        var grayShadeComponent = Convert.ToInt32(_gradualGrayColorStep * _units.Count);
                        var grayGradientColor = Color.FromArgb(grayShadeComponent, grayShadeComponent, grayShadeComponent);

                        _units.Add(new SortingUnit(value + smallIncrement, color, grayGradientColor, currentPosition));
                        currentPosition += 1;
                        smallIncrement += 0.001;
                    }

                    break;
                }

                for (int j = 0; j < eachTypeAmount; j++)
                {
                    var color = Utils.GetRandomColor();
                    var value = randomValues[type];

                    var grayShadeComponent = Convert.ToInt32(_gradualGrayColorStep * _units.Count);
                    var grayGradientColor = Color.FromArgb(grayShadeComponent, grayShadeComponent, grayShadeComponent);
    
                    _units.Add(new SortingUnit(value + smallIncrement, color, grayGradientColor, currentPosition));
                    currentPosition += 1;
                    smallIncrement += 0.001;
                }

                type += 1;
            }
        }

        private void Start()
        {
            Debug.WriteLine("Method: " + SelectedSortingAlgorithm);

            IsRunning = true;

            _exchanges.Clear();
            _currentExchange = 0;

            _placements.Clear();
            _currentPlacement = 0;

            _unitsCopy.Clear();
            _unitsCopy.AddRange(_units);

            switch (SelectedSortingAlgorithm)
            {
                case SortingAlgorithms.Bubble:
                    _unitsCopy.BubbleSortAscending(SwapFunction);
                    break;

                case SortingAlgorithms.Insertion:
                    _unitsCopy.InsertionSort(SwapFunction);
                    break;

                case SortingAlgorithms.Merge:
                    _unitsCopy.MergeSort(PlacementFunction);
                    break;

                case SortingAlgorithms.Selection:
                    _unitsCopy.SelectionSort(SwapFunction);
                    break;

                case SortingAlgorithms.Shaker:
                    _unitsCopy.ShakerSort(SwapFunction);
                    break;

                case SortingAlgorithms.Shell:
                    _unitsCopy.ShellSort(SwapFunction);
                    break;

                case SortingAlgorithms.Quick:
                    _unitsCopy.QuickSort(SwapFunction);
                    break;

                case SortingAlgorithms.Stooge:
                    _unitsCopy.StoogeSort(SwapFunction);
                    break;

                case SortingAlgorithms.Pancake:
                    _unitsCopy.PancakeSort(SwapFunction);
                    break;

                case SortingAlgorithms.Gnome:
                    _unitsCopy.GnomeSort(SwapFunction);
                    break;

                case SortingAlgorithms.Counting:
                    _unitsCopy.CountingSort(PlacementFunction);
                    break;

                case SortingAlgorithms.Radix:
                    _unitsCopy.RadixSort(PlacementFunction);
                    break;

                case SortingAlgorithms.Comb:
                    _unitsCopy.CombSort(SwapFunction);
                    break;

                case SortingAlgorithms.OddEven:
                    _unitsCopy.OddEvenSort(SwapFunction);
                    break;

                case SortingAlgorithms.Tree:
                    _unitsCopy.TreeSort(PlacementFunction);
                    break;

                case SortingAlgorithms.Heap:
                    _unitsCopy.HeapSort(SwapFunction);
                    break;

                case SortingAlgorithms.Cycle:
                    _unitsCopy.CycleSort(PlacementFunction);
                    break;

                case SortingAlgorithms.Bitonic:
                    _unitsCopy.BitonicSort(SwapFunction);
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

            _units.Clear();
            _units.AddRange(_unitsCopy);

            Render();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (SelectedSortingAlgorithm is
                SortingAlgorithms.Merge or
                SortingAlgorithms.Counting or
                SortingAlgorithms.Radix or
                SortingAlgorithms.Tree or
                SortingAlgorithms.Cycle)
            {
                DrawPlacements();
            }
            else
            {
                DrawSwaps();
            }

            UpdateCanvas();
        }

        private void DrawPlacements()
        {
            if (_currentPlacement == _placements.Count)
            {
                Stop();

                return;
            }

            var position = _placements[_currentPlacement].Position;
            var value = _placements[_currentPlacement].Value;
            _units[position] = value;

            var unit = _units[position];
            var unitValue = (int)unit.Value;
            var x = Convert.ToInt32(_xStep * position);

            _bitmap.FillRectangle(x, 0, (int)_xStep, _height, Color.White);

            switch (SelectedColorScheme)
            {
                case ColorSchemes.Solid:
                    _bitmap.FillRectangle(x, _height - unitValue, (int)_xStep, unitValue, unit.InitialPosition == position ? _sortedSolidColor : _solidColor);
                    break;

                case ColorSchemes.Random:
                    _bitmap.FillRectangle(x, _height - unitValue, (int)_xStep, unitValue, unit.Color);
                    break;

                case ColorSchemes.GraduatedGray:
                    _bitmap.FillRectangle(x, _height - unitValue, (int)_xStep, unitValue, unit.GraduatedGrayColor);
                    break;
            }

            _currentPlacement += 1;
        }

        private void DrawSwaps()
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
            var unit1Value = (int)_units[position1].Value;
            var unit2Value = (int)_units[position2].Value;

            var x1 = Convert.ToInt32(_xStep * position1);
            var x2 = Convert.ToInt32(_xStep * position2);

            _bitmap.FillRectangle(x1, 0, (int)_xStep, _height, Color.White);
            _bitmap.FillRectangle(x2, 0, (int)_xStep, _height, Color.White);

            switch (SelectedColorScheme)
            {
                case ColorSchemes.Solid:
                    _bitmap.FillRectangle(x1, _height - unit1Value, (int)_xStep, unit1Value, unit1.InitialPosition == position1 ? _sortedSolidColor : _solidColor);
                    _bitmap.FillRectangle(x2, _height - unit2Value, (int)_xStep, unit2Value, unit2.InitialPosition == position2 ? _sortedSolidColor : _solidColor);
                    break;

                case ColorSchemes.Random:
                    _bitmap.FillRectangle(x1, _height - unit1Value, (int)_xStep, unit1Value, unit1.Color);
                    _bitmap.FillRectangle(x2, _height - unit2Value, (int)_xStep, unit2Value, unit2.Color);
                    break;

                case ColorSchemes.GraduatedGray:
                    _bitmap.FillRectangle(x1, _height - unit1Value, (int)_xStep, unit1Value, unit1.GraduatedGrayColor);
                    _bitmap.FillRectangle(x2, _height - unit2Value, (int)_xStep, unit2Value, unit2.GraduatedGrayColor);
                    break;
            }

            _currentExchange += 1;
        }

        private void InnerSwap(int i, int j)
        {
            var t = _units[i];
            _units[i] = _units[j];
            _units[j] = t;
        }

        private void SwapFunction(int i, int j)
        {
            var t = _unitsCopy[i];
            _unitsCopy[i] = _unitsCopy[j];
            _unitsCopy[j] = t;

            _exchanges.Add(new SortingExchange(i, j));
        }

        private void PlacementFunction(int position, SortingUnit value)
        {
            _unitsCopy[position] = value;

            _placements.Add(new SortingPlacement(position, value));
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
                var unitValue = (int)_units[i].Value;

                switch (SelectedColorScheme)
                {
                    case ColorSchemes.Solid:
                        _bitmap.FillRectangle(x, _height - unitValue, (int)_xStep, unitValue, _units[i].InitialPosition == i ? _sortedSolidColor : _solidColor);
                        break;

                    case ColorSchemes.Random:
                        _bitmap.FillRectangle(x, _height - unitValue, (int)_xStep, unitValue, _units[i].Color);
                        break;

                    case ColorSchemes.GraduatedGray:
                        _bitmap.FillRectangle(x, _height - unitValue, (int)_xStep, unitValue, _units[i].GraduatedGrayColor);
                        break;
                }
            }

            UpdateCanvas();
        }
    }
}
