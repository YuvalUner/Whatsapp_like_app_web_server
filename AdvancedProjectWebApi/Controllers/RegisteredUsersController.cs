using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Data;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase {

        public IConfiguration _configuration;

        private readonly IRegisteredUsersService _registeredUsersService;

        public RegisteredUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {

            this._registeredUsersService = new DatabaseRegisteredUsersService(context);
            this._configuration = config;

        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string? username) {

            if (await _registeredUsersService.doesUserExists(username) == true) {

                //HttpContext.Session.SetString("username", username);
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWTBearerParams:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("username", username)
                };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTBearerParams:Key"]));
                var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    signingCredentials: mac
                    );
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return NotFound();
        }
    }
}
