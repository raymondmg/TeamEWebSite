//-----------------------------------------------【Function Indroduction】----------------------------------------------
//	  DBHelper：  database connected class
//    Language：  C#
//    IDE：VS2013
//    2015.10.16  Created by RaymondMG  
//---------------------------------------------------------------------------------------------------------------------

using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace TeamEMVCProject.Models
{
    public class DbHelper : DbContext
    {
        public DbHelper()
            : base("strConn")
        {
            //自动创建表，如果Entity有改到就更新到表结构
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbHelper, ReportingDbMigrationsConfiguration>());
        }
        public DbSet<UserModel> LoginModule { get; set; }
        public DbSet<RegisterModel> RegisterModule { get; set; }
        public DbSet<LastProductedModel> LastProductedModule { get; set; }
        public DbSet<ArticleShare> ArticleShareModule { get; set; }
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