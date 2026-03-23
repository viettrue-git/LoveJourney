using System.Security.Claims;

namespace LoveJourney.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid coupleId, string email);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
