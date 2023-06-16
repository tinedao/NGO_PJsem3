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
    public class DonationsController : ControllerBase
    {
        private readonly NgoDbContext _context;

        public DonationsController(NgoDbContext context)
        {
            _context = context;
        }

        // GET: administrator/Donations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donations>>> GetDonations()
        {
          if (_context.Donations == null)
          {
              return NotFound();
          }
            return await _context.Donations.ToListAsync();
        }

        // GET: administrator/Donations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donations>> GetDonations(int id)
        {
          if (_context.Donations == null)
          {
              return NotFound();
          }
            var donations = await _context.Donations.FindAsync(id);

            if (donations == null)
            {
                return NotFound();
            }

            return donations;
        }

        // PUT: administrator/Donations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonations(int id, Donations donations)
        {
            if (id != donations.DonationId)
            {
                return BadRequest();
            }

            _context.Entry(donations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationsExists(id))
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

        // POST: administrator/Donations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donations>> PostDonations(Donations donations)
        {
          if (_context.Donations == null)
          {
              return Problem("Entity set 'NgoDbContext.Donations'  is null.");
          }
            _context.Donations.Add(donations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonations", new { id = donations.DonationId }, donations);
        }

        // DELETE: administrator/Donations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonations(int id)
        {
            if (_context.Donations == null)
            {
                return NotFound();
            }
            var donations = await _context.Donations.FindAsync(id);
            if (donations == null)
            {
                return NotFound();
            }

            _context.Donations.Remove(donations);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonationsExists(int id)
        {
            return (_context.Donations?.Any(e => e.DonationId == id)).GetValueOrDefault();
        }
    }
}
