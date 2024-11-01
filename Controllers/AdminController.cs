using Microsoft.AspNetCore.Mvc;
using SavingAccount_BE.Model;
using SavingAccount_BE.Service.Admin.AddUserCard;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SavingAccount_BE.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAddUserCard _addUserCard;
        public AdminController(IAddUserCard addUserCard)
        {
            _addUserCard = addUserCard;
        }
        // GET: api/<AdminController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AdminController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("AddCardUser")]
        public IActionResult AddCard([FromBody] CardAddModel value)
        {
            bool result =  _addUserCard.AddCard(value);
            if (!result) {
                return StatusCode(500 , "Add Card Error!"); 
            }
            return Ok("Add Card Complete");
            
        }

        // PUT api/<AdminController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
