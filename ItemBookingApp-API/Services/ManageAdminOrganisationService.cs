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
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ItemBookingApp_API.Services
{
    public class ManageAdminOrganisationService : IManageAdminOrganisationService
    {
        private readonly IUserService _userService;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IUnitOfWork _unitOfWork;

        public ManageAdminOrganisationService (IUserService userService, ApplicationUserManager applicationUserManager,
            IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _applicationUserManager = applicationUserManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> ActivateOrDisableUser(int organisationId, long userId, bool userStatus)
        {
            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);


            if (userFromRepo.OrganisationId != organisationId)
                return new UserResponse("You can only update user status in your Tenant");

            if (userFromRepo == null)
                return new UserResponse("User not found");

            if (!userStatus)
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
            await _unitOfWork.CompleteAsync();

            return new UserResponse(userFromRepo);
        }

        public async Task<UserResponse> ChangePassword(int organisationId, long userId, string oldPassword, string newPassword, bool isAdmin)
        {
            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);

            if (userFromRepo == null || userFromRepo.OrganisationId != organisationId)
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

        public Task<UserResponse> DeleteByIdAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> ExportUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> GetUserByIdAsync(long id, int organisationId)
        {
            var user = await _applicationUserManager.FindByIdAsync(id);

            if (user == null || user.OrganisationId != organisationId)
            {
                return new UserResponse("User not found");
            }

            return new UserResponse(user);

        }

        public Task<UserResponse> ImportUsersAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<AppUser>> GetOrganisationUsersListAsync(int organisationId, UserQuery userQuery)
        {
            return await _applicationUserManager.GetOrganisationUsersListAsync(organisationId, userQuery);
        }

        public async Task<UserResponse> SaveAsync(int organisationId, AppUser user, List<string> userRoles, string password)
        {
            try
            {
                if (user.OrganisationId != organisationId)
                    return new UserResponse("You can only create a user in your Tenant");

                user.Status = EntityStatus.Active;
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

                return new UserResponse("Failed to create user", result.Errors);

            }
            catch (Exception ex)
            {
                return new UserResponse($"An error occurred while saving the user: {ex.Message}");
            }
        }

        public async Task<UserResponse> UpdateUserAsync(int organisationId, long userId, AppUser user, List<string> userRoles)
        {
            if (user.OrganisationId != organisationId)
                return new UserResponse("You can only create a user in your Tenant");

            var userFromRepo = await _applicationUserManager.FindByIdAsync(userId);
            var currentUserRoles = await _applicationUserManager.GetRolesAsync(userFromRepo);

            if (userFromRepo == null)
                return new UserResponse("User not found");


            userFromRepo.FirstName = user.FirstName;
            userFromRepo.LastName = user.LastName;

            try
            {
                var result = await _applicationUserManager.UpdateAsync(userFromRepo);

                if (result.Succeeded)
                {
                    if (userRoles.Count() > 0)
                    {
                        var rolesResult = await _applicationUserManager.AddToRolesAsync(userFromRepo, userRoles.Except(currentUserRoles));

                        if (!rolesResult.Succeeded)
                            return new UserResponse("Failed to add to roles", rolesResult.Errors);

                        rolesResult = await _applicationUserManager.RemoveFromRolesAsync(userFromRepo, currentUserRoles.Except(userRoles));

                        if (!rolesResult.Succeeded)
                            return new UserResponse("Failed to remove the roles");

                        return new UserResponse(userFromRepo);
                    }

                    return new UserResponse(userFromRepo);
                }

                return new UserResponse("Failed to update user", result.Errors);
            }
            catch (Exception ex)
            {
                return new UserResponse($"An error occured when updating user: {ex.Message}");
            }

        }
    }
}
