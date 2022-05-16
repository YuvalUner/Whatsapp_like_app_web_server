using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Services.TokenServices.Interfaces;
using Services.TokenServices.Implementations;
using Domain.CodeOnlyModels;

namespace AdvancedProjectWebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase {

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthTokenGenerator _authTokenGenerator;
        private readonly IConfiguration _configuration;

        public RefreshTokenController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {
            this._refreshTokenService = new RefreshTokenService(context);
            this._authTokenGenerator = new AuthTokenGenerator();
            this._configuration = config;
        }

        [HttpPost]
        public async Task<IActionResult> renewTokens(string? token) {
            if (token == null) {
                return BadRequest();
            }
            RefreshToken? rToken = await _refreshTokenService.GetToken(token);
            string? userAgent = Request.Headers["User-Agent"].ToString();
            if (rToken != null) {
                if (await _refreshTokenService.validateTokenExpiry(rToken) == true 
                    && rToken.UserAgent == userAgent) {

                    AuthToken aToken = _authTokenGenerator.GenerateAuthToken(rToken.RegisteredUserusername,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"],
                    20);

                    await _refreshTokenService.RemovePreviousTokens(rToken.RegisteredUserusername, userAgent);
                    await _refreshTokenService.storeRefreshToken(aToken.RefreshToken, rToken.RegisteredUserusername, userAgent); ;

                    return Ok(aToken);
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
