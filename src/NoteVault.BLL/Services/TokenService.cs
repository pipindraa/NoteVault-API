using Mapster;
using NoteVault.BLL.DTOs.Auth;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Entities;

namespace NoteVault.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IJwtProvider _jwtProvider;

        public TokenService(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public string GenerateToken(User user)
        {
            var authUserDto = user.Adapt<AuthUserDto>();
            return _jwtProvider.GenerateToken(authUserDto);
        }
    }
}
