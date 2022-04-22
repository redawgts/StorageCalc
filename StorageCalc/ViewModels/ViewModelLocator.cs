using CommunityToolkit.Mvvm.DependencyInjection;

namespace StorageCalc.ViewModels;

public class ViewModelLocator
{
    public MainViewModel MainViewModel => Ioc.Default.GetRequiredService<MainViewModel>();
}
