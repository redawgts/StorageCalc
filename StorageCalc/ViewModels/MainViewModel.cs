using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StorageCalc.Resources;
using System;
using System.Collections.Generic;
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

        public MainViewModel(IMessageBoxHelper messageBox)
        {
            _messageBox = messageBox;
        }

        [ObservableProperty] int _diskCount;
        [ObservableProperty] double _diskSpace;
        [ObservableProperty] bool _raid0;
        [ObservableProperty] bool _raid1;
        [ObservableProperty] bool _raid5;
        [ObservableProperty] bool _raid6;
        [ObservableProperty] bool _raid10;
        [ObservableProperty] string _totalSpaceText = string.Empty;
        [ObservableProperty] string _faultToleranceText = string.Empty;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CalculateCommand.NotifyCanExecuteChanged();
        }

        private bool CanCalculate()
        {
            return DiskCount > 0 && DiskSpace > 0
                && (Raid0 || Raid1 || Raid5 || Raid6 || Raid10);
        }

        [ICommand(CanExecute = nameof(CanCalculate))]
        private void Calculate()
        {
            double diskspaceInBytes = DiskSpace * 1000000000000L;
            var divisor = 1024L * 1024L * 1024L * 1024L;
            var realDiskSpace = (double)diskspaceInBytes / (double)divisor;

            var usableSpace = 0.0d;
            var faultTolerance = string.Empty;

            if (Raid0)
            {
                usableSpace = DiskCount * realDiskSpace;
                faultTolerance = LocalizedStrings.Instance["None"];
            }

            if (Raid1)
            {
                if (DiskCount == 2)
                {
                    usableSpace = realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["OneDisk"];
                }
                else
                {
                    _messageBox.Show(LocalizedStrings.Instance["ExactlyTwoPlatesAreRequired"]);
                    return;
                }
            }

            if (Raid5)
            {
                if (DiskCount >= 3)
                {
                    usableSpace = (DiskCount - 1) * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["OneDisk"];
                }
                else
                {
                    _messageBox.Show(LocalizedStrings.Instance["AtLeastThreePlatesRequired"]);
                    return;
                }
            }

            if (Raid6)
            {
                if (DiskCount >= 4)
                {
                    usableSpace = (DiskCount - 2) * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["TwoDisks"];
                }
                else
                {
                    _messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesRequired"]);
                    return;
                }
            }

            if (Raid10)
            {
                if (DiskCount % 2 == 0 && DiskCount >= 4)
                {
                    usableSpace = (DiskCount - 2) * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["MinOneDisk"];
                }
                else
                {
                    _messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesAndEvenNumber"]);
                    return;
                }
            }

            TotalSpaceText = Math.Round(usableSpace, 2) + " TB";
            FaultToleranceText = faultTolerance;
        }
    }
}
