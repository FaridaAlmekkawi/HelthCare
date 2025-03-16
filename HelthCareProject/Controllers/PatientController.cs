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
    public class PatientsController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public PatientsController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<patient>>> GetPatients()
        {
            return await _context.patients
                .Include(p => p.User)
                .ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<patient>> GetPatient(int id)
        {
            var patient = await _context.patients
                .Include(p => p.User)
                .Include(p => p.appointments)
                .Include(p => p.medicalhistoryrecords)
                .Include(p => p.messages)
                .Include(p => p.patientdoctors)
                .Include(p => p.prescriptions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<patient>> CreatePatient(patient patient)
        {
            var userExists = await _context.users.AnyAsync(u => u.Id == patient.UserId);
            if (!userExists)
            {
                return BadRequest("User not found");
            }

            _context.patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest("Patient ID mismatch");
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Patients/5/doctors
        [HttpGet("{id}/doctors")]
        public async Task<ActionResult<IEnumerable<doctor>>> GetPatientDoctors(int id)
        {
            if (!PatientExists(id))
            {
                return NotFound();
            }

            var doctors = await _context.patientdoctors
                .Where(pd => pd.PatientId == id)
                .Include(pd => pd.Doctor)
                .Select(pd => pd.Doctor)
                .ToListAsync();

            return doctors;
        }

        // POST: api/Patients/5/doctors/10
        [HttpPost("{patientId}/doctors/{doctorId}")]
        public async Task<IActionResult> AddDoctorToPatient(int patientId, int doctorId)
        {
            if (!PatientExists(patientId))
            {
                return NotFound("Patient not found");
            }

            var doctorExists = await _context.doctors.AnyAsync(d => d.Id == doctorId);
            if (!doctorExists)
            {
                return NotFound("Doctor not found");
            }

            var existingRelation = await _context.patientdoctors
                .FirstOrDefaultAsync(pd => pd.PatientId == patientId && pd.DoctorId == doctorId);

            if (existingRelation != null)
            {
                return Conflict("Doctor already assigned to this patient");
            }

            var patientDoctor = new patientdoctor { PatientId = patientId, DoctorId = doctorId };
            _context.patientdoctors.Add(patientDoctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Patients/5/doctors/10
        [HttpDelete("{patientId}/doctors/{doctorId}")]
        public async Task<IActionResult> RemoveDoctorFromPatient(int patientId, int doctorId)
        {
            var patientDoctor = await _context.patientdoctors
                .FirstOrDefaultAsync(pd => pd.PatientId == patientId && pd.DoctorId == doctorId);

            if (patientDoctor == null)
            {
                return NotFound("Relationship not found");
            }

            _context.patientdoctors.Remove(patientDoctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.patients.Any(e => e.Id == id);
        }
    }
}
