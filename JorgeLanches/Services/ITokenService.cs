using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JorgeLanches.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
        
        string GenerateRefreshToken();
    }
}
