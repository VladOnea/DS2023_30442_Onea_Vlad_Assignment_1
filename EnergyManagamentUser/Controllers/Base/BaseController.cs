using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnergyManagamentUser.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int LoggedInUserId
        {
            get => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
