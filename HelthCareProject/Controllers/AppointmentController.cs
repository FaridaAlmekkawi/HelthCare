using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelthCareProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public AppointmentController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _context.appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Prescription)
                .ToListAsync();
            return Ok(appointments);
        }

        // GET: api/Appointment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _context.appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Prescription)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // PUT: api/Appointment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.appointments.Any(e => e.Id == id))
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

        // DELETE: api/Appointment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
