using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlowDomain.DTOs.IdentityDTOs;
using TaskFlowDomain.Entites;
using TaskFlowDomain.Service.Contract;

namespace TaskFlow.Application.Service.Identity
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<User> userManager
            , SignInManager<User> signInManager
            , IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            ArgumentNullException.ThrowIfNull(registerDto);

            if (string.IsNullOrWhiteSpace(registerDto.Email) || string.IsNullOrWhiteSpace(registerDto.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser is not null)
            {
                throw new ArgumentException("This email is already registered.");
            }

            var user = new User
            {
                FullName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException(errors);
            }

            var token = await GenerateTokenAsync(user);
            return new UserDto
            {
                UserId = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Token = token,
            };
        }



        public async Task<UserDto> LogIn(LogInDto logInDto)
        {
            ArgumentNullException.ThrowIfNull(logInDto);

            if (string.IsNullOrWhiteSpace(logInDto.Email) || string.IsNullOrWhiteSpace(logInDto.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            var user = await _userManager.FindByEmailAsync(logInDto.Email);
            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, logInDto.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = await GenerateTokenAsync(user);
            return new UserDto
            {
                UserId = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Token = token
            };
        }





        private async Task<string> GenerateTokenAsync(User user)
        {
            var UserRols = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();

            foreach (var role in UserRols)
            {
                RoleClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
            }
            .Union(RoleClaims);


            var SemmetiricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSetting:Key"]!));
            var SigningCredentials = new SigningCredentials(SemmetiricKey, SecurityAlgorithms.HmacSha256);

            var objectToken = new JwtSecurityToken(
                issuer: _configuration["JWTSetting:issuer"],
                audience: _configuration["JWTSetting:audience"],
                claims: Claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWTSetting:DurationInDays"])),
                signingCredentials: SigningCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(objectToken);

        }
    }
}
