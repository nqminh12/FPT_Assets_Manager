using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class Folder
    {
        public Folder()
        {
            Assets = new HashSet<Asset>();
            Projects = new HashSet<Project>();
        }

        public int FolderId { get; set; }
        public string FolderName { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? AssetType { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
