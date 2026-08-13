using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.DTOs.Auth;
using NoteVault.BLL.Interfaces;
using NoteVault_API.Constants;
using NoteVault_API.Extensions.Results;

namespace NoteVault_API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRoutes.Auth.RoutePrefix)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(ApiRoutes.Auth.Register)]
        public async Task<ActionResult<UserRegisterResponseDto>> Register([FromBody] UserRegisterDto request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<ActionResult<UserLoginResponseDto>> Login([FromBody] UserLoginDto request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            return result.ToActionResult();
        }
    }
}