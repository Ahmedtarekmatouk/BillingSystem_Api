using Billing.Models;
using Billing.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Services
{
    public class Tokens
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly tokenRepository _tokenRepository;

        public Tokens(UserManager<ApplicationUser> userManager, IConfiguration configuration, tokenRepository tokenRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }

        // Generate JWT token
        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString("HH:mm:ss"))
            };

            // Add user roles to the token
            var roles = await _userManager.GetRolesAsync(user);
            userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:ExpiryInDays"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Check if a username exists using the repository
        public bool CheckUserNameExists(string userName)
        {
            var user = _tokenRepository.CheckByName(userName.ToUpper());
            return user != null;
        }

        // Get all users from the repository
        public List<ApplicationUser> GetAllUsers()
        {
            return _tokenRepository.GetAll();
        }

        // Delete a user by ID using the repository
        public bool DeleteUser(string id)
        {
            var user = _tokenRepository.GetById(id);
            if (user != null)
            {
                _tokenRepository.Delete(id);
                return true;
            }
            return false;
        }
    }
}
