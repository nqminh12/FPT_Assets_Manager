using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class Category
    {
        public Category()
        {
            Assets = new HashSet<Asset>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
