using System.Threading.Tasks;
using asset_management.Entities;
using asset_management.Models;
using AssetManagement.Database;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace asset_management.Services
{
    public class AppServices
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _config;
        private RoleManager<IdentityRole> _roleManager;
        public AppServices(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

        private async Task<ServiceResponse> CreateAccessToken(ApplicationUser User)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                List<string> role = (List<string>)await _userManager.GetRolesAsync(User);

                var claims = new[] {
                    new Claim (JwtRegisteredClaimNames.Sub, User.Id),
                    new Claim (ClaimTypes.NameIdentifier, User.Id),
                    new Claim (ClaimTypes.Role, role.FirstOrDefault ()),
                    new Claim (ClaimTypes.AuthorizationDecision, role.FirstOrDefault ()),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                    new Claim (JwtRegisteredClaimNames.Iat, new DateTimeOffset (now).ToUnixTimeSeconds ().ToString ())
                };

                int tokenExpiration = _config.GetValue<int>("Auth:Jwt:TokenExpiration");
                var IsserSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _config["Auth:Jwt:Issuer"],
                    audience: _config["Auth:Jwt:Audience"],
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromHours(tokenExpiration)),
                    signingCredentials: new SigningCredentials(IsserSignKey, SecurityAlgorithms.HmacSha256)
                );

                var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

                var response = new AuthResponseModel()
                {
                    token = encodeToken,
                    Name = $"{User.firstName} {User.lastName}",
                };

                return new ServiceResponse { response = response, status = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { response = ex, status = false };
            }
        }

        public async Task<ServiceResponse> GenerateToken(AuthModel model)
        {
            try
            {
                ApplicationUser User = (from user in _dbContext.ApplicationUser where user.UserName.ToLower() == model.username.ToLower() select user).FirstOrDefault();

                if (User == null)
                {
                    return new ServiceResponse { status = false, response = "User record not found" };
                }

                var role = (await _userManager.GetRolesAsync(User)).FirstOrDefault();

                var status = await _userManager.CheckPasswordAsync(User, model.password);

                if (!status)
                {
                    return new ServiceResponse { status = false, response = "Incorrect password supplied" };
                }

                return await CreateAccessToken(User);

            }
            catch (Exception ex)
            {
                return new ServiceResponse { status = false, response = new UnauthorizedResult(), message = ex.Message };
            }
        }

        public async Task<ServiceResponse> Register(ApplicationUser User, string Password, string Role)
        {
            User.Email = "needlesupport@needletech.ng";

            var result = await _userManager.CreateAsync(User, Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role));
                }
                await _userManager.AddToRoleAsync(User, Role);
                return new ServiceResponse { status = true, response = User };
            }
            else
            {
                var error = result.Errors.Select(x => x.Description).ToList();
                return new ServiceResponse { status = false, response = error[0] };
            }
        }

    }
}