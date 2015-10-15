using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using TeamEMVCProject.Models;

namespace MVCEF.Models
{
    public class DbHelper : DbContext
    {
        public DbHelper()
            : base("strConn")
        {
            //自动创建表，如果Entity有改到就更新到表结构
            Database.SetInitializer<DbHelper>(new MigrateDatabaseToLatestVersion<DbHelper, ReportingDbMigrationsConfiguration>());
        }
        public DbSet<LoginModel> LoginModule { get; set; }
        public DbSet<RegisterModel> RegisterModule { get; set; }
        public DbSet<LastProductedModel> LastProductedModule { get; set; }
    }
    internal sealed class ReportingDbMigrationsConfiguration : DbMigrationsConfiguration<DbHelper>
    {
        public ReportingDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;//所有model修改直接更新DB
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}