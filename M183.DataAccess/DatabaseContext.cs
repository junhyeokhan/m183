namespace M183.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DatabaseContext")
        {
            // Add static reference to Entity framwork
            Type type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            // Is database not existing yet?
            if (!Database.Exists())
            {
                // Try to create the database
                Database.Create();
                // Run seeding method
                var migrator = new DbMigrator(new Migrations.Configuration());
                migrator.Update();
            }
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserStatus> UserStatus { get; set; }

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostStatus> PostStatus { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
