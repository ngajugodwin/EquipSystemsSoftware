using ItemBookingApp_API.Areas.Resources.Organisation;
using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Extension
{
    public static class EntityExtension
    {
        public static OrganisationResource GetOrganisation(this Organisation organisation)
        {
            if (organisation != null)
            {
                var orgResource = new OrganisationResource
                {
                    Id = organisation.Id,
                    Name = organisation.Name
                };

                return orgResource;
            }

            return new OrganisationResource();
        }
    }
}
