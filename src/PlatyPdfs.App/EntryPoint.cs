using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using PlatyPdfs.App.Core.Logging;

namespace PlatyPdfs.App;

public static class EntryPoint
{
    [STAThread]
    private static void Main(string[] args)
    {
        // Having an async main method breaks WebView2
        try
        {
            if (args.Contains("--uninstall-platypdfs") || args.Contains("--uninstall-platypdfs"))
            {
                // If the app is being uninstalled, run the cleaner and exit
                return;
            }
            else
            {
                // Otherwise, run UniGetUI as normal
                _ = AsyncMain();
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    private static async Task AsyncMain()
    {

        // WinRT single-instance fancy stuff
        WinRT.ComWrappersSupport.InitializeComWrappers();
        bool isRedirect = await DecideRedirection();
        // If this is the main instance, start the app
        if (!isRedirect)
        {
            Microsoft.UI.Xaml.Application.Start((_) =>
            {
                DispatcherQueueSynchronizationContext context = new(
                    DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                var app = new App();
            });
        }
    }

    /// <summary>
    /// Default WinUI Redirector
    /// </summary>
    private static async Task<bool> DecideRedirection()
    {
        try
        {
            // IDK how does this work, I copied it from the MS Docs
            // example on single-instance apps using unpackaged AppSdk + WinUI3
            bool isRedirect = false;

            var keyInstance = AppInstance.FindOrRegisterForKey("TrelApps.PlatyPdfs.MainInterface");

            if (keyInstance.IsCurrent)
            {
                keyInstance.Activated += async (_, e) =>
                {
                    if (App.Current is App baseInstance)
                    {
                        await baseInstance.ShowMainWindowFromRedirectAsync(e);
                    }
                };
            }
            else
            {
                isRedirect = true;
                AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
                await keyInstance.RedirectActivationToAsync(args);
            }
            return isRedirect;
        }
        catch (Exception e)
        {
            Logger.Warn(e);
            return false;
        }
    }
}
