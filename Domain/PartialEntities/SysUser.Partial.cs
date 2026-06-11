namespace Domain.Entities
{
    public partial class SysUser
    {
        public virtual ICollection<SysUserRoleOrg> UserRoleOrgs { get; set; } = new HashSet<SysUserRoleOrg>();
    }
}
