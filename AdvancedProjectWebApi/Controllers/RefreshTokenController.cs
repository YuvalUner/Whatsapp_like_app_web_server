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

    /// <summary>
    /// A controller for managing the renewal of auth tokens.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase {

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthTokenGenerator _authTokenGenerator;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public RefreshTokenController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {
            this._refreshTokenService = new RefreshTokenService(context);
            this._authTokenGenerator = new AuthTokenGenerator();
            this._configuration = config;
        }

        /// <summary>
        /// Renews a user's access and refresh tokens.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>A new access and refresh token on success, 404 if access token not found, 401 otherwise.</returns>
        [HttpPut]
        public async Task<IActionResult> renewTokens([Bind("token")] RefreshToken token) {
            if (token == null) {
                return BadRequest();
            }
            RefreshToken? rToken = await _refreshTokenService.GetToken(token.Token);
            string? userAgent = Request.Headers["User-Agent"].ToString();
            if (rToken != null) {
                if (await _refreshTokenService.validateTokenExpiry(rToken) == true 
                    && rToken.UserAgent == userAgent) {

                    AuthToken aToken = _authTokenGenerator.GenerateAuthToken(rToken.RegisteredUserusername,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"]);

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
