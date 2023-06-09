﻿using ItemBookingApp_API.Areas.Resources.Organisation;
using ItemBookingApp_API.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace ItemBookingApp_API.Areas.Resources.AppUser
{
    public class SaveUserResource
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string[]? Roles { get; set; }

        public int OrganisationId { get; set; }

        public Domain.Models.Organisation? Organisation { get; set; }

        public int AccountType { get; set; }

        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public bool IsExternalReg { get; set; }

        public IFormFile? File { get; set; }


        // public SaveOrganisationResource SaveOrganisationResource { get; set; }


    }
}
