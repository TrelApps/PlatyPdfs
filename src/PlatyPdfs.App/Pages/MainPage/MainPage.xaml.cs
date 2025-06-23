using Microsoft.UI.Xaml.Controls;

using PlatyPdfs.App.ViewModels;

namespace PlatyPdfs.App.Pages;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
