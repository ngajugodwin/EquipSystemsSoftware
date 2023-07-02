using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ItemBookingApp_API.Persistence.Seeders
{
    public class Seed
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _contex;

        public Seed(UserManager<AppUser> userManager, RoleManager<Role> roleManager, ApplicationDbContext contex)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contex = contex;
        }

        public void SeedUsers()
        {
            if(!_contex.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("Persistence/Seeders/Data/DeliverySeedData.json");

                var deliveryMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(deliveryData);

                foreach (var item in deliveryMethods)
                {
                    _contex.DeliveryMethods.Add(item);
                }

                _contex.SaveChangesAsync();
            }

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
                    user.Status = Domain.Models.EntityStatus.Active;
                    user.AccountType = AccountType.Master;
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "SuperAdmin").Wait();
                }             

                var superAdmin = new AppUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin",
                    Street = "Priory St",
                    State = "Cov",
                    City = "Coventry",
                    ZipCode = "CV1 5FB",
                    AccountType = AccountType.Master,
                    Status = Domain.Models.EntityStatus.Active,
                    Email = "admin@example.com"
                };

                IdentityResult result = _userManager.CreateAsync(superAdmin, "Pa$$w0rd").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByEmailAsync("admin@example.com").Result;

                    _userManager.AddToRoleAsync(admin, "SuperAdmin").Wait();
                }
            }


        }
    }
}
