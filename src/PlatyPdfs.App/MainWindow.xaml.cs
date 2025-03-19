using Microsoft.UI.Windowing;
using Microsoft.Windows.AppNotifications;
using PlatyPdfs.App.Core.Enums;
using PlatyPdfs.App.Core.Logging;
using PlatyPdfs.App.Helpers;

using Windows.UI.ViewManagement;

namespace PlatyPdfs.App;

public sealed partial class MainWindow : WindowEx
{
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private UISettings settings;

    public static readonly ObservableQueue<string> ParametersToProcess = new();

    public string test = "TEst";

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    // this handles updating the caption button colors correctly when windows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }

    public void HandleNotificationActivation(AppNotificationActivatedEventArgs args)
    {
        args.Arguments.TryGetValue("action", out string? action);
        if (action is null) action = "";

        if (action == NotificationArguments.UpdateAllPackages)
        {
            //NavigationPage.UpdatesPage.UpdateAll();
        }
        else if (action == NotificationArguments.ShowOnUpdatesTab)
        {
            //NavigationPage.NavigateTo(PageType.Updates);
            //Activate();
        }
        else if (action == NotificationArguments.Show)
        {
            //Activate();
        }
        else if (action == NotificationArguments.ReleaseSelfUpdateLock)
        {
            AutoUpdater.ReleaseLockForAutoupdate_Notification = true;
        }
        else
        {
            throw new ArgumentException(
                $"args.Argument was not set to a value present in Enums.NotificationArguments (value is {action})");
        }

        Logger.Debug("Notification activated: " + args.Arguments);
    }

    /// <summary>
    /// Handle the window closing event, and divert it when the window must be hidden.
    /// </summary>
    public void HandleClosingEvent(AppWindow sender, AppWindowClosingEventArgs args)
    {
        AutoUpdater.ReleaseLockForAutoupdate_Window = true;
        //SaveGeometry(Force: true);
        //if (!Settings.Get("DisableSystemTray") || AutoUpdater.UpdateReadyToBeInstalled)
        if (AutoUpdater.UpdateReadyToBeInstalled)
        {
            args.Cancel = true;
            //DWMThreadHelper.ChangeState_DWM(true);
            //DWMThreadHelper.ChangeState_XAML(true);

            //try
            //{
            //    EfficiencyModeUtilities.SetEfficiencyMode(true);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error("Could not disable efficiency mode");
            //    Logger.Error(ex);
            //}

            this.Content = null;
            AppWindow.Hide();
        }
        else
        {
            //if (MainApp.Operations.AreThereRunningOperations())
            //{
            //    args.Cancel = true;
            //    ContentDialog d = new()
            //    {
            //        XamlRoot = NavigationPage.XamlRoot,
            //        Title = CoreTools.Translate("Operation in progress"),
            //        Content =
            //            CoreTools.Translate(
            //                "There are ongoing operations. Quitting WingetUI may cause them to fail. Do you want to continue?"),
            //        PrimaryButtonText = CoreTools.Translate("Quit"),
            //        SecondaryButtonText = CoreTools.Translate("Cancel"),
            //        DefaultButton = ContentDialogButton.Secondary
            //    };

            //    ContentDialogResult result = await ShowDialogAsync(d);
            //    if (result == ContentDialogResult.Primary)
            //    {
            //        MainApp.Instance.DisposeAndQuit();
            //    }
            //}
        }
    }

}
