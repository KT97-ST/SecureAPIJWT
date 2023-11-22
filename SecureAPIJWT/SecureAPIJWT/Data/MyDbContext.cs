using Microsoft.EntityFrameworkCore;

namespace SecureAPIJWT.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<Users> Users { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(e => {
                e.HasIndex(e => e.UserName).IsUnique();
                e.Property(e => e.FullName).IsRequired().HasMaxLength(300);
                e.Property(e => e.Email).IsRequired().HasMaxLength(300);
            });
        }

    }
}
