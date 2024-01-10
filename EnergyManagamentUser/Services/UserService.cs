using EnergyManagamentUser.Data;
using EnergyManagamentUser.Dtos;
using EnergyManagamentUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EnergyManagamentUser
{
    public class UserService : IUserService
    {
        private IConfiguration _configuration;

        private readonly UserDataContext _dbContext;

        public UserService(UserDataContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User> AddUser(UserDtoRegister userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = CreatePasswordHash(userDto.Password),
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = "User"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> AddManager(UserDtoRegister userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = CreatePasswordHash(userDto.Password),
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = "Admin"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }


        public async Task<List<GetAllUsersResponseDto>> GetAllUsers()
        {
            var users = await _dbContext.Users
                .Select(u => new GetAllUsersResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    PhoneNumber = u.PhoneNumber
                })
                .ToListAsync();

            return users;
        }

        public async Task UpdateUser(int userId, UserDtoUpdate userDto)
        {
            var user = _dbContext.Users.Find(userId);

            if(user != null)
            {
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Role = userDto.Role;

                await _dbContext.SaveChangesAsync();
            }
          
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = "",
                Audience = ""
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwt);
        }
        public string CreatePasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }

        }


    }
}
