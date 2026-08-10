using Microsoft.EntityFrameworkCore;
using NoteVault.DAL.Entities;

namespace NoteVault.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
    }
}
