using NoteVault.BLL.DTOs.Tags;

namespace NoteVault.BLL.DTOs.Notes
{
    public class NoteResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public DateTime CreationDate { get; set; }
        public List<TagDto> Tags { get; set; } = new();
    }
}
