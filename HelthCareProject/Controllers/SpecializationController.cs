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
    public class SpecializationController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public SpecializationController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Specialization
        [HttpGet]
        public async Task<ActionResult<IEnumerable<specialization>>> GetSpecializations()
        {
            return await _context.specializations.ToListAsync();
        }

        // GET: api/Specialization/5
        [HttpGet("{id}")]
        public async Task<ActionResult<specialization>> GetSpecialization(int id)
        {
            var specialization = await _context.specializations
                .Include(s => s.doctors)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

        // POST: api/Specialization
        [HttpPost]
        public async Task<ActionResult<specialization>> CreateSpecialization(specialization specialization)
        {
            _context.specializations.Add(specialization);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpecialization), new { id = specialization.Id }, specialization);
        }

        // PUT: api/Specialization/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(int id, specialization specialization)
        {
            if (id != specialization.Id)
            {
                return BadRequest("Specialization ID mismatch");
            }

            _context.Entry(specialization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecializationExists(id))
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

        // DELETE: api/Specialization/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var specialization = await _context.specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            _context.specializations.Remove(specialization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Specialization/5/doctors
        [HttpGet("{id}/doctors")]
        public async Task<ActionResult<IEnumerable<doctor>>> GetDoctorsBySpecialization(int id)
        {
            if (!SpecializationExists(id))
            {
                return NotFound();
            }

            var doctors = await _context.doctors
                .Where(d => d.SpecializationId == id)
                .ToListAsync();

            return doctors;
        }

        private bool SpecializationExists(int id)
        {
            return _context.specializations.Any(e => e.Id == id);
        }
    }
}
