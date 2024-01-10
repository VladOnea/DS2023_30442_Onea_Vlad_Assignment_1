using EnergyManagamentUser.Controllers.Base;
using EnergyManagamentUser.Data;
using EnergyManagamentUser.Dtos;
using EnergyManagamentUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EnergyManagamentUser.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private const string DEVICE_USER_URL = "http://localhost:5050/api/user";

    private readonly HttpClient _httpClient;

    private UserDataContext _dbContext;

    private readonly IUserService _userService;

    public AuthController(UserDataContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _httpClient = new HttpClient();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserLoginResponseDto>> GetLoggedInUser()
    {
        var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == LoggedInUserId);

        if (foundUser is null)
        {
            return BadRequest();
        }

        var newToken = _userService.CreateToken(foundUser);

        return new UserLoginResponseDto(newToken, foundUser.Username, foundUser.Id, foundUser.Role);
    }

    [HttpPost("register")]

    public async Task<ActionResult<User>> Register(UserDtoRegister request)
    {
        var addedUser = await _userService.AddUser(request);

        var userToTransfer= new UserDeviceDto { Id = addedUser.Id , Username = addedUser.Username};

        var response = await HttpClientService.PostAsync(_httpClient, DEVICE_USER_URL, userToTransfer);

        if(response != System.Net.HttpStatusCode.OK)
        {
            await _userService.DeleteUser(addedUser.Id);
            return BadRequest();
        }

        return addedUser;
    }

    [HttpPost("login")]

    public async Task<ActionResult<object>> Login(UserDtoLogin request)
    {
        var foundUser = await _dbContext.Users.Where(u => u.Username == request.Username)
             .FirstOrDefaultAsync();

        if(foundUser is null)
        {
            return BadRequest();
        }

        var hashedPassword = _userService.CreatePasswordHash(request.Password);
        if (hashedPassword != foundUser.PasswordHash)
        {
            return BadRequest("Wrong Password");
        }

        string token = _userService.CreateToken(foundUser);

        return new UserLoginResponseDto(token, foundUser.Username, foundUser.Id, foundUser.Role);
    }

    [HttpPost("registerAdmin")]

    public async Task<ActionResult<User>> RegisterAdmin(UserDtoRegister request)
    {
        var addedUser = await _userService.AddManager(request);

        return addedUser;
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        await HttpClientService.DeleteAsync(_httpClient, $"{DEVICE_USER_URL}/{userId}");

        await _userService.DeleteUser(userId);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update/{userId}")]
    public async Task<ActionResult> UpdateUser(int userId, UserDtoUpdate request)
    {
         await _userService.UpdateUser(userId,request);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("getAll")]
    public async Task<ActionResult<List<GetAllUsersResponseDto>>> GetAllUsers()
    {
        return await _userService.GetAllUsers();
    }


}
