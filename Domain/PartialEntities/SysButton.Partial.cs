namespace Domain.Entities
{
    public partial class SysButton
    {
        public virtual SysRoute Route { get; set; }
        public virtual ICollection<SysButtonpermission> ButtonPermissions { get; set; } = new HashSet<SysButtonpermission>();
        public virtual ICollection<SysPermissiondelegation> Delegations { get; set; } = new HashSet<SysPermissiondelegation>();
    }
}
