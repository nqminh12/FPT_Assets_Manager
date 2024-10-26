using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class AssetPermission
    {
        public int PermissionId { get; set; }
        public int? RoleId { get; set; }
        public int? AssetId { get; set; }
        public bool? CanView { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }

        public virtual Asset? Asset { get; set; }
        public virtual UserRole? Role { get; set; }
    }
}
