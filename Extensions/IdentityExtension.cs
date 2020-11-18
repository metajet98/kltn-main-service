using System.Security.Claims;
using System.Security.Principal;

namespace main_service.Extensions
{
    public static class IdentityExtension
    {
        public static int GetId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
            int.TryParse(claim?.Value, out var userId);
            return userId;
        }
    }
}