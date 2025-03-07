using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using PlatyPdfs.App.Contracts.ViewModels;
using PlatyPdfs.App.Core.Contracts.Services;
using PlatyPdfs.App.Core.Models;

namespace PlatyPdfs.App.ViewModels;

public partial class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public DataGridViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
