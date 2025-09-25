using Cleanup.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cleanup.Utils
{
    public static class ConfigLoader
    {
        public static CleanupConfig LoadCleanupItems()
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, "cleanup.config.json");

            if (!File.Exists(configPath))
                throw new FileNotFoundException("配置文件缺失: " + configPath);

            var json = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<CleanupConfig>(json);

            if (config == null)
            {
                config = new CleanupConfig
                {
                    CleanupItems = []
                };
                return config;
            }

            // 展开环境变量
            foreach (var item in config.CleanupItems)
            {
                item.FullPath = Environment.ExpandEnvironmentVariables(item.FullPath);
            }

            return config;
        }
    }
}
