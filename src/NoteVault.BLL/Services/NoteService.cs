using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Exceptions;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Interfaces;
using NoteVault.BLL.Constants;
using NoteVault.BLL.Mappers;

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
            return notes.Select(note => note.ToResponseDto()).ToList();
        }

        public async Task<NoteResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _noteRepository.GetByIdAsync(id, cancellationToken);

            if (note is null)
                throw new NotFoundException(string.Format(NoteErrorMessages.NotFoundTemplate, id));

            return note.ToResponseDto();
        }

        public async Task<NoteResponseDto> CreateAsync(NoteCreateDto request, CancellationToken cancellationToken = default)
        {
            var note = request.ToEntity();

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            return createdNote.ToResponseDto();
        }

        public async Task<NoteResponseDto> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var note = request.ToEntity(id);

            var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

            if (updatedNote is null)
                throw new NotFoundException(string.Format(NoteErrorMessages.NotFoundTemplate, id));

            return updatedNote.ToResponseDto();
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deleted = await _noteRepository.DeleteAsync(id, cancellationToken);
            if (!deleted)
                throw new NotFoundException(string.Format(NoteErrorMessages.NotFoundTemplate, id));
        }
    }
}
