using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.Query;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class ApplicationUserManager : UserManager<AppUser>
    {
        public ApplicationUserManager(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, 
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) 
                : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<PagedList<AppUser>> ListAsync(UserQuery userQuery, AccountType accountType)
        {
            var users = Enumerable.Empty<AppUser>().AsQueryable();                      

            if (accountType == AccountType.Individual)
            {
                users = base.Users.Include(ur => ur.UserRoles)
                 .ThenInclude(r => r.Role)
                 .Where(u => u.Id == userQuery.UserId)
                 .OrderByDescending(u => u.CreatedAt).AsQueryable();

                return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
            } 
            else if (accountType == AccountType.Organisation)
            {
                if (!string.IsNullOrWhiteSpace(userQuery.FilterBy) && userQuery.FilterBy.ToLower() == "inactive")
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Where(u => u.IsActive == false && u.OrganisationId == userQuery.OrganisationId)
                        .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }
                else
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Where(u => u.OrganisationId == userQuery.OrganisationId)
                        .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
                {
                    users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                                || u.LastName.Contains(userQuery.SearchString) && u.OrganisationId == userQuery.OrganisationId);
                }

                return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
            }
            else
            {
                return await GetAllUsersAsync(userQuery, true);
            }

           
        }

        private async Task<PagedList<AppUser>> GetAllUsersAsync(UserQuery userQuery, bool isSuperAdmin = false)
        {
            var users = Enumerable.Empty<AppUser>().AsQueryable();

            if (userQuery.OrganisationId > 0 && isSuperAdmin)
            {
                if (!string.IsNullOrWhiteSpace(userQuery.FilterBy) && userQuery.FilterBy.ToLower() == "inactive")
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Where(u => u.IsActive == false && u.OrganisationId == userQuery.OrganisationId)
                        .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }
                else
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
                {
                    users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                                || u.LastName.Contains(userQuery.SearchString));
                }

                return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
            }
            else
            {
                if (userQuery.OrganisationId <= 0 && isSuperAdmin)
                {
                    if (!string.IsNullOrWhiteSpace(userQuery.FilterBy) && userQuery.FilterBy.ToLower() == "inactive")
                    {
                        users = base.Users.Include(ur => ur.UserRoles)
                            .ThenInclude(r => r.Role)
                            .Where(u => u.IsActive == false)
                            .OrderByDescending(u => u.CreatedAt).AsQueryable();
                    }
                    else
                    {
                        users = base.Users.Include(ur => ur.UserRoles)
                            .ThenInclude(r => r.Role)
                             .Where(u => u.IsActive == true)
                            .OrderByDescending(u => u.CreatedAt).AsQueryable();
                    }
                }

                if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
                {
                    users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                                || u.LastName.Contains(userQuery.SearchString));
                }

            }

           return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
        }

        public async override Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            var isExistingEmail = await base.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());

            var isExistingUsername = await base.Users.AnyAsync(u => u.UserName.ToLower() == user.UserName.ToLower());


            if (isExistingEmail || isExistingUsername)
            {
                var errors = new List<IdentityError>();

                if (isExistingEmail)
                {
                    var emailError = new IdentityError() { Code = "Duplicate Email", Description = "Email is already taken" };

                    errors.Add(emailError);
                }

                if (isExistingUsername)
                {
                    var userNameError = new IdentityError() { Code = "Duplicate Username", Description = "Username is already taken" };

                    errors.Add(userNameError);
                }

                var result = IdentityResult.Failed(errors.ToArray());

                return result;
            }

            return await base.CreateAsync(user, password);
        }

        public override Task<AppUser> FindByEmailAsync(string email)
        {
            return base.FindByEmailAsync(email);
        }


        public async Task<AppUser> FindByIdAsync(long userId)
        {
            var query = base.Users.AsQueryable();

            var user =  await query.Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
                return user;

            return new AppUser();
        }

    }
}
