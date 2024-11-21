using Microsoft.AspNetCore.Mvc;
using TestDevWebApi.Models;
using TestDevWebApi.Services;

namespace TestDevWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

         
        [HttpGet("balances")]
        public async Task<ActionResult<List<AccountBalanceSummary>>> GetAccountBalances()
        {
            try
            {
                var result = await _accountService.GetAccountBalancesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        

        [HttpGet("GetDetails/{topAccountId}")]
        public async Task<IActionResult> GetAccountPaths(string topAccountId)
        {
            if (string.IsNullOrWhiteSpace(topAccountId))
            {
                return BadRequest("Top account ID is required.");
            }

            try
            {
                var paths = await _accountService.GetAccountPathsAsync(topAccountId);

                if (!paths.Any())
                {
                    return NotFound("No paths found.");
                }

                return Ok(paths);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}