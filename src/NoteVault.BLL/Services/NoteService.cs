using Mapster;
using Microsoft.Extensions.Logging;
using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.DTOs.Pagination;
using NoteVault.BLL.DTOs.Tags;
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

        public async Task<Result<PagedResponse<NoteResponseDto>>> GetPageAsync(PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var (notes, totalCount) = await _noteRepository.GetPageAsync(
                note => note.CreationDate,
                request.PageNumber,
                request.PageSize,
                descending: true, 
                cancellationToken);

            var dtos = notes.Adapt<IReadOnlyCollection<NoteResponseDto>>();

            var response = new PagedResponse<NoteResponseDto>(dtos, request.PageNumber, request.PageSize, totalCount);
            return Result<PagedResponse<NoteResponseDto>>.Success(response);
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
            var tagsResult = await GetAndValidateTagsAsync(request.TagIds, cancellationToken);
            if (tagsResult.IsFailure)
            {
                return Result<NoteResponseDto>.Failure(tagsResult.Error!.Value);
            }

            var note = request.Adapt<Note>();
            note.Id = Guid.NewGuid();
            note.CreationDate = DateTime.UtcNow;
            note.Tags = tagsResult.Value!;

            var createdNote = await _noteRepository.AddAsync(note, cancellationToken);
            return Result<NoteResponseDto>.Success(createdNote.Adapt<NoteResponseDto>());
        }

        public async Task<Result<NoteResponseDto>> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default)
        {
            var tagsResult = await GetAndValidateTagsAsync(request.TagIds, cancellationToken);
            if (tagsResult.IsFailure)
            {
                return Result<NoteResponseDto>.Failure(tagsResult.Error!.Value);
            }

            var note = new Note
            {
                Id = id,
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

        private async Task<Result<List<Tag>>> GetAndValidateTagsAsync(IEnumerable<Guid>? tagIds, CancellationToken cancellationToken)
        {
            if (tagIds is not { } ids || !ids.Any())
            {
                return Result<List<Tag>>.Success(new List<Tag>());
            }
            
            var distinctTagIds = ids.Distinct().ToList();
            var tags = await _tagRepository.GetByIdsAsync(distinctTagIds, cancellationToken);

            if (tags is not { Count: var count } || count != distinctTagIds.Count)
            {
                return Result<List<Tag>>.Failure(ErrorCode.TagNotFound);
            }

            return Result<List<Tag>>.Success(tags);
        }
    }
}
