﻿using System;

using SlotCarsGo_GUI.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo_GUI.Views
{
    public sealed partial class RaceTypeSelectPage : Page
    {
        private RaceTypeSelectViewModel ViewModel
        {
            get { return DataContext as RaceTypeSelectViewModel; }
        }

        public RaceTypeSelectPage()
        {
            InitializeComponent();
            Loaded += RaceTypeSelectPage_Loaded;
        }

        private async void RaceTypeSelectPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
