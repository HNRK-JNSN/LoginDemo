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

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> AuthenticateUser([FromBody] Login model)
        {
            if (model == null)
            {
                return NotFound();
            }
            Console.WriteLine($"Authenticating ${model.EmailAddress}");

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