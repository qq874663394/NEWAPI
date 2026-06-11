namespace Domain.Entities
{
    public partial class SysUserRoleOrg
    {
        public virtual SysUser User { get; set; }
        public virtual SysRole Role { get; set; }
        public virtual SysOrg Org { get; set; }
    }
}
