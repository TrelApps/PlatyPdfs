using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using PlatyPdfs.App.Contracts.ViewModels;
using PlatyPdfs.App.Core.Contracts.Services;
using PlatyPdfs.App.Core.Models;

namespace PlatyPdfs.App.ViewModels;

public partial class PdfFilesDataViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPdfFileService _pdfFileService;

    public ObservableCollection<PdfFile> Source { get; } = new ObservableCollection<PdfFile>();

    public PdfFilesDataViewModel(IPdfFileService pdfFileService)
    {
        _pdfFileService = pdfFileService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _pdfFileService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
