
namespace StorageCalc
{
    using StorageCalc.Resources;
    using StorageCalc.ViewModels;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCalculate_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var vm = new MainViewModel(new MessageBoxHelper());
                vm.DiskCount = Convert.ToInt32(TxtDiskCount.Text);
                vm.DiskSpace = Convert.ToDouble(TxtDiskSpace.Text);
                vm.Raid0 = RbnRaid0.IsChecked ?? false;
                vm.Raid1 = RbnRaid1.IsChecked ?? false;
                vm.Raid5 = RbnRaid5.IsChecked ?? false;
                vm.Raid6 = RbnRaid6.IsChecked ?? false;
                vm.Raid10 = RbnRaid10.IsChecked ?? false;

                vm.CalculateCommand.Execute(null);

                //(string txtTotalSize, string txtFaultTolerance) = vm.Calculate(
                //    TxtDiskCount.Text, TxtDiskSpace.Text, RbnRaid0.IsChecked,
                //    RbnRaid1.IsChecked, RbnRaid5.IsChecked,
                //    RbnRaid6.IsChecked, RbnRaid10.IsChecked);

                TxtTotalSize.Text = vm.TotalSpaceText;
                TxtFaultTolerance.Text = vm.FaultToleranceText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizedStrings.Instance["SomethingWentWrong"] + "\r\n\r\n" + ex);
            }
        }
    }
}
