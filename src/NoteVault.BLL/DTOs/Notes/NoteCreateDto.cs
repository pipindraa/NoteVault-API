using System.ComponentModel.DataAnnotations;

namespace NoteVault.BLL.DTOs.Notes
{
    public class NoteCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public List<string> ImageUrls { get; set; } = new();
    }
}
