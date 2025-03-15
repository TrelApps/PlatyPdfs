using System.Threading;
using System.Threading.Tasks;
using PlatyPdfs.App.Core.Classes;
using Windows.Networking.Connectivity;

namespace PlatyPdfs.App.Core.Tools;

public static class CoreTools
{

    /// <summary>
    /// Pings the update server and 3 well-known sites to check for internet availability
    /// </summary>
    public static async Task WaitForInternetConnection()
        => await TaskRecycler<int>.RunOrAttachAsync_VOID(_waitForInternetConnection);

    public static void _waitForInternetConnection()
    {
        //if (Settings.Get("DisableWaitForInternetConnection")) return;

        //Logger.Debug("Checking for internet connectivity...");
        bool internetLost = false;

        var profile = NetworkInformation.GetInternetConnectionProfile();
        while (profile is null || profile.GetNetworkConnectivityLevel() is not NetworkConnectivityLevel.InternetAccess)
        {
            Thread.Sleep(1000);
            profile = NetworkInformation.GetInternetConnectionProfile();
            if (!internetLost)
            {
                //Logger.Warn("User is not connected to the internet, waiting for an internet connectio to be available...");
                internetLost = true;
            }
        }
        //Logger.Debug("Internet connectivity was established.");
    }
}
