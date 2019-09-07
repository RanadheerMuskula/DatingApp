using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using datingApp.API.Data;
using datingApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace datingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
              private readonly IConfiguration _config;
        private readonly IAuthRepository _repos;
        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            _repos=repo;
            _config=config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForregisterDto UserForregisterDto)
        {
        //     if (!ModelState.IsValid){
        //         return BadRequest(ModelState);
        //     }
            {
                UserForregisterDto.Username = UserForregisterDto.Username.ToLower();
                if (await _repos.UserExists(UserForregisterDto.Username))
                {
                    return BadRequest("user already Exits");
                }
                var usertocreate = new User
                {
                    Username = UserForregisterDto.Username
                };
                var CreateUser = await _repos.Register(usertocreate, UserForregisterDto.Password);
                return StatusCode(201);
            }
            
        }
         [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
           userForLoginDto.Username= userForLoginDto.Username.ToLower();
            var userFromRepo = await _repos.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}