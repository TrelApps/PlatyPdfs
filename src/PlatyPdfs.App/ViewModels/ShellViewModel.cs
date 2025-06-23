using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;


using PlatyPdfs.App.Contracts.Services;

namespace PlatyPdfs.App.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    public ICommand MenuFileExitCommand
    {
        get;
    }

    public ICommand MenuViewsPdfFilesDataCommand
    {
        get;
    }

    public ICommand MenuSettingsCommand
    {
        get;
    }

    public ICommand MenuViewsListDetailsCommand
    {
        get;
    }

    public ICommand MenuViewsContentGridCommand
    {
        get;
    }

    public ICommand MenuViewsDataGridCommand
    {
        get;
    }

    public ICommand MenuViewsMainCommand
    {
        get;
    }

    public ICommand ItemInvokedCommand
    {
        get;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;

        MenuFileExitCommand = new RelayCommand(OnMenuFileExit);
        //MenuViewsPdfFilesDataCommand = new RelayCommand(OnMenuViewsPdfFilesData);
        //MenuSettingsCommand = new RelayCommand(OnMenuSettings);
        //MenuViewsListDetailsCommand = new RelayCommand(OnMenuViewsListDetails);
        //MenuViewsContentGridCommand = new RelayCommand(OnMenuViewsContentGrid);
        //MenuViewsDataGridCommand = new RelayCommand(OnMenuViewsDataGrid);
        //MenuViewsMainCommand = new RelayCommand(OnMenuViewsMain);
        ItemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked);
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnMenuFileExit() => Application.Current.Exit();

    private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
    {
        NavigateToFromMenu((string)args.InvokedItemContainer.Tag!);
    }

    public void NavigateToFromMenu(string menuText)
    {
        var type = menuText switch
        {
            "MainViewModel" => typeof(MainViewModel),
            "PdfFilesDataViewModel" => typeof(MergePdfsViewModel),
            "LogViewModel" => typeof(LogViewModel),
            "Settings" => typeof(SettingsViewModel),
            _ => typeof(MainViewModel)
        };
        NavigationService.NavigateTo(type.FullName!);
    }
}
