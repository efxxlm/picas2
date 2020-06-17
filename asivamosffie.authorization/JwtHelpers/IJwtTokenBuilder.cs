using AuthorizationTest.JwtHelpers;
using System.Collections.Generic;
using System.Security.Claims;

namespace asivamosffie.api.Controllers
{
    public interface IJwtTokenBuilder
    {
        JwtToken Build();
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateAccessToken(IEnumerable<Claim> claims, string serverSigningPassword, string accessTokenDurationInMinutes);
    }
}