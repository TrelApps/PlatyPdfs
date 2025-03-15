using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using PlatyPdfs.App.Activation;
using PlatyPdfs.App.Contracts.Services;
using PlatyPdfs.App.Core.Contracts.Services;
using PlatyPdfs.App.Core.Services;
using PlatyPdfs.App.Models;
using PlatyPdfs.App.Services;
using PlatyPdfs.App.ViewModels;
using PlatyPdfs.App.Views;
using Windows.ApplicationModel.Activation;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace PlatyPdfs.App;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static MainWindow MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IPdfFileService, PdfFileService>();

            // Views and ViewModels
            services.AddTransient<PdfFilesDataViewModel>();
            services.AddTransient<PdfFilesDataPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ListDetailsViewModel>();
            services.AddTransient<ListDetailsPage>();
            services.AddTransient<ContentGridDetailViewModel>();
            services.AddTransient<ContentGridDetailPage>();
            services.AddTransient<ContentGridViewModel>();
            services.AddTransient<ContentGridPage>();
            services.AddTransient<DataGridViewModel>();
            services.AddTransient<DataGridPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
        MainWindow.Closed += (_, _) => DisposeAndQuit(0);

        nint hWnd = MainWindow.GetWindowHandle();
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

        if (appWindow is not null)
        {
            appWindow.Closing += MainWindow.HandleClosingEvent;
        }
        RegisterNotificationService();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<IActivationService>().ActivateAsync(args);
    }


    public async Task ShowMainWindowFromRedirectAsync(AppActivationArguments rawArgs)
    {
        while (MainWindow is null)
            await Task.Delay(100);

        ExtendedActivationKind kind = rawArgs.Kind;
        if (kind is ExtendedActivationKind.Launch)
        {
            if (rawArgs.Data is ILaunchActivatedEventArgs launchArguments)
            {
                // If the app redirection event comes from a launch, extract
                // the CLI arguments and redirect them to the ParameterProcessor
                foreach (Match argument in Regex.Matches(launchArguments.Arguments,
                             "([^ \"']+|\"[^\"]+\"|'[^']+')"))
                {
                    MainWindow.ParametersToProcess.Enqueue(argument.Value);
                }
            }
            else
            {
                //Logger.Error("REDIRECTOR ACTIVATOR: args.Data was null when casted to ILaunchActivatedEventArgs");
            }
        }
        else
        {
            //Logger.Warn("REDIRECTOR ACTIVATOR: args.Kind is not Launch but rather " + kind);
        }

        MainWindow.DispatcherQueue.TryEnqueue(MainWindow.Activate);
    }

    /// <summary>
    /// Register the notification activation event
    /// </summary>
    private void RegisterNotificationService()
    {
        try
        {
            AppNotificationManager.Default.NotificationInvoked += (_, args) =>
            {
                MainWindow.DispatcherQueue.TryEnqueue(() =>
                {
                    MainWindow.HandleNotificationActivation(args);
                });
            };
            AppNotificationManager.Default.Register();
        }
        catch (Exception)
        {
            //Logger.Error("Could not register notification event");
            //Logger.Error(ex);
        }
    }


    public void DisposeAndQuit(int outputCode = 0)
    {
        //Logger.Warn("Quitting UniGetUI");
        //DWMThreadHelper.ChangeState_DWM(false);
        //DWMThreadHelper.ChangeState_XAML(false);
        MainWindow?.Close();
        //BackgroundApi?.Stop();
        Exit();
        // await Task.Delay(100);
        // Environment.Exit(outputCode);
    }
}
