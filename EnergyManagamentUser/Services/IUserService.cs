using EnergyManagamentUser.Dtos;

namespace EnergyManagamentUser.Services
{
    public interface IUserService
    {

        public Task<User> AddUser(UserDtoRegister userDto);

        public Task<User> AddManager(UserDtoRegister userDto);

        public Task<List<GetAllUsersResponseDto>> GetAllUsers();


        public Task DeleteUser(int userId);

        public string CreateToken(User user);

        public string CreatePasswordHash(string password);

        public Task UpdateUser(int userId, UserDtoUpdate userDto);
    }
}
