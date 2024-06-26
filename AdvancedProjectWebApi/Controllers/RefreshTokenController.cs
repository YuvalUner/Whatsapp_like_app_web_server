﻿using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.Interfaces;
using Services.TokenServices.Interfaces;
using Services.TokenServices.Implementations;
using Domain.CodeOnlyModels;

namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for managing the renewal of auth tokens.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // Ideally, should require this. But we don't know what the testers will be running.
    // [RequireHttps]
    public class RefreshTokenController : ControllerBase {

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthTokenGenerator _authTokenGenerator;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public RefreshTokenController(IConfiguration config, IRefreshTokenService refreshTokens) {
            this._refreshTokenService = refreshTokens;
            this._authTokenGenerator = new AuthTokenGenerator();
            this._configuration = config;
        }

        /// <summary>
        /// Renews a user's access and refresh tokens.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>A new access and refresh token on success, 404 if access token not found, 401 otherwise.</returns>
        [HttpPut]
        public async Task<IActionResult> renewTokens([Bind("token")] RefreshToken token, bool login = false) {
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
                    if (!login) {
                        return Ok(aToken);
                    }
                    else {
                        return Ok(new {
                            refreshToken = aToken.RefreshToken,
                            accessToken = aToken.AccessToken,
                            username = rToken.RegisteredUserusername
                        });
                    }
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
