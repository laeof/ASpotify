using ASpotifyAuth.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASpotifyAuth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet("GetUsername")]
        public async Task<IActionResult> GetUsername()
        {
            return Ok(await _userService.GetUsername());
        }
        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userService.GetUser());
        }
    }
}
