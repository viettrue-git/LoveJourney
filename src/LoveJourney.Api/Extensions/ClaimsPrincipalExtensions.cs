using System.Security.Claims;

namespace LoveJourney.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetCoupleId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(id!);
    }
}
