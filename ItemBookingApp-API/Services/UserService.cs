using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Resources.Query;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Formats.Asn1;

namespace ItemBookingApp_API.Services
{
    public class UserService : IUserService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IOrganisationService _organisationService;

        public UserService(IUnitOfWork unitOfWork, RoleManager<Role> roleManager,
                           SignInManager<AppUser> signInManager,
                           ApplicationUserManager applicationUserManager,
                           IOrganisationService organisationService)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _applicationUserManager = applicationUserManager;
            _organisationService = organisationService;
        }
        public async Task<UserResponse> ChangePassword(long userId, string oldPassword, string newPassword, bool isAdmin)
        {
            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);

            if (userFromRepo == null)
                return new UserResponse("User not found");

            if (isAdmin && !string.IsNullOrWhiteSpace(newPassword) && string.IsNullOrWhiteSpace(oldPassword))
            {
                userFromRepo.PasswordHash = _applicationUserManager.PasswordHasher.HashPassword(userFromRepo, newPassword);

                var passwordResult = await _applicationUserManager.UpdateAsync(userFromRepo);

                if (passwordResult.Succeeded)
                    return new UserResponse(userFromRepo);

                return new UserResponse("Unable to change password");
            }

            var result = await _applicationUserManager.ChangePasswordAsync(userFromRepo, oldPassword, newPassword);

            if (result.Succeeded)
                return new UserResponse(userFromRepo);

            return new UserResponse("Failed to change password due to password mismatch");
        }
        
        public async Task<UserAddress> GetUserAddress(long userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userAddress = new UserAddress(user.FirstName, user.LastName, user.Street, user.City, user.State, user.ZipCode);

                return userAddress;
            }

            return null;
        }

        public async Task<UserAddress> UpdateUserAddress(long userId, UserAddress userAddressToUpdate)
        {
            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);

            if (userFromRepo != null && userAddressToUpdate != null)
            {
                userFromRepo.ZipCode = userAddressToUpdate.ZipCode;
                userFromRepo.Street = userAddressToUpdate.Street;
                userFromRepo.City = userAddressToUpdate.State;

                await _applicationUserManager.UpdateAsync(userFromRepo);

                return userAddressToUpdate;
            }

            return null;
        }

        public Task<UserResponse> DeleteByIdAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> ExportUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> GetUserByIdAsync(long id)
        {
            var user = await _applicationUserManager.FindByIdAsync(id);

            if (user == null)
                return new UserResponse("User not found");

            return new UserResponse(user);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var user = await _applicationUserManager.FindByEmailAsync(email);

            return user ?? null;
        }

        public Task<UserResponse> ImportUsersAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<AppUser>> ListAsync(UserQuery userQuery)
        {            
          //  var user = await _applicationUserManager.FindByIdAsync(userQuery.UserId);
                
            return await _applicationUserManager.ListAsync(userQuery, userQuery.AccountType);
        }

        public async Task<PagedList<AppUser>> ListAsyncV2(UserQuery userQuery)
        {
            //  var user = await _applicationUserManager.FindByIdAsync(userQuery.UserId);

            return await _applicationUserManager.ListAsyncV2(userQuery);
        }

        public async Task<UserResponse> ActivateOrDisableUser(long userId, bool userDeactivationStatus)
        {
            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);

            if (userFromRepo == null)
                return new UserResponse("User not found");

            if (!userDeactivationStatus)
            {
                if (userFromRepo.UserName.ToLower() == "admin")
                {
                    return new UserResponse("Invalid Operation", new List<IdentityError>()
                    {
                        new IdentityError { Code = "Admin", Description = "Unable to deactivate default account"}
                    });
                }
                if (await _applicationUserManager.IsInRoleAsync(userFromRepo, RoleName.Owner))
                {
                    return new UserResponse("Invalid Operation", new List<IdentityError>()
                    {
                        new IdentityError { Code = "Owner", Description = "Unable to deactivate default owner account"}
                    });
                }
                if (userFromRepo.AccountType == AccountType.Individual)
                {
                    return new UserResponse("Invalid Operation", new List<IdentityError>()
                    {
                        new IdentityError { Code = "Individual", Description = "Unable to deactivate default individual account"}
                    });
                }

                userFromRepo.Status = EntityStatus.Disabled;
                await _unitOfWork.CompleteAsync();

                return new UserResponse(userFromRepo);
            }

            userFromRepo.Status = EntityStatus.Active;


            if (!await _applicationUserManager.IsInRoleAsync(userFromRepo, RoleName.User))
            {
                await _applicationUserManager.AddToRoleAsync(userFromRepo, RoleName.User);
            }

            await _unitOfWork.CompleteAsync();

            return new UserResponse(userFromRepo);
        }

        public async Task<UserResponse> SaveAsync(AppUser user, bool isExternalReg, List<string> userRoles, string password)
        {
            try
            {              
                if (isExternalReg)
                {
                    switch (user.AccountType)
                    {
                        case AccountType.Individual:
                            return await CreateIndividualAccount(user, password, userRoles);
                        case AccountType.Organisation:
                            return await CreateOrganisationAccount(user, password, userRoles);
                        case AccountType.Master:
                            return await CreateMasterAccount(user, password, userRoles);
                        default:
                            return new UserResponse("Failed to create user");
                    }
                }

                return await CreateOrganisationAccount(user, password, userRoles);
            }
            catch (Exception ex)
            {

                return new UserResponse($"An error occured while saving the user: {ex.Message}");
            }         
        }

        public async Task<UserResponse> UpdateAsync(long userId, AppUser user, List<string> roles)
        {
            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);
            var currentUserRoles = await _applicationUserManager.GetRolesAsync(userFromRepo);

            if (userFromRepo == null)
                return new UserResponse("User not found");

            userFromRepo.FirstName = user.FirstName;
            userFromRepo.LastName = user.LastName;
            userFromRepo.PhoneNumber = user.PhoneNumber;
            
            try
            {
                var result = await _applicationUserManager.UpdateAsync(userFromRepo);

                if (result.Succeeded)
                {
                    if (roles.Count() > 0)
                    {
                        var rolesResult = await _applicationUserManager.AddToRolesAsync(userFromRepo, roles.Except(currentUserRoles));

                        if (!rolesResult.Succeeded)
                            return new UserResponse("Failed to add to roles", rolesResult.Errors);

                        rolesResult = await _applicationUserManager.RemoveFromRolesAsync(userFromRepo, currentUserRoles.Except(roles));

                        if (!rolesResult.Succeeded)
                            return new UserResponse("Failed to remove the roles");

                        return new UserResponse(userFromRepo);
                    }

                    return new UserResponse(userFromRepo);
                }

                return new UserResponse("Failed to update user", result.Errors);
            }
            catch(Exception ex)
            {
                return new UserResponse($"An error occured when updating user: {ex.Message}");
            }
        }

        private async Task<UserResponse> CreateMasterAccount(AppUser user, string password, List<string> userRoles)
        {

            if (user != null && user.AccountType == AccountType.Master)
            {
                userRoles.Clear();
                userRoles.Add(RoleName.SuperAdmin);

                user.Status = EntityStatus.Active;

                return await CreateUser(user, password, userRoles);
            }

            return new UserResponse("Failed to create user");
        }
               

        private async Task<UserResponse> CreateOrganisationAccount(AppUser user, string password, List<string> userRoles)
        {
            var createdUser = new AppUser();

            if (user.AccountType == AccountType.Organisation && user.Organisation != null)
            {
                var orgResponse = await _organisationService.SaveAsync(user.Organisation);

                if (orgResponse.Success)
                {
                    user.IsPrimaryOrganisationContact = true;

                    user.Status = EntityStatus.Pending;
                    userRoles.Clear();
                    userRoles.Add(RoleName.Owner);

                    return await CreateUser(user, password, userRoles);
                }

                return new UserResponse("Failed to create user");
            }


            return await CreateUser(user, password, userRoles);
        }

        private async Task<UserResponse> CreateIndividualAccount(AppUser user, string password, List<string> userRoles)
        {
            if (user != null && user.AccountType == AccountType.Individual)
            {
                user.Status = EntityStatus.Pending;
                userRoles.Clear();
                userRoles.Add(RoleName.Owner);

                return await CreateUser(user, password, userRoles);
            }

            return new UserResponse("Failed to create user");
        }

        private async Task<UserResponse> CreateUser(AppUser user, string password, List<string> userRoles)
        {
            if (user == null) {
                var errors = new List<IdentityError>();
                errors.Add(new IdentityError { Code = "ERROR", Description = "Object is null"});
                return new UserResponse("User object is null", errors);
            }
            var result = await _applicationUserManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                if (userRoles.Count() > 0)
                {
                    var savedUser = await _applicationUserManager.FindByNameAsync(user.UserName);

                    await _applicationUserManager.AddToRolesAsync(savedUser, userRoles);
                }

                return new UserResponse(user);
            }

            return new UserResponse($"Failed to create User", result.Errors);
        }
    }
}
