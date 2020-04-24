using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
 
using Dapper;
 
using PTCStore.Data;
using PTCStore.QueryModels;
using Microsoft.AspNetCore.Identity;
using PTCStore.Models;
using PTCStore.RanderModels;

namespace PtcERPWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class QueryController : ControllerBase
    {
 
        private readonly SdContext _context;
        private readonly UserManager<Models.ApplicationUser> _userManager;
        public QueryController(  SdContext context, UserManager<Models.ApplicationUser> userManager)
        {
            //_contextx = contextx;
            _context = context;
            _userManager = userManager;
        }

        
 


        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<PTCStore.RanderModels.RPurchase>> Purchases(PTCStore.QueryModels.QPurchase query, int page, int pageSize = 10)
        {
            var rs = _context.Purchases
                //.Include(o => o.Barcodes)
                //.Include(o => o.Formate).ThenInclude(o => o.VandorDevice).ThenInclude(o => o.Device)
                //.Include(o => o.Formate).ThenInclude(o => o.VandorDevice).ThenInclude(o => o.Vandor)
                .Where(query.GetPredicate()).Select(o => new RPurchase
                {
                    ApplyDate = o.ApplyDate,
                    ApplyNumber = o.ApplyNumber,
                    Remark=o.Remark,
                    Barcodes = o.Barcodes,
                    DeviceId = o.DeviceId,
                    VandorId = o.VandorId,
                    FormateId = o.FormateId,
                    Formate = o.Formate,
                    FormateName = o.Formate.Name,
                    VandorName = o.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Formate.VandorDevice.Device.Name,
                    PurchaseDate = o.PurchaseDate,
                    PurchaseId = o.PurchaseId,
                    CreateDate = o.CreateDate,
                    CreateId = o.CreateId,
                    PurchasePrice = o.PurchasePrice,
                    Quantity = o.Quantity,
                    SalePrice = o.SalePrice,
                    UpDateId = o.UpDateId,
                    UpDateDate = o.UpDateDate,
                    TotalPrice=o.Quantity*o.PurchasePrice,
                     

                });;
            

            //var gg =   rs.ToList<PTCStore.RanderModels.RPurchase>();

            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<PTCStore.Models.Vandor>> Vandors(PTCStore.QueryModels.QVandor query, int page, int pageSize = 10)
        {
            var rs = _context.Vandors.Where(query.GetPredicate());//.Select(o=>new RPurchase());
            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<PTCStore.Models.Device>> Devices(PTCStore.QueryModels.QDevice query, int page, int pageSize = 10)
        {
            var rs = _context.Devices.Include(o=>o.VandorDevices).ThenInclude(q=> q.Vandor).Where(query.GetPredicate());//.Select(o=>new RPurchase());
            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<PTCStore.Models.Formate>> Formates(PTCStore.QueryModels.QFormate query, int page, int pageSize = 10)
        {
            var rs = _context.Formates.Where(query.GetPredicate());//.Select(o=>new RPurchase());
            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }

 
         

        [HttpGet("{s}")]
        public async Task<IActionResult> UserIdName(string s)
        {
            var u = await _userManager.FindByIdAsync(s);
            return Ok(u.UserName);
           // return Ok(master);
        }
    }

     

    public class NameId
    {
        public int Id { get;   set; }
        public string Name { get;   set; }

        public IEnumerable<NameId> Items { get;   set; }
    }
    public class XNameId: NameId
    {
        public int EmsId { get;   set; }
        
    }
    public class FNameId
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        public string VandorName { get; set; }
        public string DeviceName { get; set; }
        public int Count { get; set; }
        //public IEnumerable<string> Barcodes { get; set; }
        public int FormateId { get;   set; }
        public string BarcodeValue { get;   set; }
        public Guid? BarcodeId { get;   set; }
        public string FormateName { get;   set; }
        public int SetCount { get;  set; }
        public bool WithBarcodes { get; set; }
        public bool Selected { get;  set; }

        public double? OrgPrice { get; set; }
        public double? SalePriceDefault { get; internal set; }
    }

}