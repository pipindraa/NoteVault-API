namespace NoteVault.BLL.DTOs.Notes
{
    public class NoteCreateDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public IReadOnlyCollection<string> ImageUrls { get; init; } = Array.Empty<string>();
        public IReadOnlyCollection<Guid> TagIds { get; init; } = Array.Empty<Guid>();
    }
}
