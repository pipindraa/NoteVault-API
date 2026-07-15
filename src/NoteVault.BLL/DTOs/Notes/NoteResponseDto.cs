using NoteVault.BLL.DTOs.Tags;

namespace NoteVault.BLL.DTOs.Notes
{
    public class NoteResponseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public IReadOnlyCollection<string> ImageUrls { get; init; } = Array.Empty<string>();
        public DateTime CreationDate { get; init; }
        public IReadOnlyCollection<TagDto> Tags { get; init; } = Array.Empty<TagDto>();
    }
}
