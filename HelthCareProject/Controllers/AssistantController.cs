using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelthCareProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public AssistantController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Assistant
        [HttpGet]
        public async Task<IActionResult> GetAssistants()
        {
            var assistants = await _context.assistants.Include(a => a.User).Include(a => a.Doctor).ToListAsync();
            return Ok(assistants);
        }

        // GET: api/Assistant/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssistant(int id)
        {
            var assistant = await _context.assistants.Include(a => a.User).Include(a => a.Doctor)
                                                     .FirstOrDefaultAsync(a => a.Id == id);
            if (assistant == null)
            {
                return NotFound();
            }
            return Ok(assistant);
        }

        // POST: api/Assistant
        [HttpPost]
        public async Task<IActionResult> CreateAssistant([FromBody] assistant assistant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.assistants.Add(assistant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAssistant), new { id = assistant.Id }, assistant);
        }

        // PUT: api/Assistant/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssistant(int id, [FromBody] assistant assistant)
        {
            if (id != assistant.Id)
            {
                return BadRequest();
            }

            _context.Entry(assistant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.assistants.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Assistant/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssistant(int id)
        {
            var assistant = await _context.assistants.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }

            _context.assistants.Remove(assistant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
