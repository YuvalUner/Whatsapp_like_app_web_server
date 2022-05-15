using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Services.TokenServices.Interfaces;
using Services.TokenServices.Implementations;

namespace AdvancedProjectWebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RefreshTokenController : ControllerBase {

        private readonly IRefreshTokenService refreshTokenService;
        private readonly IAuthTokenGenerator _authTokenGenerator;
        private readonly IConfiguration _configuration;

        public RefreshTokenController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {
            this.refreshTokenService = new RefreshTokenService(context);
            this._authTokenGenerator = new AuthTokenGenerator();
            this._configuration = config;
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

                        return Ok(_authTokenGenerator.GenerateAuthToken(username,
                        _configuration["JWTBearerParams:Subject"],
                        _configuration["JWTBearerParams:Key"],
                        _configuration["JWTBearerParams:Issuer"],
                        _configuration["JWTBearerParams:Audience"],
                        20));
                        }
                        return NotFound();

                }
                return Forbid();
            }
            return BadRequest();
        }
    }
}
