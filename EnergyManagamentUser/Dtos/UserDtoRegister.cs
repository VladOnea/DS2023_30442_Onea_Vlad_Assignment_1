namespace EnergyManagamentUser.Dtos
{
    public class UserDtoRegister
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }
    }
}
