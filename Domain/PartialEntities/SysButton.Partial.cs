namespace Domain.Entities
{
    public partial class SysButton
    {
        public virtual SysRoute Route { get; set; }
        public virtual ICollection<SysButtonPermission> ButtonPermissions { get; set; } = new HashSet<SysButtonPermission>();
        public virtual ICollection<SysPermissionDelegation> Delegations { get; set; } = new HashSet<SysPermissionDelegation>();
    }
}
