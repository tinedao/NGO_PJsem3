using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NGO_PJsem3.Data;
using NGO_PJsem3.Models;

namespace NGO_PJsem3.Areas.Admin.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Produces("application/json")]
    [Route("administrator/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NgoDbContext _context;

        public UsersController(NgoDbContext context)
        {
            _context = context;
        }

        // GET: https://localhost:7296/administrator/Users/GetUsers
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: https://localhost:7296/administrator/Users/GetById/{id}
        [HttpGet("{id}")]
        [Route("GetById/{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: https://localhost:7296/administrator/Users/UpdateUsers/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Route("UpdateUsers/{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.UserId)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: administrator/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("PostUser")]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'NgoDbContext.Users'  is null.");
          }
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.UserId }, users);
        }

        // DELETE: administrator/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
