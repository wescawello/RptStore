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
    public class FormatesController : ControllerBase
    {
        private readonly SdContext _context;

        public FormatesController(SdContext context)
        {
            _context = context;
        }

        // GET: api/Formates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formate>>> GetFormates()
        {
            return await _context.Formates.ToListAsync();
        }

        // GET: api/Formates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Formate>> GetFormate(int id)
        {
            var formate = await _context.Formates.FindAsync(id);

            if (formate == null)
            {
                return NotFound();
            }

            return formate;
        }

        // PUT: api/Formates/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormate(int id, Formate formate)
        {
            if (id != formate.FormateId)
            {
                return BadRequest();
            }

            _context.Entry(formate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormateExists(id))
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

        // POST: api/Formates
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Formate>> PostFormate(Formate formate)
        {
            _context.Formates.Add(formate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFormate", new { id = formate.FormateId }, formate);
        }

        // DELETE: api/Formates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Formate>> DeleteFormate(int id)
        {
            var formate = await _context.Formates.FindAsync(id);
            if (formate == null)
            {
                return NotFound();
            }

            _context.Formates.Remove(formate);
            await _context.SaveChangesAsync();

            return formate;
        }

        private bool FormateExists(int id)
        {
            return _context.Formates.Any(e => e.FormateId == id);
        }
    }
}
