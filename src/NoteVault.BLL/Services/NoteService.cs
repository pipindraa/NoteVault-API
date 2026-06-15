using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Exceptions;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Interfaces;
using NoteVault.BLL.Constants;
using NoteVault.DAL.Entities;
using Mapster;

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
            var notes = await _noteRepository.GetAllAsync(note => note.CreationDate, descending: true, cancellationToken);
            return notes.Adapt<IReadOnlyCollection<NoteResponseDto>>();
        }

        public async Task<NoteResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _noteRepository.GetByIdAsync(id, cancellationToken);

            if (note is null)
                throw new NotFoundException(string.Format(NoteErrorMessages.NotFoundTemplate, id));

            return note.Adapt<NoteResponseDto>();
        }

        public async Task<NoteResponseDto> CreateAsync(NoteCreateDto request, CancellationToken cancellationToken = default)
        {
            var note = request.Adapt<Note>();
            note.Id = Guid.NewGuid();
            note.CreationDate = DateTime.UtcNow;

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            return createdNote.Adapt<NoteResponseDto>();
        }

        public async Task<NoteResponseDto> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var note = request.Adapt<Note>();
            note.Id = id;

            var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

            if (updatedNote is null)
                throw new NotFoundException(string.Format(NoteErrorMessages.NotFoundTemplate, id));

            return updatedNote.Adapt<NoteResponseDto>();
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deleted = await _noteRepository.DeleteAsync(id, cancellationToken);
            if (!deleted)
                throw new NotFoundException(string.Format(NoteErrorMessages.NotFoundTemplate, id));
        }
    }
}
