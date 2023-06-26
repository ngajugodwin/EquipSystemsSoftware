using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IUserService
    {
        Task<UserResponse> ApproveUser(long userId);
        Task<UserResponse> RejectUser(long userId);
        Task<UserAddress> UpdateUserAddress(long userId, UserAddress userAddressToUpdate);
        Task<UserAddress> GetUserAddress(long userId);
        Task<PagedList<AppUser>> ListAsync(UserQuery userQuery);

        Task<PagedList<AppUser>> ListAsyncV2(UserQuery userQuery);

        Task<UserResponse> GetUserByIdAsync(long id);

        Task<AppUser> GetUserByEmailAsync(string email);

        Task<UserResponse> SaveAsync(AppUser user, bool isExternalReg, List<string> userRoles, string password);

        Task<UserResponse> UpdateAsync(long userId, AppUser user, List<string> userRoles);

        Task<UserResponse> DeleteByIdAsync(long userId);

        Task<UserResponse> ActivateOrDisableUser(long userId, bool userDeactivationStatus);

        Task<UserResponse> ChangePassword(long userId, string oldPassword, string newPassword, bool isAdmin);

        Task<UserResponse> ImportUsersAsync(IFormFile file);

        Task<MemoryStream> ExportUsers();
    }
}
