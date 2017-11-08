namespace M183.DataAccess
{
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
        
        public virtual DbSet<LoggedText> LoggedText { get; set; }
        public virtual DbSet<LoggedAccount> LoggedAccount { get; set; }
    }
}