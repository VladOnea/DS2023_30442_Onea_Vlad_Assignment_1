using Microsoft.EntityFrameworkCore;

namespace EnergyManagementSystem.Data
{
    public class DeviceDataContext : DbContext
    {
        public DeviceDataContext(DbContextOptions<DeviceDataContext> options) : base(options)
        {
        }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<Device> Devices { get; set; }  
        public DbSet<User> Users { get; set; }

    }
    
}
