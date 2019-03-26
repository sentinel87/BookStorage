﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using BookStorage.ViewModels;
using BookStorage.Views;

namespace BookStorage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainViewModel viewModel = new MainViewModel();
            MainView view = new MainView();
            view.DataContext = viewModel;
            view.Show();
        }
    }
}
