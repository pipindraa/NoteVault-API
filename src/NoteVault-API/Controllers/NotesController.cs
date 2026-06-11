using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using static NoteVault_API.Constants.ApiRoutes.Notes;

namespace NoteVault_API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
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
            var notes = await _noteService.GetAllAsync(cancellationToken);
            return Ok(notes);
        }

        [HttpGet(IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var note = await _noteService.GetByIdAsync(id, cancellationToken);
            return note is null ? NotFound() : Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> Create([FromBody] NoteCreateDto request, CancellationToken cancellationToken)
        {
            var createdNote = await _noteService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut(IdRoute)]
        public async Task<ActionResult<NoteResponseDto>> Update(Guid id, [FromBody] NoteUpdateDto request, CancellationToken cancellationToken)
        {
            var updatedNote = await _noteService.UpdateAsync(id, request, cancellationToken);
            return updatedNote is null ? NotFound() : Ok(updatedNote);
        }

        [HttpDelete(IdRoute)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _noteService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}