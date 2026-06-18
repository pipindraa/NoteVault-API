using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using NoteVault_API.Models;
using NoteVault_API.Constants;

namespace NoteVault_API.Controllers
{
    [ApiController]
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
        public async Task<ActionResult<IReadOnlyCollection<NoteResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _noteService.GetAllAsync(cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.ErrorMessage ?? string.Empty,
                    Errors = result.Errors
                });
            }

            return Ok(result.Value);
        }

        [HttpGet(ApiRoutes.Notes.IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _noteService.GetByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(new ErrorResponse 
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.ErrorMessage ?? string.Empty,
                    Errors = result.Errors
                });
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> Create([FromBody] NoteCreateDto request, CancellationToken cancellationToken)
        {
            var result = await _noteService.CreateAsync(request, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.ErrorMessage ?? string.Empty,
                    Errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut(ApiRoutes.Notes.IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> Update([FromRoute] Guid id, [FromBody] NoteUpdateDto request, CancellationToken cancellationToken)
        {
            var result = await _noteService.UpdateAsync(id, request, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.ErrorMessage ?? string.Empty,
                    Errors = result.Errors
                });
            }

            return Ok(result.Value);
        }

        [HttpDelete(ApiRoutes.Notes.IdRoute)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _noteService.DeleteAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.ErrorMessage ?? string.Empty,
                    Errors = result.Errors
                });
            }

            return NoContent();
        }
    }
}