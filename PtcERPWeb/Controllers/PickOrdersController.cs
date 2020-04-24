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
    public class PickOrdersController : ControllerBase
    {
        private readonly SdContext _context;

        public PickOrdersController(SdContext context)
        {
            _context = context;
        }

        // GET: api/PickOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PickOrder>>> GetPickOrders()
        {
            return await _context.PickOrders.ToListAsync();
        }

        // GET: api/PickOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PickOrder>> GetPickOrder(Guid id)
        {
            var pickOrder = await _context.PickOrders.FindAsync(id);

            if (pickOrder == null)
            {
                return NotFound();
            }

            return pickOrder;
        }

        // PUT: api/PickOrders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPickOrder(Guid id, PickOrder pickOrder)
        {
            if (id != pickOrder.PickOrderId)
            {
                return BadRequest();
            }

            _context.Entry(pickOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PickOrderExists(id))
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

        // POST: api/PickOrders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PickOrder>> PostPickOrder(PickOrder pickOrder)
        {
            _context.PickOrders.Add(pickOrder);

            _context.Barcodes.Where(o => pickOrder.PickOrderSubs.Select(p => p.BarcodeId).Contains(o.BarcodeId)).ToList().ForEach(o => o.Picked = true);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPickOrder", new { id = pickOrder.PickOrderId }, pickOrder);
        }

        // DELETE: api/PickOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PickOrder>> DeletePickOrder(Guid id)
        {
            var pickOrder = await _context.PickOrders.Include(o=>o.PickOrderSubs).SingleOrDefaultAsync(o=>o.PickOrderId==id);
            if (pickOrder == null)
            {
                return NotFound();
            }
            _context.Barcodes.Where(o => pickOrder.PickOrderSubs.Select(p => p.BarcodeId).Contains(o.BarcodeId)).ToList().ForEach(o => o.Picked = false);


            _context.PickOrders.Remove(pickOrder);
            await _context.SaveChangesAsync();

            return pickOrder;
        }

        private bool PickOrderExists(Guid id)
        {
            return _context.PickOrders.Any(e => e.PickOrderId == id);
        }
    }
}
