using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatyPdfs.App.Pages.AboutPages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AboutPlatyPdfs : Page
{
    public event EventHandler? Close;
    private int previousSelectedIndex;

    public AboutPlatyPdfs()
    {
        this.InitializeComponent();
    }

    private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
        Type pageType = currentSelectedIndex switch
        {
            0 => typeof(AboutInformation),
            1 => typeof(ThirdPartyLicenses),
            _ => typeof(ThirdPartyLicenses),
        };
        SlideNavigationTransitionEffect slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

        ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo { Effect = slideNavigationTransitionEffect });

        previousSelectedIndex = currentSelectedIndex;

    }

    private void CloseButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Close?.Invoke(this, EventArgs.Empty);
    }
}
