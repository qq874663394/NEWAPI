using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Persistence.Configurations
{
    public class SysUserConfiguration : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            builder.ToTable("sys_user");
            // 一对多：用户 -> 用户角色组织
            builder.HasMany(u => u.UserRoleOrgs)
                   .WithOne(uro => uro.User)
                   .HasForeignKey(uro => uro.UserCode);          // ✅ 直接使用外键属性
        }
    }

    public class SysOrgConfiguration : IEntityTypeConfiguration<SysOrg>
    {
        public void Configure(EntityTypeBuilder<SysOrg> builder)
        {
            builder.ToTable("SYS_Org");

            // 自引用
            builder.HasOne(o => o.Parent)
                   .WithMany(o => o.Children)
                   .HasForeignKey(o => o.ParentCode);

            // 一对多：组织 -> 用户角色组织
            builder.HasMany(o => o.UserRoleOrgs)
                   .WithOne(uro => uro.Org)
                   .HasForeignKey(uro => uro.OrgCode);   // 直接使用 OrgCode 属性
        }
    }

    public class SysRoleConfiguration : IEntityTypeConfiguration<SysRole>
    {
        public void Configure(EntityTypeBuilder<SysRole> builder)
        {
            builder.ToTable("SYS_Role");

            // 自引用
            builder.HasOne(r => r.SuperiorRole)
                   .WithMany(r => r.InferiorRoles)
                   .HasForeignKey(r => r.SuperiorRoleCode);

            // 一对多：角色 -> 用户角色组织
            builder.HasMany(r => r.UserRoleOrgs)
                   .WithOne(uro => uro.Role)
                   .HasForeignKey(uro => uro.RoleCode);   // 直接使用外键字段
        }
    }

    public class SysUserRoleOrgConfiguration : IEntityTypeConfiguration<SysUserroleorg>
    {
        public void Configure(EntityTypeBuilder<SysUserroleorg> builder)
        {
            builder.ToTable("SYS_UserRoleOrg");
            // 外键已在其他配置中指定，此处不需要额外配置
        }
    }

    public class SysRouteConfiguration : IEntityTypeConfiguration<SysRoute>
    {
        public void Configure(EntityTypeBuilder<SysRoute> builder)
        {
            builder.ToTable("SYS_Route");
            // 自引用父子菜单
            builder.HasOne(r => r.Parent)
                   .WithMany(r => r.Children)
                   .HasForeignKey(r => r.ParentCode);
            // 一对多：菜单 -> 按钮
            builder.HasMany(r => r.Buttons)
                   .WithOne(b => b.Route)
                   .HasForeignKey(b => b.RouteCode);
            // 一对多：菜单 -> 菜单权限
            builder.HasMany(r => r.MenuPermissions)
                   .WithOne(mp => mp.Route)
                   .HasForeignKey(mp => mp.RouteCode);           // ✅
        }
    }

    public class SysButtonConfiguration : IEntityTypeConfiguration<SysButton>
    {
        public void Configure(EntityTypeBuilder<SysButton> builder)
        {
            builder.ToTable("SYS_Button");


            builder.HasMany(b => b.ButtonPermissions)
                   .WithOne(bp => bp.Button)          // 使用你保留的导航属性
                   .HasForeignKey(bp => bp.ButtonCode); // 外键属性，不是 bp.Button.Code

            builder.HasMany(b => b.Delegations)
                   .WithOne(pd => pd.Button)
                   .HasForeignKey(pd => pd.ButtonCode); // Delegation 也必须有 ButtonCode 属性
        }
    }

    public class SysMenuPermissionConfiguration : IEntityTypeConfiguration<SysMenupermission>
    {
        public void Configure(EntityTypeBuilder<SysMenupermission> builder)
        {
            builder.ToTable("SYS_MenuPermission");
        }
    }

    public class SysButtonPermissionConfiguration : IEntityTypeConfiguration<SysButtonpermission>
    {
        public void Configure(EntityTypeBuilder<SysButtonpermission> builder)
        {
            builder.ToTable("SYS_ButtonPermission");
        }
    }

    public class SysPermissionDelegationConfiguration : IEntityTypeConfiguration<SysPermissiondelegation>
    {
        public void Configure(EntityTypeBuilder<SysPermissiondelegation> builder)
        {
            builder.ToTable("SYS_PermissionDelegation");

            builder.HasOne(pd => pd.FromUser)
                   .WithMany()
                   .HasForeignKey(pd => pd.FromUserCode);  // 直接使用外键属性

            builder.HasOne(pd => pd.ToUser)
                   .WithMany()
                   .HasForeignKey(pd => pd.ToUserCode);

            builder.HasOne(pd => pd.Button)
                   .WithMany(b => b.Delegations)
                   .HasForeignKey(pd => pd.ButtonCode);

            builder.HasOne(pd => pd.Route)
                   .WithMany()
                   .HasForeignKey(pd => pd.RouteCode);
        }
    }

    public class SysReportLineConfiguration : IEntityTypeConfiguration<SysReportline>
    {
        public void Configure(EntityTypeBuilder<SysReportline> builder)
        {
            builder.ToTable("SYS_ReportLine");

            builder.HasOne(rl => rl.User)
                   .WithMany()
                   .HasForeignKey(rl => rl.UserCode);      // 使用外键字段，不是 rl.User.Code

            builder.HasOne(rl => rl.Supervisor)
                   .WithMany()
                   .HasForeignKey(rl => rl.SupervisorUserCode);

            builder.HasOne(rl => rl.Org)
                   .WithMany()
                   .HasForeignKey(rl => rl.OrgCode);

            builder.HasOne(rl => rl.Role)
                   .WithMany()
                   .HasForeignKey(rl => rl.RoleCode);
        }
    }

    public class SysDictionaryConfiguration : IEntityTypeConfiguration<SysDictionary>
    {
        public void Configure(EntityTypeBuilder<SysDictionary> builder)
        {
            builder.ToTable("SYS_Dictionary");
            builder.HasOne(d => d.Parent)
                   .WithMany(d => d.Children)
                   .HasForeignKey(d => d.ParentCode);
        }
    }

    // 无外键的表，仅配置表名
    public class SysAttachmentConfiguration : IEntityTypeConfiguration<SysAttachment>
    {
        public void Configure(EntityTypeBuilder<SysAttachment> builder) => builder.ToTable("SYS_Attachment");
    }

    public class SysEmailQueueConfiguration : IEntityTypeConfiguration<SysEmailqueue>
    {
        public void Configure(EntityTypeBuilder<SysEmailqueue> builder) => builder.ToTable("SYS_EmailQueue");
    }

    public class SysLogConfiguration : IEntityTypeConfiguration<SysLog>
    {
        public void Configure(EntityTypeBuilder<SysLog> builder) => builder.ToTable("SYS_Log");
    }
}
