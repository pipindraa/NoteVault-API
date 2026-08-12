using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Auth;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(UserRegisterDto request, CancellationToken cancellationToken = default)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingEmail != null)
            {
                return Result<AuthResponseDto>.Failure(ErrorCode.UserAlreadyExists, "User with this email already exists");
            }

            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (existingUsername != null)
            {
                return Result<AuthResponseDto>.Failure(ErrorCode.UserAlreadyExists, "User with this username already exists");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user, cancellationToken);

            return Result<AuthResponseDto>.Success(CreateAuthResponse(user));
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Result<AuthResponseDto>.Failure(ErrorCode.InvalidCredentials, "Invalid email or password");
            }

            return Result<AuthResponseDto>.Success(CreateAuthResponse(user));
        }

        private AuthResponseDto CreateAuthResponse(User user)
        {

            var token = _jwtProvider.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token
            };
        }
    }
}
