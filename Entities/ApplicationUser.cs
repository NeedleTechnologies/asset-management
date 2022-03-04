using System;
using Microsoft.AspNetCore.Identity;

namespace asset_management.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
    }
}