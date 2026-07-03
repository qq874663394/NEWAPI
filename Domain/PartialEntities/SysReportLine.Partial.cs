namespace Domain.Entities
{
    public partial class SysReportline
    {
        public virtual SysUser User { get; set; }
        public virtual SysUser Supervisor { get; set; }
        public virtual SysOrg Org { get; set; }
        public virtual SysRole Role { get; set; }
    }
}
