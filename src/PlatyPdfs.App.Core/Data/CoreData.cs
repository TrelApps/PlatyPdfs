using System.Net;

namespace PlatyPdfs.App.Core.Data;

public static class CoreData
{
    public const string VersionName = "0.0.0.4"; // Do not modify this line, use file scripts/apply_versions.py
    public const int BuildNumber = 37; // Do not modify this line, use file scripts/apply_versions.py
    public const string UserAgentString = $"PlatyPdfs/{VersionName} (https://trelapps.com; tremorscript@gmail.com)";

    private static bool? IS_PORTABLE;
    private static string? PORTABLE_PATH;
    public static bool IsPortable
    {
        get => IS_PORTABLE ?? false;
    }

    public static string? TEST_DataDirectoryOverride
    {
        private get; set;
    }
    public static HttpClientHandler GenericHttpClientParameters
    {
        get
        {
            return new()
            {
                AutomaticDecompression = DecompressionMethods.All,
                AllowAutoRedirect = true,
            };
        }
    }

    /// <summary>
    /// The directory where all the user data is stored. The directory is automatically created if it does not exist.
    /// </summary>
    public static string PlatyPdfsDataDirectory
    {
        get
        {
            if (TEST_DataDirectoryOverride is not null)
            {
                return TEST_DataDirectoryOverride;
            }

            if (IS_PORTABLE is null)
            {
                IS_PORTABLE = File.Exists(Path.Join(PlatyPdfsExecutableDirectory, "ForcePlatyPdfsPortable"));

                if (IS_PORTABLE is true)
                {
                    string path = Path.Join(PlatyPdfsExecutableDirectory, "Settings");
                    try
                    {
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        var testfilepath = Path.Join(path, "PermissionTestFile");
                        File.WriteAllText(testfilepath, "https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                        PORTABLE_PATH = path;
                        return path;
                    }
                    catch (Exception)
                    {
                        IS_PORTABLE = false;
                        //Logger.Error(
                        //    $"Could not acces/write path {path}. UniGetUI will NOT be run in portable mode, and User settings will be used instead");
                        //Logger.Error(ex);
                    }
                }
            }
            else if (IS_PORTABLE is true)
            {
                return PORTABLE_PATH ?? throw new Exception("This shouldn't be possible");
            }

            var new_path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PlatyPdfs");
            if (!Directory.Exists(new_path)) Directory.CreateDirectory(new_path);
            return new_path;
        }
    }

    /// <summary>
    /// The ID of the notification that is used to inform the user that updates are available
    /// </summary>
    public const int UpdatesAvailableNotificationTag = 1234;
    /// <summary>
    /// The ID of the notification that is used to inform the user that UniGetUI can be updated
    /// </summary>
    public const int UniGetUICanBeUpdated = 1235;
    /// <summary>
    /// The ID of the notification that is used to inform the user that shortcuts are available for deletion
    /// </summary>
    public const int NewShortcutsNotificationTag = 1236;
    /// <summary>
    /// A path pointing to the location where the app is installed
    /// </summary>
    public static string PlatyPdfsExecutableDirectory
    {
        get
        {
            string? dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (dir is not null)
            {
                return dir;
            }

            //Logger.Error("System.Reflection.Assembly.GetExecutingAssembly().Location returned an empty path");

            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PlatyPdfs");
        }
    }

}
