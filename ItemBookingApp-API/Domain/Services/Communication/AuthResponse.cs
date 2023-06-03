using ItemBookingApp_API.Resources.Auth;
using Microsoft.Identity.Client;

namespace ItemBookingApp_API.Domain.Services.Communication
{
    public class AuthResponse : BaseResponse<TokenResource>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="token">Token response.</param>
        /// <returns>Response.</returns>
        public AuthResponse(TokenResource token) : base(token)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public AuthResponse(string message) : base(message)
        { }
    }
}
