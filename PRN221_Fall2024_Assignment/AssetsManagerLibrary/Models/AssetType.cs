using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class AssetType
    {
        public AssetType()
        {
            Assets = new HashSet<Asset>();
        }

        public int TypeId { get; set; }
        public string TypeDescription { get; set; } = null!;

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
