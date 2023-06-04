using ItemBookingApp_API.Areas.Resources.AppUser;

namespace ItemBookingApp_API.Areas.Resources.Organisation
{
    public class SaveOrganisationResource
    {      
        public string Name { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;     
        public DateTime DateOfIncorporation { get; set; }

        public bool IsActive { get; set; }

      //  public virtual ICollection<UserResource>? Users { get; set; }

        public SaveOrganisationResource()
        {
            IsActive = false;
        }
    }
}
