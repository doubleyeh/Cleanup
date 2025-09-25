using Cleanup.Models;
using Cleanup.Utils;
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
        public ObservableCollection<CleanupItem> Items { get; } = new ObservableCollection<CleanupItem>();

        public MainWindowViewModel() {
            CleanupConfig config = ConfigLoader.LoadCleanupItems();
            foreach (var item in config.CleanupItems)
            {
                item.Size = "";
                item.IsSelected = !item.RequireAdmin || CleanupItem.IsAdmin;
                Items.Add(item);
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
        private static long GetDirectorySize(string folderPath)
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
        private static string FormatBytes(long bytes)
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


        public async Task CleanupSelectedItemsAsync()
        {
            foreach (var item in Items.Where(i => i.IsSelected))
            {
                item.Size = "处理中...";
                try
                {
                    if (Directory.Exists(item.FullPath))
                    {
                        await Task.Run(() => DeleteDirectoryRecursive(item.FullPath));
                    }
                    else if (File.Exists(item.FullPath))
                    {
                        await Task.Run(() => File.Delete(item.FullPath));
                    }
                    else
                    {
                        item.Size = "不存在";
                        continue;
                    }

                    item.Size = "已清理";
                }
                catch (UnauthorizedAccessException)
                {
                    item.Size = "需要管理员权限";
                }
                catch (Exception ex)
                {
                    item.Size = "清理失败";
                    Console.WriteLine($"清理失败: {item.FullPath} -> {ex.Message}");
                }
            }
        }

        private static void DeleteDirectoryRecursive(string dir)
        {
            foreach (var subDir in Directory.EnumerateDirectories(dir))
            {
                DeleteDirectoryRecursive(subDir);
            }

            foreach (var file in Directory.EnumerateFiles(dir))
            {
                File.Delete(file);
            }

            Directory.Delete(dir);
        }

    }
}
