using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SavingAccount_BE.Data;
using SavingAccount_BE.Model;
using SavingAccount_BE.Service.Accounts;

namespace SavingAccount_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _repo;
        public AccountController(IAccountService repo) {
            _repo = repo;
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await _repo.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {

                return Ok("Signup successful");
            }

            return BadRequest(result.Errors);
        }
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _repo.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(result)) {
                return Unauthorized();
            }
            return Ok(result);

        }

        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyPassword([FromBody] UserDTO model)
        {
            var isPasswordValid = await _repo.VerifyPasswordAsync(model);
            if (!isPasswordValid)
            {
                return BadRequest("Mật khẩu hiện tại không chính xác.");
            }

            return Ok("Mật khẩu hợp lệ.");
        }


    }
}
