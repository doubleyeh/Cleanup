using Cleanup.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleanup.Models
{
    public class CleanupItem : ReactiveObject
    {
        // 使用 ReactiveUI 的 RaiseAndSetIfChanged 机制
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _fullPath = "";
        public string FullPath
        {
            get => _fullPath;
            set => this.RaiseAndSetIfChanged(ref _fullPath, value);
        }

        private string _size = "";
        public string Size
        {
            get => _size;
            set => this.RaiseAndSetIfChanged(ref _size, value);
        }


        // 是否需要管理员权限
        public bool RequireAdmin { get; set; }

        // 是否可以勾选
        public bool CanEdit => !RequireAdmin || IsAdmin;

        public static bool IsAdmin { get; set; } = AppUtil.IsRunningAsAdministrator();
    }
}
