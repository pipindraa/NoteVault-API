using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.DTOs.Pagination;
using NoteVault.BLL.Interfaces;
using NoteVault_API.Constants;
using NoteVault_API.Extensions;
using NoteVault_API.Extensions.Results;

namespace NoteVault_API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRoutes.Notes.RoutePrefix)]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<NoteResponseDto>>> GetPage([FromQuery] PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _noteService.GetAllAsync(userId, pagination, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet(ApiRoutes.Notes.IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _noteService.GetByIdAsync(userId, id, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> Create([FromBody] NoteCreateDto request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _noteService.CreateAsync(userId, request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut(ApiRoutes.Notes.IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> Update([FromRoute] Guid id, [FromBody] NoteUpdateDto request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _noteService.UpdateAsync(userId, id, request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete(ApiRoutes.Notes.IdRoute)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _noteService.DeleteAsync(userId, id, cancellationToken);
            return result.ToActionResult();
        }
    }
}