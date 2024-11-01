using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SavingAccount_BE.Model;
using SavingAccount_BE.Service.Users.Cards;
using SavingAccount_BE.Service.Users.Deposits;
using SavingAccount_BE.Service.Users.Histories;
using SavingAccount_BE.Service.Users.Profile;
using SavingAccount_BE.Service.Users.SavingAccounts;

namespace SavingAccount_BE.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserCard _userCardService;
        private readonly IUserHistory _userHistoryService;
        private readonly IUserSavingAccountService _userSavingAccountService;
        private readonly IUserProfile  _userProfileService;
        private readonly IDepositsAndWithdraws _depositsAndWithdraws;
        public UsersController(IUserCard userCardService,IUserHistory userHistory, IUserSavingAccountService userSavingAccountService , IUserProfile userProfile , IDepositsAndWithdraws depositsAndWithdraws)
        {
            _userCardService = userCardService;
            _userHistoryService = userHistory;
            _userSavingAccountService = userSavingAccountService;
            _userProfileService = userProfile;
            _depositsAndWithdraws = depositsAndWithdraws;
        }

        [HttpGet("Wallet")]
        //[Authorize(Roles = "User")]
        public IActionResult GetListCards([FromQuery] string userId)
        {
            return Ok(_userCardService.GetListCards(userId));
        }

        [HttpGet("History")]
        //[Authorize(Roles = "User")]
        public IActionResult GetListHistory([FromQuery] string userId)
        {
            return Ok(_userHistoryService.GetHistory(userId));
        }

        [HttpGet("ListSavingAccounts")]
        //[Authorize(Roles = "User")]
        public IActionResult GetListSavingAccounts([FromQuery] string userId)
        {
            _userSavingAccountService.updateSavingAccountBalance(userId);
            return Ok(_userSavingAccountService.GetListSavingAccounts(userId));
        }
        [HttpGet("Profile")]
        //[Authorize(Roles = "User")]
        public IActionResult GetProfile([FromQuery] string userId)
        {
            return Ok(_userProfileService.GetProfile(userId));
        }


        // POST api/<ValuesController>
        [HttpPost("update-profile")]
        public IActionResult UpdateProfile([FromQuery] string id, [FromBody] UserDTO updatedUser)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            if (updatedUser == null)
            {
                return BadRequest("Invalid user data.");
            }

            // Gọi hàm updateProfile từ service
            bool isUpdated = _userProfileService.updateProfile(id, updatedUser);

            if (isUpdated)
            {
                return Ok("Profile updated successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to update profile.");
            }
        }
        [HttpPost("opening-SavingAccount")]
        public async Task<IActionResult> openSavingAccount([FromBody] AddSavingAccountModel addSavingAccountModel)
        {
            if (addSavingAccountModel == null)
            {
                return BadRequest("Invalid Saving Account data.");
            }
            var isAdd = await _userSavingAccountService.AddSavingAccountAsync(addSavingAccountModel);
            if (isAdd) {
                return Ok("Add saving account success");
            }
            else
            {
                return StatusCode(500, "Failed to open saving account!");
            }
        }
        [HttpPost("deposits-SavingAccount")]
        public async Task<IActionResult> deposisSavingAccount([FromBody] SavingAccountDeposits addSavingAccount)
        {
            if(addSavingAccount == null)
            {
                return BadRequest("Invalid Saving Account data.");
            }
            var isDeposits = await _depositsAndWithdraws.depositsSavingAccount(addSavingAccount);
            if (isDeposits)
            {
                return Ok("Add saving account success");
            }
            else
            {
                return StatusCode(500, "Failed to open saving account!");
            }
        }
        [HttpPost("withdraws-SavingAccount")]
        public async Task<IActionResult> withdrawsSavingAccount([FromBody] SavingAccountWithdraws addSavingAccount)
        {
            if (addSavingAccount == null)
            {
                return BadRequest("Invalid Saving Account data.");
            }
            var isDeposits = await _depositsAndWithdraws.withdrawsSavingAccount(addSavingAccount);
            if (isDeposits)
            {
                return Ok("Add saving account success");
            }
            else
            {
                return StatusCode(500, "Failed to open saving account!");
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
