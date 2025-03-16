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
    public class DoctorsController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public DoctorsController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<doctor>>> GetDoctors()
        {
            return await _context.doctors.Include(d => d.Specialization).Include(d => d.User).ToListAsync();
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<doctor>> GetDoctor(int id)
        {
            var doctor = await _context.doctors
                .Include(d => d.Specialization)
                .Include(d => d.User)
                .Include(d => d.appointments)
                .Include(d => d.clinics)
                .Include(d => d.messages)
                .Include(d => d.patientdoctors)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        // POST: api/Doctors
        [HttpPost]
        public async Task<ActionResult<doctor>> CreateDoctor(doctor doctor)
        {
            var userExists = await _context.users.AnyAsync(u => u.Id == doctor.UserId);
            if (!userExists)
            {
                return BadRequest("User not found");
            }

            _context.doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        // PUT: api/Doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest("Doctor ID mismatch");
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Doctors/5/patients
        [HttpGet("{id}/patients")]
        public async Task<ActionResult<IEnumerable<patient>>> GetDoctorPatients(int id)
        {
            if (!DoctorExists(id))
            {
                return NotFound();
            }

            var patients = await _context.patientdoctors
                .Where(pd => pd.DoctorId == id)
                .Include(pd => pd.Patient)
                .Select(pd => pd.Patient)
                .ToListAsync();

            return patients;
        }

        private bool DoctorExists(int id)
        {
            return _context.doctors.Any(e => e.Id == id);
        }

    }
}