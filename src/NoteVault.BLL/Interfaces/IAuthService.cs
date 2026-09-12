using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Auth;

namespace NoteVault.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserRegisterResponseDto>> RegisterAsync(UserRegisterDto request, CancellationToken cancellationToken = default);
        Task<Result<UserLoginResponseDto>> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default);
    }
}
