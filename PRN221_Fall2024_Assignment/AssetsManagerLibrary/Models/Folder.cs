using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class Folder
    {
        public Folder()
        {
            Assets = new HashSet<Asset>();
        }

        public int FolderId { get; set; }
        public int? ProjectId { get; set; }
        public string FolderName { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? AssetType { get; set; }

        public virtual Project? Project { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}
