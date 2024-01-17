namespace EnergyManagementSystem.Models
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string Description { get; set; } 

        public string Address { get; set; } 

        public string EnergyConsumption { get; set; } 

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
