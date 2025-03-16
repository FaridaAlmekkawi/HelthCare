using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelthCareProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public ChatController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Chat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<chat>>> GetChats()
        {
            return await _context.chats.Include(c => c.Users).Include(c => c.messages).ToListAsync();
        }

        // GET: api/Chat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<chat>> GetChat(int id)
        {
            var chat = await _context.chats
                .Include(c => c.Users)
                .Include(c => c.messages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }

        // POST: api/Chat
        [HttpPost]
        public async Task<ActionResult<chat>> CreateChat(chat chat)
        {
            _context.chats.Add(chat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
        }

        // DELETE: api/Chat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            var chat = await _context.chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            _context.chats.Remove(chat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Chat/5/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<message>>> GetChatMessages(int id)
        {
            var chat = await _context.chats.Include(c => c.messages).FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return chat.messages.ToList();
        }

        // POST: api/Chat/5/messages
        [HttpPost("{id}/messages")]
        public async Task<ActionResult<message>> AddMessageToChat(int id, message message)
        {
            var chat = await _context.chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            message.ChatId = id;
            _context.messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChatMessages), new { id = id }, message);
        }
    }
}
