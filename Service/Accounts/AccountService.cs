using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SavingAccount_BE.Data;
using SavingAccount_BE.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SavingAccount_BE.Service.Accounts
{
    public class AccountService : IAccountService
    { 
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly IConfiguration _configuration;
        private readonly SavingAccountDbContext _dbContext;
        public AccountService(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , IConfiguration configuration, SavingAccountDbContext dbContext) { 
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded) {
                return string.Empty;
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Email , model.Email),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
            };
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var authenKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddDays(365),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                BirthDate = model.BirthDate,
                Address = model.Address,
                City = model.City,
                Province = model.Province,
                Nation = model.Nation,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = model.Password,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
                return result;
            }
            await _userManager.AddToRoleAsync(user, "User");
            _dbContext.Users.Add(new User
            {
                IdUser = user.Id,
            });
            await _dbContext.SaveChangesAsync();
            return result;
        }
        public async Task<bool> VerifyPasswordAsync(UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.IdUser);
            if (user == null)
            {
                return false;
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.PasswordHash);
            if (isPasswordValid)
            {
                await _userManager.ChangePasswordAsync(user, model.PasswordHash, model.NewPassword);
            }
            return isPasswordValid;
        }
    }
}
