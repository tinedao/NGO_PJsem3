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
    public class QuerysController : ControllerBase
    {
        private readonly NgoDbContext _context;

        public QuerysController(NgoDbContext context)
        {
            _context = context;
        }

        // GET: administrator/Querys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Querys>>> GetQuerys()
        {
          if (_context.Querys == null)
          {
              return NotFound();
          }
            return await _context.Querys.ToListAsync();
        }

        // GET: administrator/Querys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Querys>> GetQuerys(int id)
        {
          if (_context.Querys == null)
          {
              return NotFound();
          }
            var querys = await _context.Querys.FindAsync(id);

            if (querys == null)
            {
                return NotFound();
            }

            return querys;
        }

        // PUT: administrator/Querys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuerys(int id, Querys querys)
        {
            if (id != querys.QueryId)
            {
                return BadRequest();
            }

            _context.Entry(querys).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuerysExists(id))
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

        // POST: administrator/Querys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Querys>> PostQuerys(Querys querys)
        {
          if (_context.Querys == null)
          {
              return Problem("Entity set 'NgoDbContext.Querys'  is null.");
          }
            _context.Querys.Add(querys);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuerys", new { id = querys.QueryId }, querys);
        }

        // DELETE: administrator/Querys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuerys(int id)
        {
            if (_context.Querys == null)
            {
                return NotFound();
            }
            var querys = await _context.Querys.FindAsync(id);
            if (querys == null)
            {
                return NotFound();
            }

            _context.Querys.Remove(querys);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuerysExists(int id)
        {
            return (_context.Querys?.Any(e => e.QueryId == id)).GetValueOrDefault();
        }
    }
}
