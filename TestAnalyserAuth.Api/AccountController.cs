using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestAnalyserAuth.Domain.Entity;
using TestAnalyserAuth.Domain.Security;

namespace TestAnalyserAuth;

[Route("identity")]
public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных
        private List<User> people = new List<User>
        {
            new User {Login="admin@gmail.com", Password="12345", Role = "admin" },
            new User { Login="qwerty@gmail.com", Password="55555", Role = "user" }
        };

        [Authorize (AuthenticationSchemes ="Bearer")]
        [Authorize(Policy = "OnlyPaid")]
        [HttpGet("/myRole")]
        public string? GetRole()
        {
            return User.Identity?.Name;
        }
        
        [HttpPost("/token")]
        public IActionResult Token([FromForm]string username, [FromForm]string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
 
            return Json(response);
        }
 
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person == null) return null!;
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, person.Login),
                new(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                new(CustomClaimTypes.PAYMENT_PLAN, "paid")

            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;

        }
    }