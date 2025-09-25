using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cleanup.Models
{
    public class CleanupConfig
    {
        [JsonPropertyName("cleanupItems")]
        public List<CleanupItem> CleanupItems { get; set; } = [];
    }
}
