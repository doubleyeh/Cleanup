using Cleanup.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cleanup.Models
{
    public class CleanupItem : ReactiveObject
    {
        public bool IsSelected { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("path")]
        public required string FullPath { get; set; }

        private string _size = "";
        public string Size
        {
            get => _size;
            set => this.RaiseAndSetIfChanged(ref _size, value);
        }


        // 是否需要管理员权限
        [JsonPropertyName("requireAdmin")]
        public bool RequireAdmin { get; set; }

        // 是否可以勾选
        public bool CanEdit => !RequireAdmin || IsAdmin;

        public static bool IsAdmin { get; set; } = AppUtil.IsRunningAsAdministrator();
    }
}
