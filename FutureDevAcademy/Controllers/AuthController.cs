using Application.Dto.UserDto;
using Application.Repository.Interface;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FutureDevAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) { 
        
        _authService = authService;
        
        
        }

        [HttpPost("Login")]
        
        public async Task <IActionResult> Login([FromBody]LoginRequestDto input)
        {

            var response = await _authService.LoginAsync(input);
            if (response == null)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok(response);


        }
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var accessToken = await _authService.RefreshToken(refreshToken);
            return Ok(accessToken);
        }

    }
}
