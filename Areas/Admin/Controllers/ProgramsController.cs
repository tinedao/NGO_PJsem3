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
    public class ProgramsController : ControllerBase
    {
        private readonly NgoDbContext _context;

        public ProgramsController(NgoDbContext context)
        {
            _context = context;
        }

        // GET: administrator/Programs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Programs>>> GetPrograms()
        {
          if (_context.Programs == null)
          {
              return NotFound();
          }
            return await _context.Programs.ToListAsync();
        }

        // GET: administrator/Programs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Programs>> GetPrograms(int id)
        {
          if (_context.Programs == null)
          {
              return NotFound();
          }
            var programs = await _context.Programs.FindAsync(id);

            if (programs == null)
            {
                return NotFound();
            }

            return programs;
        }

        // PUT: administrator/Programs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrograms(int id, Programs programs)
        {
            if (id != programs.ProgramId)
            {
                return BadRequest();
            }

            _context.Entry(programs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramsExists(id))
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

        // POST: administrator/Programs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Programs>> PostPrograms(Programs programs)
        {
          if (_context.Programs == null)
          {
              return Problem("Entity set 'NgoDbContext.Programs'  is null.");
          }
            _context.Programs.Add(programs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrograms", new { id = programs.ProgramId }, programs);
        }

        // DELETE: administrator/Programs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrograms(int id)
        {
            if (_context.Programs == null)
            {
                return NotFound();
            }
            var programs = await _context.Programs.FindAsync(id);
            if (programs == null)
            {
                return NotFound();
            }

            _context.Programs.Remove(programs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProgramsExists(int id)
        {
            return (_context.Programs?.Any(e => e.ProgramId == id)).GetValueOrDefault();
        }
    }
}
