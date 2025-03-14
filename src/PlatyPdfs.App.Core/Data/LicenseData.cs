﻿namespace PlatyPdfs.App.Core.Data;

public static class LicenseData
{
    public static Dictionary<string, string> LicenseNames = new() {
            {"PlatyPdfs",                "GPL-3.0" },

            // C# Libraries
            //{"Pickers",                 "MIT"},
            {"Community Toolkit",       "MIT"},
            //{"H.NotifyIcon",            "MIT"},
            {"Windows App Sdk",         "MIT"},
            //{"NancyFx",                 "MIT"},
            //{"YamlDotNet",              "MIT"},
            //{"InnoDependencyInstaller", "CPOL 1.02" },
            {"WinUIEx",                 "MIT"},
            {"docnet",                  "MIT"},


            // Package managers and related
            //{"Winget",                  "MIT"},
            //{"Scoop",                   "MIT"},
            //{"scoop-search",            "MIT"},
            //{"Chocolatey",              "Apache v2"},
            //{"Npm",                     "Artistic License 2.0"},
            //{"Pip",                     "MIT"},
            //{"parse_pip_search",        "MIT"},
            //{"PowerShell Gallery",      "Unknown"},
            {".NET Sdk",                "MIT"},
            //{"dotnet-tools-outdated",   "MIT"},

            // Other
            //{"Gsudo",                   "MIT"},
            //{"Icons",                   "By Icons8"},
        };

    public static Dictionary<string, Uri> LicenseURLs = new(){
            {"PlatyPdfs",                new Uri("https://github.com/TrelApps/PlatyPdfs/blob/main/LICENSE")},

            // C# Libraries
            //{"Pickers",                 new Uri("https://github.com/PavlikBender/Pickers/blob/master/LICENSE")},
            {"Community Toolkit",       new Uri("https://github.com/CommunityToolkit/Windows/blob/main/License.md")},
            //{"H.NotifyIcon",            new Uri("https://github.com/HavenDV/H.NotifyIcon/blob/master/LICENSE.md")},
            {"Windows App Sdk",         new Uri("https://github.com/microsoft/WindowsAppSDK/blob/main/LICENSE")},
            //{"NancyFx",                 new Uri("https://github.com/NancyFx/Nancy/blob/master/license.txt")},
            //{"YamlDotNet",              new Uri("https://github.com/aaubry/YamlDotNet/blob/master/LICENSE.txt") },
            //{"InnoDependencyInstaller", new Uri("https://github.com/DomGries/InnoDependencyInstaller/blob/master/LICENSE.md") },
            {"WinUIEx",                 new Uri("https://github.com/dotMorten/WinUIEx/blob/main/LICENSE")},
            {"docnet",                  new Uri("https://github.com/GowenGit/docnet/blob/master/LICENSE")},

            // Package managers and related
            //{"Winget",                  new Uri("https://github.com/microsoft/winget-cli/blob/master/LICENSE")},
            //{"Scoop",                   new Uri("https://github.com/ScoopInstaller/Scoop/blob/master/LICENSE")},
            //{"scoop-search",            new Uri("https://github.com/shilangyu/scoop-search/blob/master/LICENSE")},
            //{"Chocolatey",              new Uri("https://github.com/chocolatey/choco/blob/develop/LICENSE")},
            //{"Npm",                     new Uri("https://github.com/npm/cli/blob/latest/LICENSE")},
            //{"Pip",                     new Uri("https://github.com/pypa/pip/blob/main/LICENSE.txt")},
            //{"parse_pip_search",        new Uri("https://github.com/marticliment/parseable_pip_search/blob/master/LICENSE.md")},
            {".NET Sdk",                new Uri("https://github.com/dotnet/sdk/blob/main/LICENSE.TXT")},
            //{"dotnet-tools-outdated",   new Uri("https://github.com/rychlym/dotnet-tools-outdated/blob/master/LICENSE")},
            //{"PowerShell Gallery",      new Uri("https://www.powershellgallery.com/")},

            // Other
            //{"Gsudo",                   new Uri("https://github.com/gerardog/gsudo/blob/master/LICENSE.txt")},
            //{"Icons",                   new Uri("https://icons8.com/license")},
        };

    public static Dictionary<string, Uri> HomepageUrls = new(){
            {"PlatyPdfs",                new Uri("https://github.com/TrelApps/PlatyPdfs")},

            // C# Libraries
            //{"Pickers",                 new Uri("https://github.com/PavlikBender/Pickers/")},
            {"Community Toolkit",       new Uri("https://github.com/CommunityToolkit/Windows/")},
            //{"H.NotifyIcon",            new Uri("https://github.com/HavenDV/H.NotifyIcon/")},
            {"Windows App Sdk",         new Uri("https://github.com/microsoft/WindowsAppSDK/")},
            //{"NancyFx",                 new Uri("https://github.com/NancyFx/Nancy/")},
            //{"YamlDotNet",              new Uri("https://github.com/aaubry/YamlDotNet/") },
            //{"InnoDependencyInstaller", new Uri("https://github.com/DomGries/InnoDependencyInstaller")},
            {"WinUIEx",                 new Uri("https://dotmorten.github.io/WinUIEx/")},
            {"docnet",                  new Uri("https://github.com/GowenGit/docnet")},

            // Package managers and related
            //{"Winget",                  new Uri("https://github.com/microsoft/winget-cli/")},
            //{"Scoop",                   new Uri("https://github.com/ScoopInstaller/Scoop/")},
            //{"scoop-search",            new Uri("https://github.com/shilangyu/scoop-search/")},
            //{"Chocolatey",              new Uri("https://github.com/chocolatey/choco/")},
            //{"Npm",                     new Uri("https://github.com/npm/cli/")},
            //{"Pip",                     new Uri("https://github.com/pypa/pip/")},
            //{"parse_pip_search",        new Uri("https://github.com/marticliment/parseable_pip_search/")},
            {".NET Sdk",                new Uri("https://dotnet.microsoft.com/")},
            //{"dotnet-tools-outdated",   new Uri("https://github.com/rychlym/dotnet-tools-outdated/")},
            //{"PowerShell Gallery",      new Uri("https://www.powershellgallery.com/")},

            // Other
            //{"Gsudo",                   new Uri("https://github.com/gerardog/gsudo/")},
            //{"Icons",                   new Uri("https://icons8.com")},
        };

}
