namespace EnergyManagamentUser.Dtos
{
    public record UserLoginResponseDto(string Token, string Username, int UserId, string Role);
}
