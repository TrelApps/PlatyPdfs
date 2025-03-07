using Microsoft.UI.Xaml.Controls;

using PlatyPdfs.App.ViewModels;

namespace PlatyPdfs.App.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}
