using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelthCareProject.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace HelthCareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly helthcare_systemContext _context;

        public AdminController(helthcare_systemContext context)
        {
            _context = context;
        }

        // GET: api/Admin/Users
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.users.Include(u => u.Roles).ToListAsync();
            return Ok(users);
        }

        // GET: api/Admin/User/{id}
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _context.users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/Admin/UpdateUser/{id}
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] user user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        

        // GET: api/Admin/UserClaims/{userId}
        [HttpGet("UserClaims/{userId}")]
        public async Task<IActionResult> GetUserClaims(string userId)
        {
            var claims = await _context.userclaims.Where(uc => uc.UserId == userId).ToListAsync();
            return Ok(claims);
        }

        // POST: api/Admin/CreateUser
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        // DELETE: api/Admin/DeleteUser/{id}
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Admin/Roles
        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.roles.Include(r => r.Users).ToListAsync();
            return Ok(roles);
        }

        // POST: api/Admin/CreateRole
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.roles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, role);
        }

        // DELETE: api/Admin/DeleteRole/{id}
        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _context.roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.roles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
