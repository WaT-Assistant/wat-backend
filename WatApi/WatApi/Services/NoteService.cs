using WatApi.Data;
using WatApi.DTO.Note;
using WatApi.Models;
using WatApi.Services.Interfaces;

namespace WatApi.Services
{
    public class NoteService(AppDbContext context) : INoteService
    {
        private readonly AppDbContext _context = context;

        public async Task<Note> CreateNoteAsync(NoteCreateDto dto, Guid userId)
        {
            var user = await _context.Users.FindAsync(userId) 
                ?? throw new KeyNotFoundException("User not found");

            var note = new Note {
                Id = Guid.NewGuid(),
                UserId = userId,
                Text = dto.Text,
                ColorHex = dto.ColorHex,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task DeleteNoteAsync(Guid noteId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Note> GetNoteByIdAsync(Guid noteId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Note> UpdateNoteAsync(NoteUpdateDto dto, Guid noteId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
