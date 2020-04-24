using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTCStore.Data;
using PTCStore.Models;

namespace PtcERPWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VandorsController : ControllerBase
    {
        private readonly SdContext _context;

        public VandorsController(SdContext context)
        {
            _context = context;
        }

        // GET: api/Vandors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vandor>>> GetVandors()
        {
            return await _context.Vandors.ToListAsync();
        }

        // GET: api/Vandors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vandor>> GetVandor(int id)
        {
            var vandor = await _context.Vandors.FindAsync(id);

            if (vandor == null)
            {
                return NotFound();
            }

            return vandor;
        }

        // PUT: api/Vandors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVandor(int id, Vandor vandor)
        {
            if (id != vandor.VandorId)
            {
                return BadRequest();
            }

            _context.Entry(vandor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VandorExists(id))
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

        // POST: api/Vandors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Vandor>> PostVandor(Vandor vandor)
        {
            _context.Vandors.Add(vandor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVandor", new { id = vandor.VandorId }, vandor);
        }

        // DELETE: api/Vandors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vandor>> DeleteVandor(int id)
        {
            var vandor = await _context.Vandors.FindAsync(id);
            if (vandor == null)
            {
                return NotFound();
            }

            _context.Vandors.Remove(vandor);
            await _context.SaveChangesAsync();

            return vandor;
        }

        private bool VandorExists(int id)
        {
            return _context.Vandors.Any(e => e.VandorId == id);
        }
    }
}
