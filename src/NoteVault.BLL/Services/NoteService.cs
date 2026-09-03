using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.BLL.Common;
using Mapster;
using NoteVault.BLL.DTOs.Pagination;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace NoteVault.BLL.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly ILogger<NoteService> _logger;

        public NoteService(INoteRepository noteRepository, ILogger<NoteService> logger)
        {
            _noteRepository = noteRepository;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<NoteResponseDto>>> GetAllAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var notes = await _noteRepository.GetAllAsync(
                userId,
                note => note.CreationDate,
                request.PageNumber,
                request.PageSize,
                descending: true, 
                cancellationToken);

            var dtos = notes.Adapt<IReadOnlyCollection<NoteResponseDto>>();
            return Result<IReadOnlyCollection<NoteResponseDto>>.Success(dtos);
        }

        public async Task<Result<NoteResponseDto>> GetByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default)
        {
            var note = await _noteRepository.GetByIdAsync(userId, noteId, cancellationToken);

            if (note is null)
            {
                return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
            }

            var dto = note.Adapt<NoteResponseDto>();
            return Result<NoteResponseDto>.Success(dto);
        }

        public async Task<Result<NoteResponseDto>> CreateAsync(Guid userId, NoteCreateDto request, CancellationToken cancellationToken = default)
        {
            var note = request.Adapt<Note>();
            note.Id = Guid.NewGuid();
            note.UserId = userId;
            note.CreationDate = DateTime.UtcNow;

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            var dto = createdNote.Adapt<NoteResponseDto>(); 
            return Result<NoteResponseDto>.Success(dto);
        }

        public async Task<Result<NoteResponseDto>> UpdateAsync(Guid userId, Guid noteId, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var existingNote = await _noteRepository.GetByIdAsync(userId, noteId, cancellationToken);
            if (existingNote is null)
            {
                return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
            }

            var note = new Note
            {
                Id = noteId,
                Name = request.Name,
                Description = request.Description
            };

            try
            {
                await _noteRepository.UpdateAsync(note, cancellationToken);

                var dto = note.Adapt<NoteResponseDto>();
                return Result<NoteResponseDto>.Success(dto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict during update for note {NoteId}.", noteId);

                return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating note {NoteId}.", noteId);
                return Result<NoteResponseDto>.Failure(ErrorCode.InternalServerError);
            }
        }        
        public async Task<Result> DeleteAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default)
        {
            var existingNote = await _noteRepository.GetByIdAsync(userId, noteId, cancellationToken);
            if (existingNote is null)
            {
                return Result.Failure(ErrorCode.NotFound);
            }

            var deleted = await _noteRepository.DeleteAsync(noteId, cancellationToken);
            if (!deleted)
            {
                return Result.Failure(ErrorCode.NotFound);
            }

            return Result.Success();
        }
    }
}
