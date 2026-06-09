namespace NoteVault.DAL.Entities
{
    public class Tag : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
