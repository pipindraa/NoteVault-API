using NoteVault.BLL.Interfaces;
using System.Security.Claims;

namespace NoteVault_API.Extensions
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool TryGetUserId(out Guid userId)
        {
            var userIdValue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdValue, out userId);
        }
    }
}
