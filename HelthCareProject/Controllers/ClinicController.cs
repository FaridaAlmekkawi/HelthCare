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
    public class ClinicsController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public ClinicsController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Clinics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<clinic>>> GetClinics()
        {
            return await _context.clinics.Include(c => c.ClinicAddress).Include(c => c.Doctor).Include(c => c.appointmentslots).Include(c => c.Governorate).ToListAsync();
        }

        // GET: api/Clinics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<clinic>> GetClinic(int id)
        {
            var clinic = await _context.clinics
                .Include(c => c.ClinicAddress)
                .Include(c => c.Doctor)
                .Include(c => c.appointmentslots)
                .Include(c => c.Governorate)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clinic == null)
            {
                return NotFound();
            }

            return clinic;
        }

        // POST: api/Clinics
        [HttpPost]
        public async Task<ActionResult<clinic>> CreateClinic(clinic clinic)
        {
            var doctorExists = await _context.doctors.AnyAsync(d => d.Id == clinic.DoctorId);
            if (!doctorExists)
            {
                return BadRequest("Doctor not found");
            }

            _context.clinics.Add(clinic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClinic), new { id = clinic.Id }, clinic);
        }

        // PUT: api/Clinics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinic(int id, clinic clinic)
        {
            if (id != clinic.Id)
            {
                return BadRequest("Clinic ID mismatch");
            }

            _context.Entry(clinic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(id))
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

        // DELETE: api/Clinics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinic(int id)
        {
            var clinic = await _context.clinics.FindAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }

            _context.clinics.Remove(clinic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Clinics/5/address
        [HttpGet("{id}/address")]
        public async Task<ActionResult<clinicaddress>> GetClinicAddress(int id)
        {
            var clinic = await _context.clinics.Include(c => c.ClinicAddress).FirstOrDefaultAsync(c => c.Id == id);
            if (clinic == null || clinic.ClinicAddress == null)
            {
                return NotFound();
            }

            return clinic.ClinicAddress;
        }

        // GET: api/Clinics/5/appointmentslots
        [HttpGet("{id}/appointmentslots")]
        public async Task<ActionResult<IEnumerable<appointmentslot>>> GetClinicAppointmentSlots(int id)
        {
            if (!ClinicExists(id))
            {
                return NotFound();
            }

            var slots = await _context.appointmentslots
                .Where(s => s.ClinicId == id)
                .ToListAsync();

            return slots;
        }

        // GET: api/Clinics/5/governorate
        [HttpGet("{id}/governorate")]
        public async Task<ActionResult<governorate>> GetClinicGovernorate(int id)
        {
            var clinic = await _context.clinics.Include(c => c.Governorate).FirstOrDefaultAsync(c => c.Id == id);
            if (clinic == null || clinic.Governorate == null)
            {
                return NotFound();
            }

            return clinic.Governorate;
        }

        // GET: api/Clinics/5/assistants
        [HttpGet("{id}/assistants")]
        public async Task<ActionResult<IEnumerable<assistant>>> GetClinicAssistants(int id)
        {
            var clinic = await _context.clinics.Include(c => c.Doctor).FirstOrDefaultAsync(c => c.Id == id);
            if (clinic == null || clinic.Doctor == null)
            {
                return NotFound();
            }

            var assistants = await _context.assistants
                .Where(a => a.DoctorId == clinic.Doctor.Id)
                .ToListAsync();

            return assistants;
        }

        private bool ClinicExists(int id)
        {
            return _context.clinics.Any(e => e.Id == id);
        }
    }
}
