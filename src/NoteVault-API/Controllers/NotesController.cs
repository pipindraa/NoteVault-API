using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using static NoteVault_API.Constants.ApiRoutes.Notes;
using static NoteVault_API.Constants.ApiVersions;

namespace NoteVault_API.Controllers
{
    [ApiController]
    [ApiVersion(V1)]
    [Route(RoutePrefix)]
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
            return Ok(result.Value);
        }

        [HttpGet(IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _noteService.GetByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(new { error = result.ErrorMessage });
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> Create([FromBody] NoteCreateDto request, CancellationToken cancellationToken)
        {
            var result = await _noteService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut(IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> Update(Guid id, [FromBody] NoteUpdateDto request, CancellationToken cancellationToken)
        {
            var result = await _noteService.UpdateAsync(id, request, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(new { error = result.ErrorMessage });
            }

            return Ok(result.Value);
        }

        [HttpDelete(IdRoute)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _noteService.DeleteAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(new { error = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}