﻿using Microsoft.UI.Xaml.Controls;

using PlatyPdfs.App.ViewModels;

namespace PlatyPdfs.App.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();
    }

    //private void OnViewStateChanged(object sender, ListDetailsViewState e)
    //{
    //    if (e == ListDetailsViewState.Both)
    //    {
    //        ViewModel.EnsureItemSelected();
    //    }
    //}
}
