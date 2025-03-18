using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PlatyPdfs.App.Contracts.Services;
using PlatyPdfs.App.Helpers;
using PlatyPdfs.App.Pages.AboutPages;
using PlatyPdfs.App.ViewModels;

using Windows.System;

namespace PlatyPdfs.App.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    public Microsoft.UI.Xaml.Controls.NavigationView NavigationView
    {
        get
        {
            return NavigationViewControl;
        }
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();


        _ = AutoUpdater.UpdateCheckLoopAsync(App.MainWindow, UpdatesBanner);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));

        //ShellMenuBarSettingsButton.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerPressed), true);
        //ShellMenuBarSettingsButton.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerReleased), true);
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        //ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerPressedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerPressed);
        //ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerReleased);
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    //private void ShellMenuBarSettingsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
    //{
    //    AnimatedIcon.SetState((UIElement)sender, "PointerOver");
    //}

    //private void ShellMenuBarSettingsButton_PointerPressed(object sender, PointerRoutedEventArgs e)
    //{
    //    AnimatedIcon.SetState((UIElement)sender, "Pressed");
    //}

    //private void ShellMenuBarSettingsButton_PointerReleased(object sender, PointerRoutedEventArgs e)
    //{
    //    AnimatedIcon.SetState((UIElement)sender, "Normal");
    //}

    //private void ShellMenuBarSettingsButton_PointerExited(object sender, PointerRoutedEventArgs e)
    //{
    //    AnimatedIcon.SetState((UIElement)sender, "Normal");
    //}

    private async void AboutNavButton_Click(object sender, RoutedEventArgs e)
    {
        ContentDialog? AboutDialog = new();
        AboutPlatyPdfs AboutPage = new();
        AboutDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        AboutDialog.XamlRoot = XamlRoot;
        AboutDialog.Resources["ContentDialogMaxWidth"] = 1200;
        AboutDialog.Resources["ContentDialogMaxHeight"] = 1000;
        AboutDialog.Content = AboutPage;
        AboutDialog.PrimaryButtonText = "Close";
        AboutPage.Close += (s, e) => { AboutDialog.Hide(); };
        AboutDialog.RequestedTheme = (ElementTheme)Application.Current.RequestedTheme;
        await AboutDialog.ShowAsync();
        AboutDialog.Content = null;
        AboutDialog = null;
    }

    private void OnControlsSearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            var hasChangedSelection = EnsureItemIsVisibleInNavigation((string)args.ChosenSuggestion);
        }
    }

    public bool EnsureItemIsVisibleInNavigation(string name)
    {
        bool changedSelection = false;
        foreach (object rawItem in NavigationView.MenuItems)
        {
            // Check if we encountered the separator
            if (!(rawItem is NavigationViewItem))
            {
                // Skipping this item
                continue;
            }

            var item = rawItem as NavigationViewItem;

            // Check if we are this category
            if ((string)item.Content == name)
            {
                NavigationView.SelectedItem = item;
                ViewModel.NavigateToFromMenu((string)item.Tag);
                changedSelection = true;
            }
            // We are not :/
            else
            {
                // Maybe one of our items is?
                if (item.MenuItems.Count != 0)
                {
                    foreach (NavigationViewItem child in item.MenuItems)
                    {
                        if ((string)child.Content == name)
                        {
                            // We are the item corresponding to the selected one, update selection!

                            // Deal with differences in displaymodes
                            if (NavigationView.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
                            {
                                // In Topmode, the child is not visible, so set parent as selected
                                // Everything else does not work unfortunately
                                NavigationView.SelectedItem = item;
                                ViewModel.NavigateToFromMenu((string)item.Tag);
                                item.StartBringIntoView();
                            }
                            else
                            {
                                // Expand so we animate
                                item.IsExpanded = true;
                                // Ensure parent is expanded so we actually show the selection indicator
                                NavigationView.UpdateLayout();
                                // Set selected item
                                NavigationView.SelectedItem = child;
                                ViewModel.NavigateToFromMenu((string)child.Tag);
                                child.StartBringIntoView();
                            }
                            // Set to true to also skip out of outer for loop
                            changedSelection = true;
                            // Break out of child iteration for loop
                            break;
                        }
                    }
                }
            }
            // We updated selection, break here!
            if (changedSelection)
            {
                break;
            }
        }
        return changedSelection;
    }

    private void OnControlsSearchBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suggestions = new List<string>();
            var querySplit = sender.Text;


            foreach (object rawItem in NavigationView.MenuItems)
            {
                // Check if we encountered the separator
                if (!(rawItem is NavigationViewItem))
                {
                    // Skipping this item
                    continue;
                }

                var item = rawItem as NavigationViewItem;

                // Check if we are this category
                if (((string)item.Content).StartsWith(querySplit))
                {
                    suggestions.Add((string)item.Content);
                }
                if (item.MenuItems.Count != 0)
                {
                    foreach (NavigationViewItem child in item.MenuItems)
                    {

                        if (((string)child.Content).StartsWith(querySplit))
                        {
                            suggestions.Add((string)child.Content);
                        }
                    }

                }
            }

            if (suggestions.Count > 0)
            {
                controlsSearchBox.ItemsSource = suggestions.OrderByDescending(i => i);
            }
            else
            {
                controlsSearchBox.ItemsSource = new string[] { "No results found" };
            }
        }
    }



    private void CtrlF_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        controlsSearchBox.Focus(FocusState.Programmatic);
    }
}
