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

        public async Task<Result<UserRegisterResponseDto>> RegisterAsync(UserRegisterDto request, CancellationToken cancellationToken = default)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingEmail is not null)
            {
                return Result<UserRegisterResponseDto>.Failure(ErrorCode.UserAlreadyExists, "User with this email already exists");
            }

            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            if (existingUsername is not null)
            {
                return Result<UserRegisterResponseDto>.Failure(ErrorCode.UserAlreadyExists, "User with this username already exists");
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

            var response = new UserRegisterResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return Result<UserRegisterResponseDto>.Success(response);
        }

        public async Task<Result<UserLoginResponseDto>> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Result<UserLoginResponseDto>.Failure(ErrorCode.InvalidCredentials, "Invalid email or password");
            }

            var token = _jwtProvider.GenerateToken(user);

            var response = new UserLoginResponseDto
            {
                UserId = user.Id,
                Token = token
            };

            return Result<UserLoginResponseDto>.Success(response);
        }
    }
}
