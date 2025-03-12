using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using PlatyPdfs.App.Core.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatyPdfs.App.Pages.AboutPages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>

public class LibraryLicense
{
    public string Name { get; set; } = "";
    public string License { get; set; } = "";
    public Uri? LicenseURL
    {
        get; set;
    }
    public string HomepageText { get; set; } = "";
    public Uri? HomepageUrl
    {
        get; set;
    }
}

public sealed partial class ThirdPartyLicenses : Page
{
    public ObservableCollection<LibraryLicense> Licenses = [];

    public ThirdPartyLicenses()
    {
        InitializeComponent();
        foreach (string license in LicenseData.LicenseNames.Keys)
        {
            Licenses.Add(new LibraryLicense
            {
                Name = license,
                License = LicenseData.LicenseNames[license],
                LicenseURL = LicenseData.LicenseURLs[license],
                HomepageUrl = LicenseData.HomepageUrls[license],
                // TODO: convert to localized string
                HomepageText = String.Format("{0} homepage", license)
            });
        }

    }
}
