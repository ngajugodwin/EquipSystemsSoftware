﻿namespace ItemBookingApp_API.Resources.Order
{
    public class AddressDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string? Country { get; set; }
        public string ZipCode { get; set; }
    }
}
