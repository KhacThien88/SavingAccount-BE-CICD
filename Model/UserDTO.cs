namespace SavingAccount_BE.Model
{
    public class UserDTO
    {
        public string? IdUser {  get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? Nation { get; set; }
        public string? PasswordHash { get; set; }
        public string? NewPassword {get; set; }
        public string? SecurityStampHash { get; set; }
        public bool? TwoFactorEndable { get; set; }
        public bool? LockoutEndable { get; set; }
        public string? Base64Image { get; set; }
    }
}
