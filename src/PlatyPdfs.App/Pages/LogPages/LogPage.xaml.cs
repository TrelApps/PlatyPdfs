using System.Diagnostics;
using ExternalLibraries.Clipboard;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using PlatyPdfs.App.Core.Logging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatyPdfs.App.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LogPage : Page, IKeyboardShortcutListener, IEnterLeaveListener
{
    protected int LOG_LEVEL = 4;

    public LogPage()
    {
        this.InitializeComponent();
        LoadLogLevels();

        ActualThemeChanged += (_, _) => LoadLog();
    }

    protected void LoadLogLevels()
    {
        LogLevelCombo.Items.Clear();
        LogLevelCombo.Items.Add(String.Format("1 - Errors"));
        LogLevelCombo.Items.Add(String.Format("2 - Warnings"));
        LogLevelCombo.Items.Add(String.Format("3 - Information (less)"));
        LogLevelCombo.Items.Add(String.Format("4 - Information (more)"));
        LogLevelCombo.Items.Add(String.Format("5 - information (debug)"));
        LogLevelCombo.SelectedIndex = 3;
    }

    public void LoadLog(bool isReload = false)
    {
        bool IS_DARK = ActualTheme == Microsoft.UI.Xaml.ElementTheme.Dark;

        LogEntry[] logs = Logger.GetLogs();
        LogTextBox.Blocks.Clear();
        foreach (LogEntry log_entry in logs)
        {
            Paragraph p = new();
            if (log_entry.Content == "")
            {
                continue;
            }

            if (LOG_LEVEL == 1 && (log_entry.Severity == LogEntry.SeverityLevel.Debug || log_entry.Severity == LogEntry.SeverityLevel.Info || log_entry.Severity == LogEntry.SeverityLevel.Success || log_entry.Severity == LogEntry.SeverityLevel.Warning))
            {
                continue;
            }

            if (LOG_LEVEL == 2 && (log_entry.Severity == LogEntry.SeverityLevel.Debug || log_entry.Severity == LogEntry.SeverityLevel.Info || log_entry.Severity == LogEntry.SeverityLevel.Success))
            {
                continue;
            }

            if (LOG_LEVEL == 3 && (log_entry.Severity == LogEntry.SeverityLevel.Debug || log_entry.Severity == LogEntry.SeverityLevel.Info))
            {
                continue;
            }

            if (LOG_LEVEL == 4 && (log_entry.Severity == LogEntry.SeverityLevel.Debug))
            {
                continue;
            }

            Brush color = log_entry.Severity switch
            {
                LogEntry.SeverityLevel.Debug => new SolidColorBrush { Color = IS_DARK ? DARK_GREY : LIGHT_GREY },
                LogEntry.SeverityLevel.Info => new SolidColorBrush { Color = IS_DARK ? DARK_LIGHT_GREY : LIGHT_LIGHT_GREY },
                LogEntry.SeverityLevel.Success => new SolidColorBrush { Color = IS_DARK ? DARK_WHITE : LIGHT_WHITE },
                LogEntry.SeverityLevel.Warning => new SolidColorBrush { Color = IS_DARK ? DARK_YELLOW : LIGHT_YELLOW },
                LogEntry.SeverityLevel.Error => new SolidColorBrush { Color = IS_DARK ? DARK_RED : LIGHT_RED },
                _ => new SolidColorBrush { Color = IS_DARK ? DARK_GREY : LIGHT_GREY },
            };
            string[] lines = log_entry.Content.Split('\n');
            int date_length = -1;
            foreach (string line in lines)
            {
                if (date_length == -1)
                {
                    p.Inlines.Add(new Run { Text = $"[{log_entry.Time}] {line}\n", Foreground = color });
                    date_length = $"[{log_entry.Time}] ".Length;
                }
                else
                {
                    p.Inlines.Add(new Run { Text = new string(' ', date_length) + line + "\n", Foreground = color });
                }
            } ((Run)p.Inlines[^1]).Text = ((Run)p.Inlines[^1]).Text.TrimEnd();
            LogTextBox.Blocks.Add(p);
        }
        if (isReload) MainScroller.ScrollToVerticalOffset(MainScroller.ScrollableHeight);
    }

    protected void SelectLogLevelByName(string name)
    {
        LogLevelCombo.SelectedValue = name;
    }

    public void ReloadTriggered()
        => LoadLog(isReload: true);

    public void SelectAllTriggered()
        => LogTextBox.SelectAll();

    public void SearchTriggered()
    {
    }

    // Dark theme colors
    protected Color DARK_GREY = Color.FromArgb(255, 130, 130, 130);
    protected Color DARK_LIGHT_GREY = Color.FromArgb(255, 190, 190, 190);
    protected Color DARK_WHITE = Color.FromArgb(255, 250, 250, 250);
    protected Color DARK_YELLOW = Color.FromArgb(255, 255, 255, 90);
    protected Color DARK_RED = Color.FromArgb(255, 255, 80, 80);
    protected Color DARK_GREEN = Color.FromArgb(255, 80, 255, 80);
    protected Color DARK_BLUE = Color.FromArgb(255, 120, 120, 255);

    // Light theme colors
    protected Color LIGHT_GREY = Color.FromArgb(255, 125, 125, 225);
    protected Color LIGHT_LIGHT_GREY = Color.FromArgb(255, 50, 50, 150);
    protected Color LIGHT_WHITE = Color.FromArgb(255, 0, 0, 0);
    protected Color LIGHT_YELLOW = Color.FromArgb(255, 150, 150, 0);
    protected Color LIGHT_RED = Color.FromArgb(255, 205, 0, 0);
    protected Color LIGHT_GREEN = Color.FromArgb(255, 0, 205, 0);
    protected Color LIGHT_BLUE = Color.FromArgb(255, 0, 0, 205);

    public void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        LogTextBox.SelectAll();
        WindowsClipboard.SetText(LogTextBox.SelectedText);
        LogTextBox.Select(LogTextBox.SelectionStart, LogTextBox.SelectionStart);
    }

    public async void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        FileSavePicker savePicker = new()
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary
        };
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
        savePicker.FileTypeChoices.Add(String.Format("Text"), [".txt"]);
        savePicker.SuggestedFileName = String.Format("PlatyPdfs Log");

        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file is not null)
        {
            LogTextBox.SelectAll();
            await File.WriteAllTextAsync(file.Path, LogTextBox.SelectedText);
            LogTextBox.Select(LogTextBox.SelectionStart, LogTextBox.SelectionStart);
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = @$"/select, ""{file.Path}"""
            });
        }
    }

    public void ReloadButton_Click(object sender, RoutedEventArgs e)
        => LoadLog();

    private void LogLevelCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LOG_LEVEL = LogLevelCombo.SelectedIndex + 1;
        LoadLog();
    }

    public void OnEnter()
        => LoadLog();

    public void OnLeave()
        => LogTextBox.Blocks.Clear();
}
