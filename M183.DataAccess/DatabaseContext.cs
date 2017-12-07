namespace M183.DataAccess
{
    using M183.DataAccess.Models;
    using M183.DataAccess.Models.Configurations;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DatabaseContext")
        {
            //Static reference is ensured.
            Type type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        #region Configuration
        public virtual DbSet<GlobalConfiguration> GlobalConfigurations { get; set; }
        public virtual DbSet<UserConfiguration> UserConfigurations { get; set; }
        #endregion

        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}