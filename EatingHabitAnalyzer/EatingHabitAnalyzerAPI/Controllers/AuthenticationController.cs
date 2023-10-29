using EatingHabitAnalyzerAPI.Models;
using EatingHabitAnalyzerAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EatingHabitAnalyzerAPI.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private ILogger<FoodController> _logger;

        private IDatabaseService _service;

        private string _issuer;

        private string _audience;

        private SigningCredentials _credentials;

        public AuthenticationController(ILogger<FoodController> logger, IDatabaseService service, IConfiguration configuration)
        {
            if (configuration == null || configuration["JwtSettings:Issuer"] == null || configuration["JwtSettings:Audience"] == null || configuration["JwtSettings:Key"] == null)
            {
                throw new ArgumentException("Invalid JWT Settings");
            }
            _logger = logger;
            _service = service;
            _issuer = configuration["JwtSettings:Issuer"]!;
            _audience = configuration["JwtSettings:Audience"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!));
            _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        [HttpPost, Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login(string email, string pin)
        {
            var result = _service.GetUserAsync(email).GetAwaiter().GetResult();
            if (result == null)
            {
                return BadRequest("User Name Or Pin is incorrect");
            }
            else
            {
                if (result.Pin == pin)
                {
                    return Ok(GenerateJSONWebToken(result));
                }
                else
                {
                    return BadRequest("User Name Or Pin is incorrect");
                }
            }
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var userIdClaim = new Claim("UserId", userInfo.UserId.ToString());
            var userEmailClaim = new Claim("UserEmail", userInfo.Email);
            var token = new JwtSecurityToken(_issuer, _audience, claims: new[] { userIdClaim,userEmailClaim },expires: DateTime.Now.AddMinutes(120),signingCredentials: _credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost, Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Register(User user)
        {
            var result = _service.GetUserAsync(user.Email).GetAwaiter().GetResult();
            if (result == null)
            {
                _service.InsertNewUser(user).GetAwaiter().GetResult();
                return Ok();
            }
            else
            {
                return BadRequest("User already exists");
            }
        }

    }
}
