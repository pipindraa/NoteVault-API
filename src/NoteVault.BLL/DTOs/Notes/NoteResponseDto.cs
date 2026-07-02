using NoteVault.BLL.DTOs.Tags;

namespace NoteVault.BLL.DTOs.Notes
{
    public class NoteResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IReadOnlyCollection<string> ImageUrls { get; set; } = Array.Empty<string>();
        public DateTime CreationDate { get; set; }
        public IReadOnlyCollection<TagDto> Tags { get; set; } = Array.Empty<TagDto>();
    }
}
