using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IReadOnlyCollection<NoteResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var notes = await _noteRepository.GetAllAsync(cancellationToken);
            return notes.Select(MapToResponseDto).ToList();
        }

        public async Task<NoteResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _noteRepository.GetByIdAsync(id, cancellationToken);
            return note is null ? null : MapToResponseDto(note);
        }

        public async Task<NoteResponseDto> CreateAsync(NoteCreateDto request, CancellationToken cancellationToken = default)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ImageUrls = request.ImageUrls,
                CreationDate = DateTime.UtcNow
            };

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            return MapToResponseDto(createdNote);
        }

        public async Task<NoteResponseDto?> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var note = new Note
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                ImageUrls = request.ImageUrls
            };

            var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);
            return updatedNote is null ? null : MapToResponseDto(updatedNote);
        }
        
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _noteRepository.DeleteAsync(id, cancellationToken);
        }


        private static NoteResponseDto MapToResponseDto(Note note)
        {
            return new NoteResponseDto
            {
                Id = note.Id,
                Name = note.Name,
                Description = note.Description,
                ImageUrls = note.ImageUrls,
                CreationDate = note.CreationDate
            };
        }
    }
}
