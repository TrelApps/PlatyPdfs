using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using PlatyPdfs.App.Contracts.Services;
using PlatyPdfs.App.Contracts.ViewModels;
using PlatyPdfs.App.Core.Contracts.Services;
using PlatyPdfs.App.Core.Models;

namespace PlatyPdfs.App.ViewModels;

public partial class ContentGridDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private SampleOrder? item;

    public ICommand GoBackCommand
    {
        get;
    }

    public ContentGridDetailViewModel(ISampleDataService sampleDataService, INavigationService navigationService)
    {
        _sampleDataService = sampleDataService;
        _navigationService = navigationService;

        GoBackCommand = new RelayCommand(OnGoBack);
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnGoBack()
    {
        if (_navigationService.CanGoBack)
        {
            _navigationService.GoBack();
        }
    }
}
