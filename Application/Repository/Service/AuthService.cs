using Application.Dto.UserDto;
using Application.Repository.Interface;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Repository.Service
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Student> _studentrepo;

        private readonly IHttpContextAccessor _httpacessor;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
        private readonly IConfiguration _config;
        public AuthService(IGenericRepository<User> userRepo, IHttpContextAccessor httpacessor, IConfiguration config, IGenericRepository<RefreshToken> refreshTokenRepo, IGenericRepository<Role> roleRepo, IGenericRepository<Student> studentrepo) {
            _userRepo = userRepo;
            _httpacessor = httpacessor;
            _config = config;
            _refreshTokenRepo = refreshTokenRepo;
            _roleRepo = roleRepo;
            _studentrepo = studentrepo;


        }

        public string GenerateAccessToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.role.Name)


            };

            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            var userIdClaim = _httpacessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(userIdClaim);

            var storedToken = _refreshTokenRepo.GetAll()
                .FirstOrDefault(rt => rt.UserId == userId && rt.Token == refreshToken && rt.Expires > DateTime.UtcNow);
            if (storedToken == null)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }
            var user = await _userRepo.GetById(storedToken.UserId);
            return GenerateAccessToken(user);
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto input)
        {
            var user = await _userRepo.GetAll().Include(x=>x.role)
                
                .FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == input.Email.Trim().ToLower());

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();
            var passowrdResult = passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);

            if (passowrdResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await _refreshTokenRepo.Insert(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new LoginResponseDto
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                UserName = user.FullName,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = user.role.Name.ToString()
            };
        }

        public async Task ResetPassword(ResetPasswordDto input)
        {
            var claimid = _httpacessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(claimid);
            var user = await _userRepo.GetById(userId);

            var hashpassword = new PasswordHasher<User>().VerifyHashedPassword(user, input.OldPassword, input.NewPassword);
            if(hashpassword == PasswordVerificationResult.Failed)
            {
                throw new Exception("Old password is wring ");
            }
            user.Password = input.NewPassword;

        }

        public async Task UpdateUserDetails(UpdateUserDetails input)
        {
            var claimid = _httpacessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(claimid);
            var user = await _userRepo.GetById(userId);
            user.FullName = input.FullName;
            user.Email = input.Email;
            user.PhoneNumber = input.Phone;
             _userRepo.Update(user);
            _userRepo.SaveChanges();

            

        }

       
        public async Task ChangePassword(ChangePasswordDto input) {
        var user =await _userRepo.GetById(input.UserId);
            user.Password = input.NewPassword;
           
             _userRepo.Update(user);
           await _userRepo.SaveChanges();
        
        }
    }
}
