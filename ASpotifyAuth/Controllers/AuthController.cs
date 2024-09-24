using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Dto;
using ASpotifyAuth.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASpotifyAuth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IJWTService _jwtService;
        private readonly IPasswordHasherService _passwordHasher;
        public AuthController(IUserService userService,
            IPasswordHasherService passwordHasherService,
            IAccountService accountService,
            IJWTService jwtService)
        {
            _userService = userService;
            _passwordHasher = passwordHasherService;
            _accountService = accountService;
            _jwtService = jwtService;
        }
        [HttpGet("email/{email}")]
        public async Task<IActionResult> ValidEmail(string email)
        {
            if (email == string.Empty)
                return BadRequest("String is empty");

            return Ok(await _accountService.CheckExistsEmail(email));
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> ValidUsername(string username)
        {
            if (username == string.Empty)
                return BadRequest("String is empty");

            return Ok(await _accountService.CheckValidUsername(username));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var validEmail = _accountService.CheckValidEmail(request.Email);

            if (!validEmail)
                return BadRequest("EmailValidationError:EmailNotValid");

            validEmail = await _accountService.CheckExistsEmail(request.Email);

            if (validEmail)
                return BadRequest("EmailValidationError:AlreadyExists");

            var validUsername = await _accountService.CheckValidUsername(request.Username);

            if (validUsername)
                return BadRequest("UsernameValidationError:AlreadyExists");

            var newuser = await _accountService.Register(request);

            return Ok(newuser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var validEmail = _accountService.CheckValidEmail(request.Email);

            if (!validEmail)
                return BadRequest("EmailValidationError:EmailNotValid");

            validEmail = await _accountService.CheckExistsEmail(request.Email);

            if (!validEmail)
                return BadRequest("EmailValidationError:EmailDoesNotExists");

            var validPassword = await _accountService.CheckValidPassword(request);

            if (!validPassword)
                return BadRequest("PasswordValidationError:PasswordIsNotCorrect");

            var user = await _accountService.GetUserAsync(request);

            var token = _jwtService.GenerateToken(user.Id);

            if (token == null)
                return Unauthorized("TokenGenerateError");

            var tokens = new UserRefreshToken {
                RefreshToken = token.RefreshToken,
                UserId = user.Id,
            };

            await _accountService.AddUserRefreshTokens(tokens);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(Tokens token)
        {
            if (token.AccessToken == "" || token.RefreshToken == "")
                return BadRequest("token cannot be null");

            var principal = _jwtService.GetPrincipalFromExpiredToken(token.AccessToken);
            var id = principal.Identity?.Name;

            if(id == null)
                return Unauthorized("Invalid attempt!");

            var guid = Guid.Parse(id);

            var savedRefreshToken = await _accountService.GetSavedRefreshTokens(guid, token.RefreshToken);

            if (savedRefreshToken.RefreshToken != token.RefreshToken)
                return Unauthorized("Invalid attempt!");

            var newJwtToken = _jwtService.GenerateRefreshToken(guid);

            if (newJwtToken == null)
                return Unauthorized("Invalid attempt!");

            UserRefreshToken obj = new UserRefreshToken
            {
                UserId = Guid.Parse(id),
                RefreshToken = newJwtToken.RefreshToken,
            };

            await _accountService.DeleteUserRefreshTokens(savedRefreshToken.Id, token.RefreshToken);
            await _accountService.AddUserRefreshTokens(obj);

            return Ok(newJwtToken);
        }

        [HttpPost("access-expire")]
        public IActionResult Access(string token)
        {
            return Ok(_jwtService.ExpireAccessToken(token));
        }
    }
}
