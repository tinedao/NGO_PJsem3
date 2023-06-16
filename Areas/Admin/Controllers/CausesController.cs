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
    public class CausesController : ControllerBase
    {
        private readonly NgoDbContext _context;

        public CausesController(NgoDbContext context)
        {
            _context = context;
        }

        // GET: https://localhost:7296/administrator/Causes/GetCause
        [HttpGet]
        [Route("GetCause")]
        public async Task<ActionResult<IEnumerable<Causes>>> GetCauses()
        {
          if (_context.Causes == null)
          {
              return NotFound();
          }
            return await _context.Causes.ToListAsync();
        }

        // GET: https://localhost:7296/administrator/Causes/GetCauseById/{id}
        [HttpGet("{id}")]
        [Route("GetCauseById/{id}")]
        public async Task<ActionResult<Causes>> GetCauses(int id)
        {
          if (_context.Causes == null)
          {
              return NotFound();
          }
            var causes = await _context.Causes.FindAsync(id);

            if (causes == null)
            {
                return NotFound();
            }

            return causes;
        }

        // PUT: https://localhost:7296/administrator/Causes/UpdateCause/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Route("UpdateCause/{id}")]
        public async Task<IActionResult> PutCauses(int id, Causes causes)
        {
            if (id != causes.CauseId)
            {
                return BadRequest();
            }

            _context.Entry(causes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CausesExists(id))
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

        // POST: https://localhost:7296/administrator/Causes/PostCauses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("PostCauses")]
        public async Task<ActionResult<Causes>> PostCauses(Causes causes)
        {
          if (_context.Causes == null)
          {
              return Problem("Entity set 'NgoDbContext.Causes'  is null.");
          }
            _context.Causes.Add(causes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCauses", new { id = causes.CauseId }, causes);
        }

        // DELETE: https://localhost:7296/administrator/Causes/DeleteCauses/{id}
        [HttpDelete("{id}")]
        [Route("DeleteCauses/{id}")]
        public async Task<IActionResult> DeleteCauses(int id)
        {
            if (_context.Causes == null)
            {
                return NotFound();
            }
            var causes = await _context.Causes.FindAsync(id);
            if (causes == null)
            {
                return NotFound();
            }

            _context.Causes.Remove(causes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CausesExists(int id)
        {
            return (_context.Causes?.Any(e => e.CauseId == id)).GetValueOrDefault();
        }
    }
}
