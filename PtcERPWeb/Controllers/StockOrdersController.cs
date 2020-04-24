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
    public class StockOrdersController : ControllerBase
    {
        private readonly SdContext _context;

        public StockOrdersController(SdContext context)
        {
            _context = context;
        }

        // GET: api/StockOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockOrder>>> GetStockOrders()
        {
            return await _context.StockOrders.ToListAsync();
        }

        // GET: api/StockOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockOrder>> GetStockOrder(Guid id)
        {
            var stockOrder = await _context.StockOrders.FindAsync(id);

            if (stockOrder == null)
            {
                return NotFound();
            }

            return stockOrder;
        }

        // PUT: api/StockOrders/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockOrder(Guid id, StockOrder stockOrder)
        {
            if (id != stockOrder.StockOrderId)
            {
                return BadRequest();
            }

            var vds = _context.StockOrderSubs.Where(o => o.StockOrderId == stockOrder.StockOrderId).ToList();
            _context.StockOrderSubs.RemoveRange(vds.Where(p => !stockOrder.StockOrderSubs.Select(q => q.StockOrderSubId).ToList().Contains(p.StockOrderSubId)));

            _context.Entry(stockOrder).State = EntityState.Modified;

            if (stockOrder.ProcessDate.HasValue)
            {
                var barcodes = await _context.Barcodes.Where(o => stockOrder.StockOrderSubs.Select(s => s.BarcodeId).Contains(o.BarcodeId)).ToListAsync();
                barcodes.ForEach(b => b.StockUnitId = stockOrder.StockUnitId);
            }


            //_context.Update(stockOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockOrderExists(id))
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

        // POST: api/StockOrders
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StockOrder>> PostStockOrder(StockOrder stockOrder)
        {
            if (stockOrder.ProcessDate.HasValue)
            {
                stockOrder.ProcessId = stockOrder.CreateId;
            }
            
            _context.StockOrders.Add(stockOrder);
            if (stockOrder.ProcessDate.HasValue)
            {
                var barcodes = await _context.Barcodes.Where(o => stockOrder.StockOrderSubs.Select(s => s.BarcodeId).Contains(o.BarcodeId)).ToListAsync();
                barcodes.ForEach(b => b.StockUnitId = stockOrder.StockUnitId);
            }


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStockOrder", new { id = stockOrder.StockOrderId }, stockOrder);
        }

        // DELETE: api/StockOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StockOrder>> DeleteStockOrder(Guid id)
        {
            var stockOrder = await _context.StockOrders.FindAsync(id);
            if (stockOrder == null)
            {
                return NotFound();
            }

            _context.StockOrders.Remove(stockOrder);
            await _context.SaveChangesAsync();

            return stockOrder;
        }

        private bool StockOrderExists(Guid id)
        {
            return _context.StockOrders.Any(e => e.StockOrderId == id);
        }
    }
}
