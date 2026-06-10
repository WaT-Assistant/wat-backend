using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatApi.DTO.Note;
using WatApi.Services.Interfaces;

namespace WatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController(INoteService noteService) : ControllerBase
    {
        private readonly INoteService _noteService = noteService;

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreateDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var note = await _noteService.CreateNoteAsync(dto, userId);

            var response = new NoteResponseDto
            {
                Id = note.Id,
                Text = note.Text,
                ColorHex = note.ColorHex,
                CreatedAt = note.CreatedAt
            };

            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetNoteByUserId(Guid Id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var note = await _noteService.GetNoteByIdAsync(Id, userId);

            var response = new NoteResponseDto
            {
                Id = note.Id,
                Text = note.Text,
                ColorHex = note.ColorHex,
                CreatedAt = note.CreatedAt
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var notes = await _noteService.GetAllNotesAsync(userId);

            var response = notes.Select(note => new NoteResponseDto
            {
                Id = note.Id,
                Text = note.Text,
                ColorHex = note.ColorHex,
                CreatedAt = note.CreatedAt
            });
            return Ok(response);
        }
    }
}
