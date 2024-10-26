using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class CloudStorage
    {
        public CloudStorage()
        {
            Assets = new HashSet<Asset>();
        }

        public int CloudId { get; set; }
        public string Provider { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
