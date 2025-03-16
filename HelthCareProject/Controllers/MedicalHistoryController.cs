using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelthCareProject.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryRecordsController : ControllerBase
    {
        private readonly helthcare_systemContext _context;

        public MedicalHistoryRecordsController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/MedicalHistoryRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<medicalhistoryrecord>>> GetMedicalHistoryRecords()
        {
            return await _context.medicalhistoryrecords
                .Include(m => m.Patient)
                .ToListAsync();
        }

        // GET: api/MedicalHistoryRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<medicalhistoryrecord>> GetMedicalHistoryRecord(int id)
        {
            var medicalHistoryRecord = await _context.medicalhistoryrecords
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalHistoryRecord == null)
            {
                return NotFound();
            }

            return medicalHistoryRecord;
        }

        // GET: api/MedicalHistoryRecords/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<medicalhistoryrecord>>> GetMedicalHistoryRecordsByPatient(int patientId)
        {
            return await _context.medicalhistoryrecords
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
        }

        // POST: api/MedicalHistoryRecords
        [HttpPost]
        public async Task<ActionResult<medicalhistoryrecord>> PostMedicalHistoryRecord([FromForm] IFormFile file, [FromForm] int patientId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }

            // Verify patient exists
            var patient = await _context.patients.FindAsync(patientId);
            if (patient == null)
            {
                return BadRequest("Patient not found");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var medicalHistoryRecord = new medicalhistoryrecord
            {
                FileName = file.FileName,
                FileData = memoryStream.ToArray(),
                UploadDate = DateTime.Now,
                PatientId = patientId
            };

            _context.medicalhistoryrecords.Add(medicalHistoryRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalHistoryRecord), new { id = medicalHistoryRecord.Id }, medicalHistoryRecord);
        }

        // PUT: api/MedicalHistoryRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalHistoryRecord(int id, [FromForm] IFormFile file, [FromForm] int patientId)
        {
            var medicalHistoryRecord = await _context.medicalhistoryrecords.FindAsync(id);
            if (medicalHistoryRecord == null)
            {
                return NotFound();
            }

            // Verify patient exists
            var patient = await _context.patients.FindAsync(patientId);
            if (patient == null)
            {
                return BadRequest("Patient not found");
            }

            if (file != null && file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                medicalHistoryRecord.FileName = file.FileName;
                medicalHistoryRecord.FileData = memoryStream.ToArray();
            }

            medicalHistoryRecord.PatientId = patientId;
            medicalHistoryRecord.UploadDate = DateTime.Now;

            _context.Entry(medicalHistoryRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalHistoryRecordExists(id))
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

        // DELETE: api/MedicalHistoryRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalHistoryRecord(int id)
        {
            var medicalHistoryRecord = await _context.medicalhistoryrecords.FindAsync(id);
            if (medicalHistoryRecord == null)
            {
                return NotFound();
            }

            _context.medicalhistoryrecords.Remove(medicalHistoryRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/MedicalHistoryRecords/download/5
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var record = await _context.medicalhistoryrecords.FindAsync(id);

            if (record == null || record.FileData == null)
            {
                return NotFound();
            }

            return File(record.FileData, "application/octet-stream", record.FileName);
        }

        private bool MedicalHistoryRecordExists(int id)
        {
            return _context.medicalhistoryrecords.Any(e => e.Id == id);
        }
    }
}