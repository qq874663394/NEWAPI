namespace Domain.Entities
{
    public partial class SysUser
    {
        public virtual ICollection<SysUserroleorg> UserRoleOrgs { get; set; } = new HashSet<SysUserroleorg>();
    }
}
