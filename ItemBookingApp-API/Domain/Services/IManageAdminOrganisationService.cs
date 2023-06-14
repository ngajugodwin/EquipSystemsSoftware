using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IManageAdminOrganisationService
    {
        Task<PagedList<AppUser>> GetOrganisationUsersListAsync(int organisationId, UserQuery userQuery);

        Task<UserResponse> GetUserByIdAsync(long id, int organisationId);

        Task<UserResponse> SaveAsync(int organisationId, AppUser user, List<string> userRoles, string password);

        Task<UserResponse> UpdateUserAsync(int organisationId, long userId, AppUser user, List<string> userRoles);

        Task<UserResponse> DeleteByIdAsync(long userId);

        Task<UserResponse> ActivateOrDisableUser(int organisationId, long userId, bool userStatus);

        Task<UserResponse> ChangePassword(int organisationId, long userId, string oldPassword, string newPassword, bool isAdmin);

        Task<UserResponse> ImportUsersAsync(IFormFile file);

        Task<MemoryStream> ExportUsers();
    }
}
