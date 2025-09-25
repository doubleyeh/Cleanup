using Avalonia.Controls;
using Avalonia.Interactivity;
using Cleanup.Models;
using Cleanup.Utils;
using Cleanup.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private async void CleanupButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                // 检查是否有选中需要管理员权限的项目
                var needAdminItems = vm.Items.Where(i => i.IsSelected && i.RequireAdmin).ToList();

                if (needAdminItems.Count != 0 && !CleanupItem.IsAdmin)
                {
                    // 弹窗提示权限不足
                    var mbParams = new MessageBoxStandardParams
                    {
                        ContentTitle = "权限不足",
                        ContentMessage = "选中的项目需要管理员权限才能清理。",
                        ButtonDefinitions = ButtonEnum.Ok,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };

                    await MessageBoxManager.GetMessageBoxStandard(mbParams).ShowWindowAsync();

                    return; // 没权限就不清理
                }

                await vm.CleanupSelectedItemsAsync();
            }
        }

    }
}