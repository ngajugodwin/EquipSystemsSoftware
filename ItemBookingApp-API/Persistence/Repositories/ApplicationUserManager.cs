using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.Query;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

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
        private IQueryable<AppUser> FilterByStatus(IQueryable<AppUser> users, EntityStatus status, int orgId)
        {
            if (users.Count() > 0)
            {
                if (orgId > 0)
                {
                    switch (status)
                    {
                        case EntityStatus.Pending:
                            return users.Include(x => x.Organisation)
                                .Where(x => x.Status == status 
                                || x.OrganisationId == orgId).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        case EntityStatus.Active:
                            return users.Include(x => x.Organisation)
                                .Where(x => x.Status == status 
                                || x.OrganisationId == orgId).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        case EntityStatus.Disabled:
                            return users.Include(x => x.Organisation)
                                .Where(x => x.Status == status 
                                || x.OrganisationId == orgId).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        default:
                            return users.Include(x => x.Organisation)
                                .Where(x => x.Status == status 
                                || x.OrganisationId == orgId).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                    }

                } else
                {
                    switch (status)
                    {
                        case EntityStatus.Pending:
                            return users.Include(x=> x.Organisation).Where(x => x.Status == status).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        case EntityStatus.Active:
                            return users.Include(x => x.Organisation).Where(x => x.Status == status).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        case EntityStatus.Disabled:
                            return users.Include(x => x.Organisation).Where(x => x.Status == status).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        default:
                            return users.Include(x => x.Organisation).Where(x => x.Status == status).OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                    }
                }

                  
            }

            return users;
        }

        public async Task<PagedList<AppUser>> ListAsync(UserQuery userQuery, AccountType accountType)
        {
            var users = Enumerable.Empty<AppUser>().AsQueryable();                      

            if (accountType == AccountType.Individual)
            {
                if (!string.IsNullOrWhiteSpace(userQuery.AccountTypeName))
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Where(u => u.FirstName.Contains(userQuery.AccountTypeName) || u.LastName.Contains(userQuery.AccountTypeName))
                        .OrderByDescending(u => u.CreatedAt).AsQueryable().AsNoTracking();
                }
                

                users = base.Users.Include(ur => ur.UserRoles)
                    .ThenInclude(r => r.Role)
                    .OrderByDescending(u => u.CreatedAt).AsQueryable().AsNoTracking();               

                return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
            } 
            else if (accountType == AccountType.Organisation)
            {
                if (userQuery.Status > 0)
                {

                    switch (userQuery.Status)
                    {
                        case EntityStatus.Pending:
                            users = FilterByStatus(base.Users, userQuery.Status, userQuery.OrganisationId);
                            break;
                        case EntityStatus.Active:
                            users = FilterByStatus(base.Users, userQuery.Status, userQuery.OrganisationId);
                            break;
                        case EntityStatus.Disabled:
                            users = FilterByStatus(base.Users, userQuery.Status, userQuery.OrganisationId);
                            break;
                        default:
                            users = FilterByStatus(base.Users, userQuery.Status, userQuery.OrganisationId);
                            break;
                    }

                    //users = base.Users.Include(ur => ur.UserRoles)
                    //    .ThenInclude(r => r.Role)
                    //    .Include(o => o.Organisation)
                    //    .Where(u => u.Status == EntityStatus.Disabled && u.Organisation.Name.Contains(userQuery.AccountTypeName))
                    //    .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }
                else
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Include(o => o.Organisation)
                        .Where(u => u.Organisation.Name.Contains(userQuery.AccountTypeName) && u.OrganisationId == userQuery.OrganisationId || u.AccountType == userQuery.AccountType)
                        .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
                {
                    users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                                || u.LastName.Contains(userQuery.SearchString) 
                                || u.Organisation.Name.Contains(userQuery.AccountTypeName) && u.OrganisationId == userQuery.OrganisationId);
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
                if (userQuery.Status > 0)
                {
                    users = FilterByStatus(base.Users, userQuery.Status, userQuery.OrganisationId);
                }
                else
                {
                    users = base.Users.Include(ur => ur.UserRoles)
                        .ThenInclude(r => r.Role)
                        .Include(u => u.Organisation)
                        .Where(u => u.OrganisationId == userQuery.OrganisationId)
                        .OrderByDescending(u => u.CreatedAt).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
                {
                    users = base.Users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                                || u.LastName.Contains(userQuery.SearchString));
                }

                return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
            }
            else
            {
                if (userQuery.OrganisationId <= 0 && isSuperAdmin)
                {
                    if (userQuery.Status > 0)
                    {
                        users = FilterByStatus(base.Users, userQuery.Status, userQuery.OrganisationId);
                    }

                    users = FilterByStatus(base.Users, userQuery.Status, 0);
                }

                if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
                {
                    users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                                || u.LastName.Contains(userQuery.SearchString));
                }

            }

           return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
        }

        private IQueryable<AppUser> ApplyCustomFilter(IQueryable<AppUser> users, UserQuery query)
        {
         //   var usersToReturn = Enumerable.Empty<AppUser>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.AccountTypeName) || !string.IsNullOrWhiteSpace(query.SearchString))
            {
                if (query.AccountType == AccountType.Master || query.AccountType == AccountType.Individual)
                {
                    users = users.Include(x => x.Organisation)
                        .Where(u => u.Status == query.Status
                        && u.AccountType == query.AccountType
                        || u.FirstName.Contains(query.AccountTypeName)
                        || u.LastName.Contains(query.AccountTypeName)).AsQueryable().AsNoTracking();
                } else
                {
                    users = users.Include(x => x.Organisation)
                       .Where(u => u.Status == query.Status
                       && u.AccountType == query.AccountType
                       && u.Organisation.Name.Contains(query.AccountTypeName)).AsQueryable().AsNoTracking();
                }
              

                //users = users.Include(x => x.Organisation)
                //    .Where(u => u.Status == query.Status 
                //    && u.AccountType == query.AccountType
                //    || u.FirstName.Contains(query.AccountTypeName)
                //    || u.LastName.Contains(query.AccountTypeName)
                //    || u.Organisation.Name.Contains(query.AccountTypeName)).AsQueryable().AsNoTracking();
            }
            else
            {
                if (query.AccountType == AccountType.Master || query.AccountType == AccountType.Individual)
                {
                    users = users.Include(x => x.Organisation)
                        .Where(u => u.Status == query.Status
                        && u.AccountType == query.AccountType
                        || u.FirstName.Contains(query.AccountTypeName)
                        || u.LastName.Contains(query.AccountTypeName)).AsQueryable().AsNoTracking();
                }
                else
                {
                    users = users.Include(x => x.Organisation)
                       .Where(u => u.Status == query.Status
                       && u.AccountType == query.AccountType
                       && u.Organisation.Name.Contains(query.AccountTypeName)).AsQueryable().AsNoTracking();
                }


                //users = users.Include(x => x.Organisation)
                //    .Where(u => u.AccountType == query.AccountType
                //    && u.Status == query.Status).AsQueryable().AsNoTracking();
            }

            return users;
        }

        public async Task<PagedList<AppUser>> ListAsyncV2(UserQuery userQuery)
        {
            var users = base.Users;

            if (userQuery.Status > 0 && userQuery.AccountType > 0)
            {
                switch (userQuery.Status)
                {
                    case EntityStatus.Active:
                        users = ApplyCustomFilter(users, userQuery);
                        break;
                    case EntityStatus.Disabled:
                        users = ApplyCustomFilter(users, userQuery);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                users = users.Include(o => o.Organisation).Where(x => x.AccountType == userQuery.AccountType && x.Status == userQuery.Status);
            }

            if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
            {
                users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                            || u.LastName.Contains(userQuery.SearchString)
                            && u.Status == userQuery.Status
                            && u.AccountType == userQuery.AccountType);
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

        public async Task<PagedList<AppUser>> GetOrganisationUsersListAsync(int organisationId, UserQuery userQuery)
        {
            var users = Enumerable.Empty<AppUser>().AsQueryable();

            if (userQuery.Status > 0)
            {
                switch (userQuery.Status)
                {
                    case EntityStatus.Pending:
                        users = base.Users.Include(x => x.Organisation)
                                     .Where(x => x.Status == userQuery.Status
                                     && x.OrganisationId == organisationId)
                                     .OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        break;
                    case EntityStatus.Active:
                        users = base.Users.Include(x => x.Organisation)
                                     .Where(x => x.Status == userQuery.Status
                                     && x.OrganisationId == organisationId)
                                     .OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        break;
                    case EntityStatus.Disabled:
                        users = base.Users.Include(x => x.Organisation)
                                     .Where(x => x.Status == userQuery.Status
                                     && x.OrganisationId == organisationId)
                                     .OrderBy(c => c.CreatedAt).AsQueryable().AsNoTracking();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                users = base.Users
                    .Include(ur => ur.UserRoles)
                    .ThenInclude(r => r.Role)
                    .Where(x => x.OrganisationId == organisationId)
                    .OrderByDescending(u => u.CreatedAt).AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(userQuery.SearchString))
            {
                users = users.Where(u => u.FirstName.Contains(userQuery.SearchString)
                            || u.LastName.Contains(userQuery.SearchString)
                            && u.OrganisationId == organisationId
                            && u.Status == userQuery.Status);
            }

            return await PagedList<AppUser>.CreateAsync(users, userQuery.PageNumber, userQuery.PageSize);
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
