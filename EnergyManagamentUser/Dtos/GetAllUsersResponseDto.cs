namespace EnergyManagamentUser.Dtos
{
    public class GetAllUsersResponseDto
    {
        public int Id { get; set; } 
        public string Username { get; set; }
        public required string PhoneNumber { get; set; }
    
        public string Email { get; set; }

        public required string Role { get; set; }
    }
}
