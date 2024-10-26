using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class Project
    {
        public Project()
        {
            Folders = new HashSet<Folder>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Folder> Folders { get; set; }
    }
}
