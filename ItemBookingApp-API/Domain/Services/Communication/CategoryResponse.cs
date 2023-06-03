using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Domain.Services.Communication
{
    public class CategoryResponse : BaseResponse<Category>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="itemType">ItemType response.</param>
        /// <returns>Response.</returns>
        public CategoryResponse(Category category) : base(category)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public CategoryResponse(string message) : base(message)
        { }
    }
}
