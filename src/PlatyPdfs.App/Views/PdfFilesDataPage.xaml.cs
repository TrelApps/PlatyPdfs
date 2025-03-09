using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PlatyPdfs.App.Core.Models;
using PlatyPdfs.App.Extensions;
using PlatyPdfs.App.ViewModels;
using Windows.Storage.Pickers;

namespace PlatyPdfs.App.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class PdfFilesDataPage : Page
{
    public PdfFilesDataViewModel ViewModel
    {
        get;
    }

    public PdfFilesDataPage()
    {
        ViewModel = App.GetService<PdfFilesDataViewModel>();
        InitializeComponent();
    }

    private async void PickAFileButton_Click(object sender, RoutedEventArgs e)
    {
        //disable the button to avoid double-clicking
        var senderButton = sender as Button;
        senderButton.IsEnabled = false;

        // Create a file picker
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        // See the sample code below for how to make the window accessible from the App class.
        var window = App.MainWindow;

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your file picker
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.FileTypeFilter.Add(".pdf");

        // Open the picker for the user to pick a file
        var files = await openPicker.PickMultipleFilesAsync();
        if (files.ToList().Count > 0)
        {
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file.Path);
                ViewModel.Source.Add(new PdfFile
                {
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length.ToSizeString(),
                    FilePath = Path.GetDirectoryName(fileInfo.FullName),
                    DateModified = fileInfo.LastWriteTime.ToString("dd-MMM-yyy hh:mm t"),
                    OrderNumber = 12345
                });
            }
        }

        //re-enable the button
        senderButton.IsEnabled = true;
    }
}
