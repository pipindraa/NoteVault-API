using System.ComponentModel.DataAnnotations;

namespace NoteVault.BLL.DTOs.Notes
{
    public class NoteUpdateDto
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public string Description { get; init; } = string.Empty;

        public IReadOnlyCollection<string> ImageUrls { get; init; } = Array.Empty<string>();
    }
}
