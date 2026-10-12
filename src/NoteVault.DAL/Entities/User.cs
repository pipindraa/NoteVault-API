namespace NoteVault.DAL.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Note> Notes { get; set; } = new HashSet<Note>(); 
    }
}
