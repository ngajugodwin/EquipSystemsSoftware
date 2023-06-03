namespace ItemBookingApp_API.Domain.Models
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfIncorporation { get; set; }
    }
}
