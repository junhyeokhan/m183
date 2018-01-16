namespace M183.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DatabaseContext")
        {
            // Add static reference to Entity framwork
            Type type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            //TODO: Create database when database does not exist
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Token> Token { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
