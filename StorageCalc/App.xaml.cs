using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StorageCalc.ViewModels;
using System.Globalization;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace StorageCalc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.CurrentCulture;

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IMessageBoxHelper, MessageBoxHelper>()
                    .AddSingleton<MainViewModel>()
                    .BuildServiceProvider()
                );
        }
    }
}
