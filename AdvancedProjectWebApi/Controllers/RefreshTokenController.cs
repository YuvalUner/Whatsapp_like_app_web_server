using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedProjectWebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RefreshTokenController : ControllerBase {

        private readonly IRefreshTokenService refreshTokenService;

        public RefreshTokenController(AdvancedProgrammingProjectsServerContext context) {
            this.refreshTokenService = new RefreshTokenService(context);
        }

        [HttpPost]
        public async Task<IActionResult> renewTokens(string? token) {
            if (token == null) {
                return BadRequest();
            }
            RefreshToken? rToken = await refreshTokenService.GetToken(token);
            if (rToken != null) {
                string? username = User.FindFirst("username")?.Value;
                if (rToken.RegisteredUserusername == username) {
                    if (await refreshTokenService.validateTokenExpiry(rToken) == true) {
                        rToken = await refreshTokenService.renewRefreshToken(rToken);
                        return Ok(rToken.Token);
                    }
                    return NotFound();
                }
                return Forbid();
            }
            return BadRequest();
        }
    }
}
