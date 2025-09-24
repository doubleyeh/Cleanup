using Avalonia.Controls;
using Avalonia.Interactivity;
using Cleanup.Models;
using Cleanup.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cleanup.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private async void ScanButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                await vm.ScanSelectedItemsAsync();
            }
        }
    }
}