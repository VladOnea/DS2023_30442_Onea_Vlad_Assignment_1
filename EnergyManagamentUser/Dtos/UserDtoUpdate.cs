namespace EnergyManagamentUser.Dtos
{
    public class UserDtoUpdate
    {
        public required string Username { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }

        public required string Role { get; set; }
    }
}
