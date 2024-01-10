using Microsoft.EntityFrameworkCore;


namespace EnergyManagamentUser.Data
{
    public class UserDataContext : DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<User> Users { get; set; }

    }
}
