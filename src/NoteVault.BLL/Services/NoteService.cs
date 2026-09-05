using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.DTOs.Pagination;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;

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

        public async Task<Result<PagedResponse<NoteResponseDto>>> GetPageAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var (notes, totalCount) = await _noteRepository.GetPageAsync(
                userId,
                note => note.CreationDate,
                request.PageNumber,
                request.PageSize,
                descending: true,
                cancellationToken);

            var dtos = notes.Adapt<IReadOnlyCollection<NoteResponseDto>>();

            var response = new PagedResponse<NoteResponseDto>(dtos, request.PageNumber, request.PageSize, totalCount);
            return Result<PagedResponse<NoteResponseDto>>.Success(response);
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
            var tagsResult = await GetAndValidateTagsAsync(request.TagIds, cancellationToken);
            if (tagsResult.IsFailure)
            {
                return Result<NoteResponseDto>.Failure(tagsResult.Error!.Value);
            }

            var note = request.Adapt<Note>();
            note.Id = Guid.NewGuid();
            note.UserId = userId;
            note.CreationDate = DateTime.UtcNow;
            note.Tags = tagsResult.Value!;

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            return Result<NoteResponseDto>.Success(createdNote.Adapt<NoteResponseDto>());
        }

        public async Task<Result<NoteResponseDto>> UpdateAsync(Guid userId, Guid noteId, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var existingNote = await _noteRepository.GetByIdAsync(userId, noteId, cancellationToken);
            if (existingNote is null)
            {
                return Result<NoteResponseDto>.Failure(ErrorCode.NotFound);
            }

            var tagsResult = await GetAndValidateTagsAsync(request.TagIds, cancellationToken);
            if (tagsResult.IsFailure)
            {
                return Result<NoteResponseDto>.Failure(tagsResult.Error!.Value);
            }

            var note = new Note
            {
                Id = noteId,
                Name = request.Name,
                Description = request.Description,
                Tags = tagsResult.Value!
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

        private async Task<Result<List<Tag>>> GetAndValidateTagsAsync(IEnumerable<Guid>? tagIds, CancellationToken cancellationToken)
        {
            if (tagIds is not { } ids || !ids.Any())
            {
                return Result<List<Tag>>.Success(new List<Tag>());
            }
            
            var distinctTagIds = ids.Distinct().ToList();
            var tags = await _tagRepository.GetByIdsAsync(Guid.Empty, distinctTagIds, cancellationToken);

            if (tags is not { Count: var count } || count != distinctTagIds.Count)
            {
                return Result<List<Tag>>.Failure(ErrorCode.TagNotFound);
            }

            return Result<List<Tag>>.Success(tags);
        }
    }
}
