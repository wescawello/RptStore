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
    public class CustomerInfosController : ControllerBase
    {
        private readonly SdContext _context;

        public CustomerInfosController(SdContext context)
        {
            _context = context;
        }

        // GET: api/CustomerInfos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetCustomerInfos()
        {
            return await _context.CustomerInfos.ToListAsync();
        }

        // GET: api/CustomerInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerInfo>> GetCustomerInfo(int id)
        {
            var customerInfo = await _context.CustomerInfos.FindAsync(id);

            if (customerInfo == null)
            {
                return NotFound();
            }

            return customerInfo;
        }

        // PUT: api/CustomerInfos/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerInfo(int id, CustomerInfo customerInfo)
        {
            if (id != customerInfo.CustomerInfoId)
            {
                return BadRequest();
            }

            _context.Entry(customerInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerInfoExists(id))
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

        // POST: api/CustomerInfos
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CustomerInfo>> PostCustomerInfo(CustomerInfo customerInfo)
        {
            _context.CustomerInfos.Add(customerInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerInfo", new { id = customerInfo.CustomerInfoId }, customerInfo);
        }

        // DELETE: api/CustomerInfos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerInfo>> DeleteCustomerInfo(int id)
        {
            var customerInfo = await _context.CustomerInfos.FindAsync(id);
            if (customerInfo == null)
            {
                return NotFound();
            }

            _context.CustomerInfos.Remove(customerInfo);
            await _context.SaveChangesAsync();

            return customerInfo;
        }

        private bool CustomerInfoExists(int id)
        {
            return _context.CustomerInfos.Any(e => e.CustomerInfoId == id);
        }
    }
}
