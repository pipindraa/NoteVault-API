using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Auth;

namespace NoteVault.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(UserRegisterDto request, CancellationToken cancellationToken = default);
        Task<Result<AuthResponseDto>> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default);
    }
}
