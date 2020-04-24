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
    public class StockUnitsController : ControllerBase
    {
        private readonly SdContext _context;

        public StockUnitsController(SdContext context)
        {
            _context = context;
        }

        // GET: api/StockUnits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockUnit>>> GetStockUnits()
        {
            return await _context.StockUnits.ToListAsync();
        }

        // GET: api/StockUnits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockUnit>> GetStockUnit(int id)
        {
            var stockUnit = await _context.StockUnits.FindAsync(id);

            if (stockUnit == null)
            {
                return NotFound();
            }

            return stockUnit;
        }

        // PUT: api/StockUnits/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockUnit(int id, StockUnit stockUnit)
        {
            if (id != stockUnit.StockUnitId)
            {
                return BadRequest();
            }

            _context.Entry(stockUnit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockUnitExists(id))
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

        // POST: api/StockUnits
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StockUnit>> PostStockUnit(StockUnit stockUnit)
        {
            _context.StockUnits.Add(stockUnit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStockUnit", new { id = stockUnit.StockUnitId }, stockUnit);
        }

        // DELETE: api/StockUnits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StockUnit>> DeleteStockUnit(int id)
        {
            var stockUnit = await _context.StockUnits.FindAsync(id);
            if (stockUnit == null)
            {
                return NotFound();
            }

            _context.StockUnits.Remove(stockUnit);
            await _context.SaveChangesAsync();

            return stockUnit;
        }

        private bool StockUnitExists(int id)
        {
            return _context.StockUnits.Any(e => e.StockUnitId == id);
        }
    }
}
