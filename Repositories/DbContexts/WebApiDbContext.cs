using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data;
using Repositories.Data;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using Domain.AggregateRoots;
using Domain.Model;

namespace Repositories.WebApiDB
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext()
        {
        }

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<T_Button> T_Buttons { get; set; } = null!;
        public virtual DbSet<T_Dictionary> T_Dictionarys { get; set; } = null!;
        public virtual DbSet<T_Permission> T_Permissions { get; set; } = null!;
        public virtual DbSet<T_EmailQueue> T_EmailQueues { get; set; } = null!;
        public virtual DbSet<T_Log> T_Logs { get; set; } = null!;
        public virtual DbSet<T_Route> T_Routes { get; set; } = null!;
        public virtual DbSet<T_Org> T_Orgs { get; set; } = null!;
        public virtual DbSet<T_Resource> T_Resources { get; set; } = null!;
        public virtual DbSet<T_Role> T_Roles { get; set; } = null!;
        public virtual DbSet<T_User> T_Users { get; set; } = null!;
        //public virtual DbSet<T_UserOrg> T_UserOrgs { get; set; } = null!;
        //public virtual DbSet<T_UserRole> T_UserRoles { get; set; } = null!;
        public virtual DbSet<T_UserRoleOrg> T_UserRoleOrg { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
