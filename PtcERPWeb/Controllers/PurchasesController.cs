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
    public class PurchasesController : ControllerBase
    {
        private readonly SdContext _context;

        public PurchasesController(SdContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
            return await _context.Purchases.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(Guid id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(Guid id, Purchase purchase)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }
            //purchase.Barcodes.ForEach(b =>
            //{
            //    //var bb = _context.Entry(b);
            //    //bb.State = EntityState.Modified;
            //    b.BarcodeValue = b.BarcodeValue;
            //    b.UpDateId = "gggg";
            //    //_context.Update(b);
            //});
            //_context.Entry(purchase).State = EntityState.Modified;


            _context.Update(purchase);

            try
            {
                 await _context.SaveChangesAsync();
 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Purchase>> DeletePurchase(Guid id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        private bool PurchaseExists(Guid id)
        {
            return _context.Purchases.Any(e => e.PurchaseId == id);
        }
    }
}
