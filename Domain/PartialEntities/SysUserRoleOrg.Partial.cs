namespace Domain.Entities
{
    public partial class SysUserroleorg
    {
        public virtual SysUser User { get; set; }
        public virtual SysRole Role { get; set; }
        public virtual SysOrg Org { get; set; }
    }
}
