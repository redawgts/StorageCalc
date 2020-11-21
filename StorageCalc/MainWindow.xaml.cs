
namespace StorageCalc
{
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
                var diskCount = Convert.ToInt32(this.TxtDiskCount.Text);
                var diskSpace = Convert.ToDouble(this.TxtDiskSpace.Text);

                var diskspaceInBytes = diskSpace * 1000000000000L;
                var divisor = 1024L * 1024L * 1024L * 1024L;
                var realDiskSpace = (double)diskspaceInBytes / (double)divisor;

                var usableSpace = 0.0d;
                var faultTolerance = string.Empty;

                if (this.RbnRaid0.IsChecked == true)
                {
                    usableSpace = diskCount * realDiskSpace;
                    faultTolerance = "keine";
                }

                if (this.RbnRaid1.IsChecked == true)
                {
                    if (diskCount == 2)
                    {
                        usableSpace = realDiskSpace;
                        faultTolerance = "1 Platte";
                    }
                    else
                    {
                        MessageBox.Show("Genau 2 Platten benötigt");
                        return;
                    }
                }

                if (this.RbnRaid5.IsChecked == true)
                {
                    if (diskCount >= 3)
                    {
                        usableSpace = (diskCount - 1) * realDiskSpace;
                        faultTolerance = "1 Platte";
                    }
                    else
                    {
                        MessageBox.Show("Min. 3 Platten notwendig");
                        return;
                    }
                }

                if (this.RbnRaid6.IsChecked == true)
                {
                    if (diskCount >= 4)
                    {
                        usableSpace = (diskCount - 2) * realDiskSpace;
                        faultTolerance = "2 Platten";
                    }
                    else
                    {
                        MessageBox.Show("Min. 4 Platten notwendig");
                        return;
                    }
                }

                if (this.RbnRaid10.IsChecked == true)
                {
                    if (diskCount % 2 == 0 && diskCount >= 4)
                    {
                        usableSpace = (diskCount - 2) * realDiskSpace;
                        faultTolerance = "Min. 1 Platte";
                    }
                    else
                    {
                        MessageBox.Show("Min. 4 Platten und gerade Anzahl notwendig");
                        return;
                    }
                }

                this.TxtTotalSize.Text = Math.Round(usableSpace, 2) + " TB";
                this.TxtFaultTolerance.Text = faultTolerance;
            }
            catch (FormatException)
            {
                MessageBox.Show("Eingabe prüfen, bitte nur Zahlen eingeben");
            }
            catch (OverflowException)
            {
                MessageBox.Show("Overflow, bitte kleinere Zahlen eingeben");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Da ging was schief:\r\n\r\n" + ex);
            }
        }
    }
}
