

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelthCareProject.Models;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly helthcare_systemContext _context;

        public HomeController(helthcare_systemContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new { message = "Welcome to HealthCare API!" });
        }

        [HttpGet("SearchDoctor")]
        public async Task<IActionResult> SearchDoctor(string query)
        {
            var doctors = await _context.doctors
                .Include(d => d.Specialization)
                .Where(d => d.UserId.Contains(query) || d.Specialization.Name.Contains(query))
                .ToListAsync();

            return Ok(doctors);
        }

        
        [HttpPost("AddPrescription")]
        public async Task<IActionResult> AddPrescription([FromBody] prescription request)
        {
            if (!_context.patients.Any(p => p.Id == request.PatientId) ||
                !_context.doctors.Any(d => d.Id == request.Id))
            {
                return BadRequest("Invalid Patient or Doctor ID");
            }

            _context.prescriptions.Add(request);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Prescription added successfully!" });
        }

       
        [HttpGet("MedicalHistory")]
        public async Task<IActionResult> GetMedicalHistory(int patientId)
        {
            var history = await _context.medicalhistoryrecords
                .Where(h => h.PatientId == patientId)
                .ToListAsync();

            return Ok(new { patientId, history });
        }
    }
}



