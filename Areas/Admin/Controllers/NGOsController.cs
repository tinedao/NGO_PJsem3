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
    public class NGOsController : ControllerBase
    {
        private readonly NgoDbContext _context;

        public NGOsController(NgoDbContext context)
        {
            _context = context;
        }

        // GET: administrator/NGOs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NGOs>>> GetNGOs()
        {
          if (_context.NGOs == null)
          {
              return NotFound();
          }
            return await _context.NGOs.ToListAsync();
        }

        // GET: administrator/NGOs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NGOs>> GetNGOs(int id)
        {
          if (_context.NGOs == null)
          {
              return NotFound();
          }
            var nGOs = await _context.NGOs.FindAsync(id);

            if (nGOs == null)
            {
                return NotFound();
            }

            return nGOs;
        }

        // PUT: administrator/NGOs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNGOs(int id, NGOs nGOs)
        {
            if (id != nGOs.NgoId)
            {
                return BadRequest();
            }

            _context.Entry(nGOs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NGOsExists(id))
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

        // POST: administrator/NGOs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NGOs>> PostNGOs(NGOs nGOs)
        {
          if (_context.NGOs == null)
          {
              return Problem("Entity set 'NgoDbContext.NGOs'  is null.");
          }
            _context.NGOs.Add(nGOs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNGOs", new { id = nGOs.NgoId }, nGOs);
        }

        // DELETE: administrator/NGOs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNGOs(int id)
        {
            if (_context.NGOs == null)
            {
                return NotFound();
            }
            var nGOs = await _context.NGOs.FindAsync(id);
            if (nGOs == null)
            {
                return NotFound();
            }

            _context.NGOs.Remove(nGOs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NGOsExists(int id)
        {
            return (_context.NGOs?.Any(e => e.NgoId == id)).GetValueOrDefault();
        }
    }
}
