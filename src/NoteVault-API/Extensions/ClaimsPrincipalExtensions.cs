using System.Security.Claims;

namespace NoteVault_API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim is missing.");
            }

            return Guid.Parse(userIdClaim);
        }
    }
}
