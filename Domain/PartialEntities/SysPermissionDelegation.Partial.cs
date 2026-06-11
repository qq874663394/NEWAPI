namespace Domain.Entities
{
    public partial class SysPermissionDelegation
    {
        public virtual SysUser FromUser { get; set; }
        public virtual SysUser ToUser { get; set; }
        public virtual SysButton Button { get; set; }
        public virtual SysRoute Route { get; set; }
    }
}
