using EnergyManagementSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnergyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DeviceDataContext _dbContext;
        public UserController(DeviceDataContext deviceDataContext)
        {
            _dbContext = deviceDataContext;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{userId:int}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if(foundUser is null)
            {
                return NotFound();
            }

            _dbContext.Remove(foundUser);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{userId:int}")]
        public async Task<ActionResult> UpdateUser(int userId, User user)
        {
            var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (foundUser is null)
            {
                return NotFound(); 
            }
            foundUser.Username = user.Username;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
