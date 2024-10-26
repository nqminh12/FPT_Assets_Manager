using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class AssetLog
    {
        public int LogId { get; set; }
        public int? AssetId { get; set; }
        public string Action { get; set; } = null!;
        public string? ActionDetails { get; set; }
        public string? OldPath { get; set; }
        public string? NewPath { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual Asset? Asset { get; set; }
    }
}
