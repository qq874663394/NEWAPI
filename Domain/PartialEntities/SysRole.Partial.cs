namespace Domain.Entities
{
    public partial class SysRole
    {
        public virtual SysRole SuperiorRole { get; set; }
        public virtual ICollection<SysRole> InferiorRoles { get; set; } = new HashSet<SysRole>();
        public virtual ICollection<SysUserRoleOrg> UserRoleOrgs { get; set; } = new HashSet<SysUserRoleOrg>();
    }
}
