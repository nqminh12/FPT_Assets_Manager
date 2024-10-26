using System;
using System.Collections.Generic;

namespace AssetsManagerLibrary.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            AssetPermissions = new HashSet<AssetPermission>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<AssetPermission> AssetPermissions { get; set; }
    }
}
