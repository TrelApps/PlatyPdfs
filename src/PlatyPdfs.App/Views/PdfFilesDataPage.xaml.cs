using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PlatyPdfs.App.Core.Models;
using PlatyPdfs.App.Extensions;
using PlatyPdfs.App.ViewModels;
using Windows.Storage;
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
                    DirectoryPath = Path.GetDirectoryName(fileInfo.FullName),
                    FilePath = fileInfo.FullName,
                    DateModified = fileInfo.LastWriteTime.ToString("dd-MMM-yyy hh:mm t"),
                    OrderNumber = 12345
                });
            }
        }

        //re-enable the button
        senderButton.IsEnabled = true;
    }

    private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedFiles = ((DataGrid)sender).SelectedItems;
        ViewModel.SelectedFiles = [];

        foreach (var file in selectedFiles)
        {
            ViewModel.SelectedFiles.Add((PdfFile)file);
        }

        ViewModel.IsMergeEnabled = selectedFiles.Count > 1;

    }

    private async void MergePdfsButton_Click(object sender, RoutedEventArgs e)
    {

        var fileBytes = ViewModel.SelectedFiles.Select(x => File.ReadAllBytes(x.FilePath)).ToArray();
        var bytes = Docnet.Core.DocLib.Instance.Merge(fileBytes);

        FileSavePicker savePicker = new FileSavePicker();
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        // Dropdown of file types the user can save the file as
        savePicker.FileTypeChoices.Add("Pdf", new List<string>() { ".pdf" });
        // Default file name if the user does not type one in or select a file to replace
        savePicker.SuggestedFileName = "New Document";

        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
            //CachedFileManager.DeferUpdates(file);
            // write to file
            await FileIO.WriteBytesAsync(file, bytes);
            // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
            // Completing updates may require Windows to ask for user input.
            //FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            //if (status == FileUpdateStatus.Complete)
            //{
            //   OutputTextBlock.Text = "File " + file.Name + " was saved.";
            //}
            //else
            //{
            //  OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
            //}
        }
        else
        {
            //OutputTextBlock.Text = "Operation cancelled.";
        }
    }


    //public void CopyButton_Click(object sender, RoutedEventArgs e)
    //{
    //    LogTextBox.SelectAll();
    //    WindowsClipboard.SetText(LogTextBox.SelectedText);
    //    LogTextBox.Select(LogTextBox.SelectionStart, LogTextBox.SelectionStart);
    //}
}
