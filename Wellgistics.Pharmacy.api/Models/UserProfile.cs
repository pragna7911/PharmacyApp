using Microsoft.EntityFrameworkCore;

namespace Wellgistics.Pharmacy.api.Models
{
    [Keyless]
    public class UserProfile
    {
        public int PharmacyInfoId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string initial { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public string legalName { get; set;}
        public bool? PasswordChanges { get; set; }
    }
    [Keyless]
    public class PharmacyEmployee
    {
        public int PharmacyInfoId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string initial { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public string legalName { get; set; }
        public string? password { get; set; }
    }
    public class UserPassword
    {
        public string email { get; set; }
        public string userId { get; set; }
        public string passwordChangedAt { get; set; }
        
    }
}
