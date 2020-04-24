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
    public class SaleOrdersController : ControllerBase
    {
        private readonly SdContext _context;

        public SaleOrdersController(SdContext context)
        {
            _context = context;
        }

        // GET: api/SaleOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleOrder>>> GetSaleOrders()
        {
            return await _context.SaleOrders.ToListAsync();
        }

        // GET: api/SaleOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleOrder>> GetSaleOrder(Guid id)
        {
            var saleOrder = await _context.SaleOrders.FindAsync(id);

            if (saleOrder == null)
            {
                return NotFound();
            }

            return saleOrder;
        }

        // PUT: api/SaleOrders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleOrder(Guid id, SaleOrder saleOrder)
        {
            if (id != saleOrder.SaleOrderId)
            {
                return BadRequest();
            }

            _context.Entry(saleOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleOrderExists(id))
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

        // POST: api/SaleOrders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaleOrder>> PostSaleOrder(SaleOrder saleOrder)
        {
            saleOrder.PickOrders = await _context.PickOrders.Include(p=>p.PickOrderSubs).ThenInclude(s=>s.Barcode ).Where(p => saleOrder.PickOrders.Select(o => o.PickOrderId).Contains(p.PickOrderId)).ToListAsync();

            saleOrder.PickOrders.ForEach(p =>
            {
                p.CustomerInfoId = saleOrder.Customer.CustomerInfoId;
                p.ProcessDate = DateTime.Now;
                p.ProcessId = saleOrder.CreateId;
                p.PickOrderSubs.Select(s => s.Barcode).ToList().ForEach(b => b.Saled = true);
            });


            _context.SaleOrders.Add(saleOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleOrder", new { id = saleOrder.SaleOrderId }, saleOrder);
        }

        // DELETE: api/SaleOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleOrder>> DeleteSaleOrder(Guid id)
        {
            var saleOrder = await _context.SaleOrders.FindAsync(id);
            if (saleOrder == null)
            {
                return NotFound();
            }

            _context.SaleOrders.Remove(saleOrder);
            await _context.SaveChangesAsync();

            return saleOrder;
        }

        private bool SaleOrderExists(Guid id)
        {
            return _context.SaleOrders.Any(e => e.SaleOrderId == id);
        }
    }
}
