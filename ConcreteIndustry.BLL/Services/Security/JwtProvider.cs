using ConcreteIndustry.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ConcreteIndustry.BLL.Services.Security
{
    public static class JwtProvider
    {
        private static string? _key;
        private static string? _issuer;
        private static string? _audience;

        public static void Initialize(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }

        public static string CreateToken(AppUser user, out DateTime expired)
        {
            var originalkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            expired = DateTime.UtcNow.AddMinutes(15);
            var claims = new List<Claim>
            {
                new Claim("emailaddress", user.Email),
                new Claim("username", user.UserName),
                new Claim("firstname", user.FirstName),
                new Claim("lastname", user.LastName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userid", user.Id.ToString()),
                new Claim("expire", expired.ToString())
            };

            var credentials = new SigningCredentials(originalkey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expired,
                SigningCredentials = credentials,
                Issuer = _issuer,
                Audience = _audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;    
        }

        public static string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static string HashToken(string accesToken)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(accesToken));
                return Convert.ToBase64String(bytes);
            }
        }

        private static byte[] GetValidKeyFromConfig()
        {
            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentNullException("_key", "Key not set in appsettings.json");
            }

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(_key));
            }
        }
    }
}
