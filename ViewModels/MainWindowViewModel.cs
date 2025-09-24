using Cleanup.Models;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cleanup.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<CleanupItem> Items { get; } = [];

        public MainWindowViewModel() {
            FillCleanupItems();
        }

        private void FillCleanupItems()
        {
            // 临时文件夹
            var tempPath = Environment.GetEnvironmentVariable("TEMP");
            if (!string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath))
            {
                Items.Add(new CleanupItem
                {
                    IsSelected = true,
                    Name = "临时文件 (%TEMP%)",
                    FullPath = tempPath,
                    Size = ""
                });
            }

            // 系统临时文件
            var windowsTemp = @"C:\Windows\Temp";
            if (Directory.Exists(windowsTemp))
            {
                Items.Add(new CleanupItem
                {
                    IsSelected = false,
                    Name = "系统临时文件",
                    FullPath = windowsTemp,
                    Size = "",
                    RequireAdmin = true
                });
            }

            // 缩略图缓存
            var thumbCache = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Explorer");
            if (Directory.Exists(thumbCache))
            {
                Items.Add(new CleanupItem
                {
                    IsSelected = false,
                    Name = "缩略图缓存",
                    FullPath = thumbCache,
                    Size = ""
                });
            }

            // Windows 更新缓存
            var windowsUpdateCache = @"C:\Windows\SoftwareDistribution\Download";
            if (Directory.Exists(windowsUpdateCache))
            {
                Items.Add(new CleanupItem
                {
                    IsSelected = false,
                    Name = "Windows 更新缓存",
                    FullPath = windowsUpdateCache,
                    Size = "",
                    RequireAdmin = true
                });
            }

            // 日志文件目录
            var logFolder = @"C:\Windows\Logs";
            if (Directory.Exists(logFolder))
            {
                Items.Add(new CleanupItem
                {
                    IsSelected = false,
                    Name = "日志文件 (*.log)",
                    FullPath = logFolder,
                    Size = "",
                    RequireAdmin = true
                });
            }
        }


        // 扫描选中的项目
        public async Task ScanSelectedItemsAsync()
        {
            foreach (var item in Items.Where(i => i.IsSelected))
            {
                if (Directory.Exists(item.FullPath))
                {
                    try
                    {
                        // 异步扫描目录大小
                        long totalBytes = await Task.Run(() => GetDirectorySize(item.FullPath));
                        item.Size = FormatBytes(totalBytes);
                    }
                    catch (Exception ex)
                    {
                        item.Size = "扫描失败";
                    }
                }
                else
                {
                    item.Size = "目录不存在";
                }
            }
        }

        // 递归计算目录大小
        private long GetDirectorySize(string folderPath)
        {
            long size = 0;

            // 所有文件大小累加
            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var info = new FileInfo(file);
                    size += info.Length;
                }
                catch { /* 忽略无法访问的文件 */ }
            }

            return size;
        }

        // 格式化字节大小
        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
