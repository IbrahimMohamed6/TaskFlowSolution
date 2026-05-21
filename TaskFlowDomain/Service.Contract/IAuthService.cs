using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.DTOs.IdentityDTOs;

namespace TaskFlowDomain.Service.Contract
{
    public interface IAuthService
    {
        public Task<UserDto> Register(RegisterDto registerDto);

        public Task<UserDto> LogIn(LogInDto logInDto);
    }
}
