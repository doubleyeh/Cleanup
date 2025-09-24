using Avalonia.Controls;
using Avalonia.Interactivity;
using Cleanup.Models;
using Cleanup.Utils;
using Cleanup.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System.Collections.Generic;

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

        private async void CheckBox_OnClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is CleanupItem item)
            {
                if (item.RequireAdmin && !CleanupItem.IsAdmin)
                {
                    // 取消勾选
                    cb.IsChecked = false;

                    // 弹窗提示
                    var mbParams = new MessageBoxStandardParams
                    {
                        ContentTitle = "权限提示",
                        ContentMessage = $"⚠️ 需要管理员权限才能操作 {item.Name}。\n是否以管理员权限重新运行？",
                        ButtonDefinitions = ButtonEnum.YesNo,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };

                    var result = await MessageBoxManager.GetMessageBoxStandard(mbParams).ShowWindowAsync();
                    if (result == ButtonResult.Yes)
                    {
                        AppUtil.RestartAsAdministrator();
                    }
                }
            }
        }

    }
}