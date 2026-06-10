using WatApi.DTO.Note;
using WatApi.Models;
namespace WatApi.Services.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync(Guid userId);
        Task<Note> GetNoteByIdAsync(Guid noteId, Guid userId);
        Task<Note> CreateNoteAsync(NoteCreateDto dto, Guid userId);
        Task<Note> UpdateNoteAsync(NoteUpdateDto dto, Guid noteId, Guid userId);
        Task DeleteNoteAsync(Guid noteId, Guid userId);
    }
}
