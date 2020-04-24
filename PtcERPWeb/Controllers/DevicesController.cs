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
    public class DevicesController : ControllerBase
    {
        private readonly SdContext _context;

        public DevicesController(SdContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // PUT: api/Devices/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device device)
        {
            if (id != device.DeviceId)
            {
                return BadRequest();
            }
            var vds = _context.VandorDevices.Where(o => o.DeviceId == id).ToList();
            var vdfs = _context.VandorDevices.Where(o =>  o.DeviceId == id && o.Formates.Count()>0).ToList();
            if(vdfs.Count(p => !device.VandorDevices.Select(q => q.VandorId).ToList().Contains(p.VandorId)) > 0)
            {
                return BadRequest("Formats need clear clean");
            }



            _context.VandorDevices.RemoveRange(vds.Where(p=> ! device.VandorDevices.Select(q=>q.VandorId).ToList().Contains(p.VandorId) ) );
            _context.VandorDevices.AddRange(device.VandorDevices.Where(p=> !vds.Select(q=>q.VandorId).ToList().Contains(p.VandorId) ) );




            _context.Entry(device).State = EntityState.Modified;
            

            //_context.VandorDevices.AddRange(device.VandorDevices);




            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {

            //var vd = device.VandorDevices.ToList();
            //device.VandorDevices.Clear();

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
           // var vdfix = vd.Select(o => new VandorDevice { Device = device , Vandor = _context.Find<Vandor>( o.VandorId) }).ToList();
           //_context.VandorDevices.AddRange(vdfix);
           // await _context.SaveChangesAsync();
           // device.VandorDevices = vdfix.ToList();


            return CreatedAtAction("GetDevice", new { id = device.DeviceId }, device);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Device>> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return device;
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }
    }
}
