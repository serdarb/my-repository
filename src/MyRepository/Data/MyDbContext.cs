using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;

using MyRepository.Entity;

namespace MyRepository.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<User> User { get; set; }
    }

    public sealed class MyDbContextMigrationConfiguration : DbMigrationsConfiguration<MyDbContext>
    {
        public MyDbContextMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(MyDbContext context)
        {
        }
    }
}