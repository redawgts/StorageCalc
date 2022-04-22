
namespace StorageCalc
{
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
            var vm = new MainViewModel();

            (string txtTotalSize, string txtFaultTolerance) = vm.Calculate(
                TxtDiskCount.Text, TxtDiskSpace.Text, RbnRaid0.IsChecked,
                RbnRaid1.IsChecked, RbnRaid5.IsChecked,
                RbnRaid6.IsChecked, RbnRaid10.IsChecked);

            TxtTotalSize.Text = txtTotalSize;
            TxtFaultTolerance.Text = txtFaultTolerance;
        }
    }
}
