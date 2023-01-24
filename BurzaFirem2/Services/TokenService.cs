using BurzaFirem2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BurzaFirem2.Services
{
    public class TokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly JWTOptions _options;
        private readonly UserManager<ApplicationUser> _um;

        public TokenService(ILogger<TokenService> logger, IOptions<JWTOptions> options, UserManager<ApplicationUser> um)
        {
            _logger = logger;
            _options = options.Value;
            _um = um;
        }

        public async Task<AuthenticationToken> Issue(ApplicationUser user)
        {
            return await GenerateAuthenticationToken(user);
        }

        private async Task<AuthenticationToken?> GenerateAuthenticationToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_options.Key);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new("sub", user.Id.ToString()),
                new(ClaimTypes.Email, user.Email)
            };

            var userClaims = await _um.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_options.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationToken { Name = "authentication_token", Value = tokenHandler.WriteToken(token) };
        }
    }

    public class JWTOptions
    {
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public int Expiration { get; set; }
    }
}
