using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class Asset
    {
        public Asset()
        {
            AssetLogs = new HashSet<AssetLog>();
            AssetPermissions = new HashSet<AssetPermission>();
        }

        public int AssetId { get; set; }
        public string AssetName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public int SizeKb { get; set; }
        public int? TypeId { get; set; }
        public int CategoryId { get; set; }
        public int? CloudId { get; set; }
        public int? FolderId { get; set; }
        public DateTime? ImportedDate { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual CloudStorage? Cloud { get; set; }
        public virtual Folder? Folder { get; set; }
        public virtual AssetType? Type { get; set; }
        public virtual ICollection<AssetLog> AssetLogs { get; set; }
        public virtual ICollection<AssetPermission> AssetPermissions { get; set; }
    }
}
