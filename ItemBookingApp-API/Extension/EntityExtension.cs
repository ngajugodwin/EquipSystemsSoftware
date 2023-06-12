using ItemBookingApp_API.Areas.Resources.Organisation;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;

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

        public static string GetAccountType(this AccountType accountType)
        {
            var result = string.Empty;

            switch (accountType)
            {
                case AccountType.Individual:
                     result = "INDIVIDUAL";
                    break;
                case AccountType.Organisation:
                    result = "ORGANISATION";
                    break;
                case AccountType.Master:
                    result = "MASTER";
                    break;
                default:
                    result = "NOT_SET";
                    break;
            }

            return result;
        }
    }
}
