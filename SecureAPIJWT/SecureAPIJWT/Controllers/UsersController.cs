using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureAPIJWT.Data;
using SecureAPIJWT.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureAPIJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSettings _appSettings;

        public UsersController(MyDbContext myDbContext, IOptionsMonitor<AppSettings> optionsMonitor) 
        {
            _context = myDbContext;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(p=>p.UserName == model.UserName && p.Password == model.Password);
            
            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                });
            }

          


            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                // cap token
                Data = GenerateToken(user)
            });
        }

        private string GenerateToken(Users users)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKetBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescripttion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name , users.FullName),
                    new Claim(ClaimTypes.Email , users.Email),
                    new Claim("UserName" , users.UserName),
                    new Claim("ID" , users.ID.ToString()),

                     // roles

                    new Claim("TokenID", Guid.NewGuid().ToString())
                }),
                // thoi gian het han
                Expires = DateTime.UtcNow.AddMinutes(1),
               // ky
               SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKetBytes),
               SecurityAlgorithms.HmacSha512Signature)
               
            };

            var token = jwtTokenHandler.CreateToken(tokenDescripttion);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
