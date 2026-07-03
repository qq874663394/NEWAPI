namespace Domain.Entities
{
    public partial class SysOrg
    {
        public virtual SysOrg Parent { get; set; }
        public virtual ICollection<SysOrg> Children { get; set; } = new HashSet<SysOrg>();
        public virtual ICollection<SysUserroleorg> UserRoleOrgs { get; set; } = new HashSet<SysUserroleorg>();
    }
}
