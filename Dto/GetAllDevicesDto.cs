namespace EnergyManagementSystem.Dto
{
    public class GetAllDevicesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string EnergyConsumption { get; set; }

        public string OwnerUsername { get; set; }
    }
}
