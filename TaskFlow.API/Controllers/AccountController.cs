using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlowDomain.DTOs.IdentityDTOs;
using TaskFlowDomain.Service.Contract;

namespace TaskFlow.API.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _authService.Register(registerDto);
            return Ok(ApiResponse<UserDto>.Ok(user, "Registered"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> LogIn([FromBody] LogInDto logInDto)
        {
            var user = await _authService.LogIn(logInDto);
            return Ok(ApiResponse<UserDto>.Ok(user, "Logged in"));
        }
    }
}
