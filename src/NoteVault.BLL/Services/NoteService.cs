using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.BLL.Common;
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

        public async Task<Result<IReadOnlyCollection<NoteResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var notes = await _noteRepository.GetAllAsync(note => note.CreationDate, descending: true, cancellationToken);
            var dtos = notes.Adapt<IReadOnlyCollection<NoteResponseDto>>();
            return Result<IReadOnlyCollection<NoteResponseDto>>.Success(dtos);
        }

        public async Task<Result<NoteResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var note = await _noteRepository.GetByIdAsync(id, cancellationToken);

            if (note is null)
            {
                return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
            }

            var dto = note.Adapt<NoteResponseDto>();
            return Result<NoteResponseDto>.Success(dto);
        }

        public async Task<Result<NoteResponseDto>> CreateAsync(NoteCreateDto request, CancellationToken cancellationToken = default)
        {
            var note = request.Adapt<Note>();
            note.Id = Guid.NewGuid();
            note.CreationDate = DateTime.UtcNow;

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            var dto = createdNote.Adapt<NoteResponseDto>(); 
            return Result<NoteResponseDto>.Success(dto);
        }

        public async Task<Result<NoteResponseDto>> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var note = new Note
            {
                Id = id,
                Name = request.Name,
                Description = request.Description
            };

            var updatedNote = await _noteRepository.UpdateAsync(note, cancellationToken);

            if (updatedNote is null)
            {
                return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
            }

            var dto = updatedNote.Adapt<NoteResponseDto>();
            return Result<NoteResponseDto>.Success(dto);
        }
        
        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deleted = await _noteRepository.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return Result.Failure(ErrorCode.NotFound);
            }

            return Result.Success();
        }
    }
}
