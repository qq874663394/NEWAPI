namespace Domain.Entities
{
    public partial class SysRoute
    {
        public virtual SysRoute Parent { get; set; }
        public virtual ICollection<SysRoute> Children { get; set; } = new HashSet<SysRoute>();
        public virtual ICollection<SysButton> Buttons { get; set; } = new HashSet<SysButton>();
        public virtual ICollection<SysMenuPermission> MenuPermissions { get; set; } = new HashSet<SysMenuPermission>();
    }
}
