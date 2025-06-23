namespace PlatyPdfs.App.Core.Data;

public static class LicenseData
{
    public static Dictionary<string, string> LicenseNames = new() {
            {"PlatyPdfs",                "GPL-3.0" },
            {"Community Toolkit",       "MIT"},
            {"Windows App Sdk",         "MIT"},
            {"WinUIEx",                 "MIT"},
            {"docnet",                  "MIT"},
            {".NET Sdk",                "MIT"},
        };

    public static Dictionary<string, Uri> LicenseURLs = new(){
            {"PlatyPdfs",               new Uri("https://github.com/TrelApps/PlatyPdfs/blob/master/LICENSE") },
            {"Community Toolkit",       new Uri("https://github.com/CommunityToolkit/Windows/blob/main/License.md")},
            {"Windows App Sdk",         new Uri("https://github.com/microsoft/WindowsAppSDK/blob/main/LICENSE")},
            {"WinUIEx",                 new Uri("https://github.com/dotMorten/WinUIEx/blob/main/LICENSE")},
            {"docnet",                  new Uri("https://github.com/GowenGit/docnet/blob/master/LICENSE")},
            {".NET Sdk",                new Uri("https://github.com/dotnet/sdk/blob/main/LICENSE.TXT")},
        };

    public static Dictionary<string, Uri> HomepageUrls = new(){
            {"PlatyPdfs",               new Uri("https://github.com/TrelApps/PlatyPdfs")},
            {"Community Toolkit",       new Uri("https://github.com/CommunityToolkit/Windows/")},
            {"Windows App Sdk",         new Uri("https://github.com/microsoft/WindowsAppSDK/")},
            {"WinUIEx",                 new Uri("https://dotmorten.github.io/WinUIEx/")},
            {"docnet",                  new Uri("https://github.com/GowenGit/docnet")},
            {".NET Sdk",                new Uri("https://dotnet.microsoft.com/")},
        };

}
