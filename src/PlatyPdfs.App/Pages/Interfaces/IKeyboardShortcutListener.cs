﻿namespace PlatyPdfs.App.Pages;

/// <summary>
/// Any object that can perform any of the following listed actions should
/// implement this class, to allow proper keyboard bindings on the interface.
/// </summary>
internal interface IKeyboardShortcutListener
{
    /// <summary>
    /// Handles when a search-like automation was triggered (Ctrl+F, etc.)
    /// </summary>
    void SearchTriggered();

    /// <summary>
    /// Handles when a reload/refresh-like automation was triggered (F5, Ctrl+R, etc)
    /// </summary>
    void ReloadTriggered();

    /// <summary>
    /// Handles when a select-like automation was triggered (Ctrl+A, etc.)
    /// </summary>
    void SelectAllTriggered();
}
