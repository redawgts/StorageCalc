using StorageCalc.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StorageCalc.ViewModels
{
    public class MainViewModel
    {
        private readonly IMessageBoxHelper _messageBox;

        public MainViewModel(IMessageBoxHelper messageBox)
        {
            _messageBox = messageBox;
        }

        public (string txtTotalSize, string txtFaultTolerance) Calculate(
            string txtDiskCount, string txtDiskSpace,
            bool? raid0, bool? raid1, bool? raid5, bool? raid6, bool? raid10)
        {
            try
            {
                var diskCount = Convert.ToInt32(txtDiskCount);
                var diskSpace = Convert.ToDouble(txtDiskSpace);

                var diskspaceInBytes = diskSpace * 1000000000000L;
                var divisor = 1024L * 1024L * 1024L * 1024L;
                var realDiskSpace = (double)diskspaceInBytes / (double)divisor;

                var usableSpace = 0.0d;
                var faultTolerance = string.Empty;

                if (raid0 == true)
                {
                    usableSpace = diskCount * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["None"];
                }

                if (raid1 == true)
                {
                    if (diskCount == 2)
                    {
                        usableSpace = realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["OneDisk"];
                    }
                    else
                    {
                        _messageBox.Show(LocalizedStrings.Instance["ExactlyTwoPlatesAreRequired"]);
                        return default;
                    }
                }

                if (raid5 == true)
                {
                    if (diskCount >= 3)
                    {
                        usableSpace = (diskCount - 1) * realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["OneDisk"];
                    }
                    else
                    {
                        _messageBox.Show(LocalizedStrings.Instance["AtLeastThreePlatesRequired"]);
                        return default;
                    }
                }

                if (raid6 == true)
                {
                    if (diskCount >= 4)
                    {
                        usableSpace = (diskCount - 2) * realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["TwoDisks"];
                    }
                    else
                    {
                        _messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesRequired"]);
                        return default;
                    }
                }

                if (raid10 == true)
                {
                    if (diskCount % 2 == 0 && diskCount >= 4)
                    {
                        usableSpace = (diskCount - 2) * realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["MinOneDisk"];
                    }
                    else
                    {
                        _messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesAndEvenNumber"]);
                        return default;
                    }
                }

                return (Math.Round(usableSpace, 2) + " TB", faultTolerance);
            }
            catch (FormatException)
            {
                _messageBox.Show(LocalizedStrings.Instance["PleaseOnlyEnterNumber"]);
                return default;
            }
            catch (OverflowException)
            {
                _messageBox.Show(LocalizedStrings.Instance["PleaseEnterSmallerNumbers"]);
                return default;
            }
        }
    }
}
