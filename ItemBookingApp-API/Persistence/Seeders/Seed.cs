using ItemBookingApp_API.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ItemBookingApp_API.Persistence.Seeders
{
    public class Seed
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<AppUser> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var path = "Persistence/Seeders/Data/UserSeedData.json";
                var userData = System.IO.File.ReadAllText(path);

                var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "User"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Owner"},
                    new Role{Name = "SuperAdmin"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }



                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Owner").Wait();
                }

                var superAdmin = new AppUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin",
                    IsActive = true,
                    Email = "admin@example.com"
                };

                IdentityResult result = _userManager.CreateAsync(superAdmin, "Pa$$w0rd").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("SuperAdmin").Result;

                    _userManager.AddToRoleAsync(admin, "SuperAdmin").Wait();
                }
            }


        }
    }
}
