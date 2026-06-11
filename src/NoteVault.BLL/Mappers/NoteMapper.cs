using NoteVault.BLL.DTOs.Notes;
using NoteVault.DAL.Entities;

namespace NoteVault.BLL.Mappers
{
    public static class NoteMapper
    {
        public static NoteResponseDto ToResponseDto(this Note note)
        {
            return new NoteResponseDto
            {
                Id = note.Id,
                Name = note.Name,
                Description = note.Description,
                ImageUrls = note.ImageUrls,
                CreationDate = note.CreationDate
            };
        }

        public static Note ToEntity(this NoteCreateDto dto)
        {
            return new Note
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                ImageUrls = dto.ImageUrls,
                CreationDate = DateTime.UtcNow
            };
        }

        public static Note ToEntity(this NoteUpdateDto dto, Guid id)
        {
            return new Note
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrls = dto.ImageUrls
            };
        }
    }
}
