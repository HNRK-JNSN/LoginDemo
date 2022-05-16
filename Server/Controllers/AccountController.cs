using Microsoft.AspNetCore.Mvc;
using LoginDemo.Server.Repositories;
using LoginDemo.Shared.Models;

namespace LoginDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository accountRepo, ILogger<AccountController> logger)
        {
            _accountRepo = accountRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _accountRepo.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("user")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddUser([FromBody] AddUser model)
        {
            try {
                var aff = await _accountRepo.AddUser(model);

                if (aff > 0) 
                {
                    return Ok();
                } 
                else 
                {
                    return StatusCode(409, "Could not insert user");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpPost("authenticate")]
        [Consumes("application/json")]
        public async Task<IActionResult> AuthenticateUser([FromBody] Login model)
        {
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                var user = await _accountRepo.AuthenticateUser(model);               
                return Ok(user);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}