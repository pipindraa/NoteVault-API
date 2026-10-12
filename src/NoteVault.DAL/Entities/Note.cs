namespace NoteVault.DAL.Entities
{
    public class Note : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Tag> Tags { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new();
        public DateTime CreationDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
