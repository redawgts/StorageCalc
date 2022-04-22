using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StorageCalc.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IMessageBoxHelper _messageBox;
        private readonly IRaidCalculatorFactory _calculatorFactory;

        public MainViewModel(IMessageBoxHelper messageBox, IRaidCalculatorFactory calculatorFactory)
        {
            _messageBox = messageBox;
            _calculatorFactory = calculatorFactory;
            Calculators = new(_calculatorFactory.GetAll());
        }

        [ObservableProperty] int _diskCount;
        [ObservableProperty] double _diskSpace;
        [ObservableProperty] string _totalSpaceText = string.Empty;
        [ObservableProperty] string _faultToleranceText = string.Empty;
        [ObservableProperty] IRaidCalculator _selectedCalculator;

        public ObservableCollection<IRaidCalculator> Calculators { get; init; }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CalculateCommand.NotifyCanExecuteChanged();
        }

        private bool CanCalculate()
        {
            return DiskCount > 0 && DiskSpace > 0
                && SelectedCalculator != null;
        }

        [ICommand(CanExecute = nameof(CanCalculate))]
        private void Calculate()
        {
            var result = SelectedCalculator.Calculate(DiskCount, DiskSpace);

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                _messageBox.Show(result.ErrorMessage);
                return;
            }

            TotalSpaceText = Math.Round(result.UseableDiskSpace, 2) + " TB";
            FaultToleranceText = result.FaultTolerance;
        }
    }
}
