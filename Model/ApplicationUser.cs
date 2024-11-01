using Microsoft.AspNetCore.Identity;

namespace SavingAccount_BE.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; } = new DateTime(2000, 01, 01);
        public string Province { get; set; }
        public string City { get; set; }
        public string Address {  get; set; }
        public string Nation { get; set; }
        public string? Base64Image { get; set; }

        
    }
}
