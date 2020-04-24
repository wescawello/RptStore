using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTCStore.Models;
using PTCStore.QueryModels;
using PTCStore.RanderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PtcERPWeb.Controllers
{
    //info
    public partial class QueryController
    {
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<IActionResult> Infos(QInfo query, int page, int pageSize = 10)
        {

            var rs = Infosbase(query);

            return Ok(await rs.GetPagedAsync(pageSize, page, query.Sort));
        }


        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<IActionResult> InfobyStocks(QInfo query, int page, int pageSize = 10)
        {

            var rs = Infosbase(query);


            return Ok(await rs.GroupBy(g => new { g.StockUnitName, g.StockUnitId }).Select(s => new GGG
            {
                ProdName = "",
                StockUnitName = s.Key.StockUnitName,
                StockUnitId = s.Key.StockUnitId,
                Count = s.Sum(r => r.Count),
                Price = s.Sum(r => r.Price)
            }).GetPagedAsync(pageSize, page, query.Sort));
        }

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<IActionResult> InfobyProds(QInfo query, int page, int pageSize = 10)
        {
            query.ByProd = true;
            var rs = Infosbase(query);


            return Ok(await rs.GroupBy(g => new { g.ProdName, g.FormateId }).Select(s => new GGG
            {
                ProdName = s.Key.ProdName,
                FormateId = s.Key.FormateId,
                StockUnitName = "",
                Count = s.Sum(r => r.Count),
                Price = s.Sum(r => r.Price)
            }).GetPagedAsync(pageSize, page, query.Sort));
        }
        public IQueryable<GGG> Infosbase(QInfo query)
        {
            var prers = _context.Barcodes.Include(o => o.Purchase).ThenInclude(p => p.Formate).Where(o => !o.Picked && !o.Saled && o.Purchase.PurchasePrice.HasValue);
            if (query.ByProd)
            {
                prers = prers.Where(o => o.StockUnit.Name != "報廢倉");
            }
            if (query.FormateId.HasValue)
            {
                prers = prers.Where(o => o.Purchase.FormateId == query.FormateId);
            }
            if (query.StockUnitId.HasValue)
            {
                prers = prers.Where(o => o.StockUnitId == query.StockUnitId.Value);
            }
            return prers.Select(
                o => new
                {
                    Vn = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    Dn = o.Purchase.Formate.VandorDevice.Device.Name,
                    Fn = o.Purchase.Formate.Name,
                    StockUnitName = o.StockUnit.Name,
                    FormateId = o.Purchase.Formate.FormateId,
                    StockUnitId = o.StockUnit.StockUnitId,
                    Price = o.Purchase.PurchasePrice.HasValue ? (o.Purchase.PurchasePrice.Value / (o.Purchase.Formate.DoTimes ? o.Purchase.Formate.SetCount.Value : 1)) : 0
                }
                )
                .Select(t => new
                {
                    StockUnitName = t.StockUnitName,
                    ProdName = t.Vn + "/" + t.Dn + "/" + t.Fn,
                    StockUnitId = t.StockUnitId,
                    FormateId = t.FormateId,
                    Price = t.Price
                }).GroupBy(x => new { x.StockUnitName, x.ProdName, x.StockUnitId, x.FormateId })
                .Select(s => new GGG
                {
                    ProdName = s.Key.ProdName,
                    StockUnitName = s.Key.StockUnitName,
                    StockUnitId = s.Key.StockUnitId,
                    FormateId = s.Key.FormateId,
                    Count = s.Count(),
                    Price = s.Sum(r => r.Price)
                });

        }
        [HttpGet]
        public async Task<IActionResult> InfoHisSave()
        {
            var query = new QInfo();
            var crDateYM = DateTime.Now.ToString("yyyyMM");
            var rs = Infosbase(query);
            var rsInfos=await rs.Select(o => new InfoHistory
            {
                ProdName = o.ProdName,
                StockUnitName = o.StockUnitName,
                StockUnitId = o.StockUnitId,
                FormateId = o.FormateId,
                Price = o.Price,
                Count = o.Count,
                CreateYM= crDateYM,
                TypeName= "Infos"
            }).ToListAsync();
            var rsInfobyStocks =await rs.GroupBy(g => new { g.StockUnitName, g.StockUnitId }).Select(s => new GGG
            {
                ProdName = "",
                StockUnitName = s.Key.StockUnitName,
                StockUnitId = s.Key.StockUnitId,
                Count = s.Sum(r => r.Count),
                Price = s.Sum(r => r.Price)
            }).Select(o => new InfoHistory
            {
                ProdName = o.ProdName,
                StockUnitName = o.StockUnitName,
                StockUnitId = o.StockUnitId,
                FormateId = o.FormateId,
                Price = o.Price,
                Count = o.Count,
                CreateYM = crDateYM,
                TypeName = "Stock"
            }).ToListAsync();






            query = new QInfo { ByProd = true };
            rs = Infosbase(query);
            var rsInfobyProds =await rs.GroupBy(g => new { g.ProdName, g.FormateId }).Select(s => new GGG
            {
                ProdName = s.Key.ProdName,
                FormateId = s.Key.FormateId,
                StockUnitName = "",
                Count = s.Sum(r => r.Count),
                Price = s.Sum(r => r.Price)
            }).Select(o => new InfoHistory
            {
                ProdName = o.ProdName,
                StockUnitName = o.StockUnitName,
                StockUnitId = o.StockUnitId,
                FormateId = o.FormateId,
                Price = o.Price,
                Count = o.Count,
                CreateYM = crDateYM,
                TypeName = "Prod"
            }).ToListAsync();
            var allRs = rsInfos.Union(rsInfobyStocks).Union(rsInfobyProds);

            if (_context.InfoHistories.Any(o => o.CreateYM == crDateYM))
            {
                _context.InfoHistories.RemoveRange(_context.InfoHistories.Where(o => o.CreateYM == crDateYM));
                var irm = await _context.SaveChangesAsync();
            }

            _context.InfoHistories.AddRange(allRs);
            var iadd= await _context.SaveChangesAsync();


            return Ok(  iadd );
        }


    }

    public class GGG
    {
        public string ProdName { get; set; }
        public string StockUnitName { get; set; }
        public int StockUnitId { get; set; }
        public int FormateId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Vn { get; set; }
        public string Dn { get; set; }
        public string Fn { get; set; }

    }


  
}