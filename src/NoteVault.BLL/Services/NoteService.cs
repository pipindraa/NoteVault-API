using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.BLL.Common;
using Mapster;
using NoteVault.BLL.DTOs.Pagination;
using Microsoft.Extensions.Logging;

namespace NoteVault.BLL.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<NoteService> _logger;

        public NoteService(INoteRepository noteRepository,ITagRepository tagRepository, ILogger<NoteService> logger)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<NoteResponseDto>>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var notes = await _noteRepository.GetAllAsync(
                note => note.CreationDate,
                request.PageNumber,
                request.PageSize,
                descending: true, 
                cancellationToken);

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

            if (request.TagIds.Any())
            {
                var tags = await _tagRepository.GetByIdsAsync(request.TagIds, cancellationToken);
                note.Tags = tags;
            }

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

            if (request.TagIds.Any())
            {
                var tags = await _tagRepository.GetByIdsAsync(request.TagIds, cancellationToken);
                note.Tags = tags;
            }

            try
            {
                await _noteRepository.UpdateAsync(note, cancellationToken);

                var dto = note.Adapt<NoteResponseDto>();
                return Result<NoteResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                var existingNote = await _noteRepository.GetByIdAsync(id, cancellationToken);

                if (existingNote is null)
                {
                    return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
                }

                _logger.LogError(ex, "Failed to update note with id {NoteId}.", id);

                return Result<NoteResponseDto>.Failure(ErrorCode.ValidationError);
            }
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
