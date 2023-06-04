using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Domain.Services.Communication
{
    public class OrganisationResponse : BaseResponse<Organisation>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="itemType">ItemType response.</param>
        /// <returns>Response.</returns>
        public OrganisationResponse(Organisation organisation) : base(organisation)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public OrganisationResponse(string message) : base(message)
        { }
    }
}
