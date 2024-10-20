using JwtPratik.Context;
using JwtPratik.Dtos;
using JwtPratik.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtPratik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PratikJwtDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(PratikJwtDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLogin)
        {
            var user = _context.Users.FirstOrDefault(x=>x.Email == userLogin.Email && x.Password == userLogin.Password);

            if (user is null)
            {
                return BadRequest();
            }

            var token = GenerateJwt(user);

            return Ok(new {Token = token});
        }

        private  string GenerateJwt(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken
                (
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
