using System;
using System.Threading.Tasks;
using asset_management.Entities;
using AssetManagement.Database;
using Coravel.Invocable;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace asset_management.Services
{
    public class SeedData
    {
        // UserManager<ApplicationUser> _userManager;
        // RoleManager<IdentityRole> _roleManager;
        // public SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        // {
        //     _userManager = userManager;
        //     _roleManager = roleManager;
        // }
        public static async Task Invoke(IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var _signInManager = scope.ServiceProvider.GetService<SignInManager<ApplicationUser>>();
                var _userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                ApplicationUser User = new ApplicationUser
                {
                    firstName = "Administrator",
                    lastName = "User",
                    UserName = "Admin"
                };

                string Role = "Admin";
                User.Email = "needlesupport@needletech.ng";

                var result = await _userManager.CreateAsync(User, "AdminPassword@123");

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Role));
                    }
                    await _userManager.AddToRoleAsync(User, Role);
                }
            }
        }
    }
}