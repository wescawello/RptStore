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
    public class ReturnOrdersController : ControllerBase
    {
        private readonly SdContext _context;

        public ReturnOrdersController(SdContext context)
        {
            _context = context;
        }

        // GET: api/ReturnOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnOrder>>> GetReturnOrders()
        {
            return await _context.ReturnOrders.ToListAsync();
        }

        // GET: api/ReturnOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnOrder>> GetReturnOrder(Guid id)
        {
            var returnOrder = await _context.ReturnOrders.FindAsync(id);

            if (returnOrder == null)
            {
                return NotFound();
            }

            return returnOrder;
        }

        // PUT: api/ReturnOrders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReturnOrder(Guid id, ReturnOrder returnOrder)
        {
            if (id != returnOrder.ReturnOrderId)
            {
                return BadRequest();
            }

            _context.Entry(returnOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReturnOrderExists(id))
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

        // POST: api/ReturnOrders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ReturnOrder>> PostReturnOrder(ReturnOrder returnOrder)
        {
            var cp = returnOrder.ReturnOrderSubs.ToList();

            returnOrder.ReturnOrderSubs = await _context.PickOrderSubs.Include(s => s.Barcode).Where(p => cp.Select(o => o.PickOrderSubId).Contains(p.PickOrderSubId)).ToListAsync();

            returnOrder.ReturnOrderSubs.ForEach(r =>
            {
                var m = cp.FirstOrDefault(o => o.PickOrderSubId == r.PickOrderSubId);
                if (m != null)
                {
                    r.Returned = true;
                    r.ReturnPrice = m.ReturnPrice;
                    r.ReturnReason = m.ReturnReason;

                    r.Barcode.ReturnCount++;
                    r.Barcode.Saled = false;
                    r.Barcode.Picked = false;
                    r.Barcode.StockUnitId = 99;
                }
            });
            _context.ReturnOrders.Add(returnOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReturnOrder", new { id = returnOrder.ReturnOrderId }, returnOrder);
        }

        // DELETE: api/ReturnOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReturnOrder>> DeleteReturnOrder(Guid id)
        {
            var returnOrder = await _context.ReturnOrders.FindAsync(id);
            if (returnOrder == null)
            {
                return NotFound();
            }

            _context.ReturnOrders.Remove(returnOrder);
            await _context.SaveChangesAsync();

            return returnOrder;
        }

        private bool ReturnOrderExists(Guid id)
        {
            return _context.ReturnOrders.Any(e => e.ReturnOrderId == id);
        }
    }
}
