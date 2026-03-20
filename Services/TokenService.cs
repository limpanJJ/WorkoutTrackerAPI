using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    internal sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user, IList<string> roles)
        {
            var jwtSecret = _configuration["Jwt:SecretKey"] ?? Environment.GetEnvironmentVariable("Jwt__SecretKey");
            if (string.IsNullOrWhiteSpace(jwtSecret))
                throw new InvalidOperationException("Jwt:SecretKey not configured. Set it via User Secrets, .env or environment variable.");

            byte[] keyBytes;
            try
            {
                // Prefer base64-secret if provided
                keyBytes = Convert.FromBase64String(jwtSecret);
            }
            catch
            {
                // Fallback to UTF8 bytes for hex/raw secrets
                keyBytes = Encoding.UTF8.GetBytes(jwtSecret);
            }

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            // Add a role claim for each role the user belongs to
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiresInMinutes = _configuration.GetValue<int?>("Jwt:ExpirationInMinutes") ?? 60;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}