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

    public partial class QueryController
    {
        #region 退貨
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<PTCStore.RanderModels.RReturnOrder>> ReturnOrders(PTCStore.QueryModels.QReturnOrder query, int page, int pageSize = 10)
        {

            var rs = _context.ReturnOrders.Where(query.GetPredicate()).Select(o => new RReturnOrder
            {
                ApplyNumber = o.ApplyNumber,
                ReturnOrderId = o.ReturnOrderId,
                ReturnDate = o.ReturnDate,
                CustomerName = o.Customer.Name,
                CustomerConnecter = o.Customer.Connecter,
                OrgPrice  = o.ReturnOrderSubs.Select(x=>x.Barcode.Purchase.PurchasePrice / (x.Barcode.Purchase.Formate.DoTimes ? x.Barcode.Purchase.Formate.SetCount : 1)).Sum(),
                ReturnPrice = o.ReturnOrderSubs.Select(r => r.ReturnPrice).Sum(),
                SalePrice = o.ReturnOrderSubs.Select(r => r.SalePrice).Sum(),
                ItemCount = o.ReturnOrderSubs.Count(),
            });


            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }

        //ReturnOrderProsess
        [HttpPost ]
        public async Task<bool> ReturnOrderProsess(List<PTCStore.QueryModels.QReturnOrderProsess> models)
        {
            try
            {
                var ps = await _context.PickOrderSubs.Include(o=>o.Barcode).Where(o => models.Select(m => m.PickOrderSubId).Contains(o.PickOrderSubId)).ToListAsync();
                ps.ForEach(p =>
                {
                    var m = models.FirstOrDefault(o => o.PickOrderSubId == p.PickOrderSubId);
                    if (m != null)
                    {
                        p.ReturnOrderId = m.ReturnOrderId;
                        p.ReturnPrice = m.ReturnPrice;
                        p.ReturnReason = m.ReturnReason;
                        p.Returned = true;

                        p.Barcode.StockUnitId = 99;
                        p.Barcode.Saled = false;
                        p.Barcode.Picked = false;
                        p.Barcode.ReturnCount += 1;
                        
                    }

                });
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        
        
        
        
        }
    //ReturnOrderAddQuery
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<RReturnOrderAdd>> ReturnOrderAddQuery(PTCStore.QueryModels.QReturnOrderAdd query, int page, int pageSize = 10)
        {
            if (query.Customer.CustomerInfoId > 0)
            {
                var rs = _context.PickOrderSubs.Where(o => o.PickOrder.SaleOrder.Customer.CustomerInfoId == query.Customer.CustomerInfoId && !o.Returned)
                 .Select(o => new PTCStore.RanderModels.RReturnOrderAdd
                 {
                     PickOrderSubId = o.PickOrderSubId,

                     BarcodeId = o.Barcode.BarcodeId,
                     BarcodeValue = o.Barcode.BarcodeValue,
                     FormateId = o.Barcode.Purchase.FormateId,
                     FormateName = o.Barcode.Purchase.Formate.Name,
                     VandorName = o.Barcode.Purchase.Formate.VandorDevice.Vandor.Name,
                     DeviceName = o.Barcode.Purchase.Formate.VandorDevice.Device.Name,
                     WithBarcodes = o.Barcode.Purchase.Formate.WithBarcodes,
                     StockUnitName= o.PickOrder.StockUnit.Name,
                     PurchaseDate = o.Barcode.Purchase.PurchaseDate,
                     PApplyNumber = o.Barcode.Purchase.ApplyNumber,
                     OrgPrice = o.Barcode.Purchase.PurchasePrice / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1),

                     SaleDate = o.PickOrder.SaleOrder.SaleDate,
                     SApplyNumber = o.PickOrder.SaleOrder.ApplyNumber,
                     SalePrice = o.SalePrice,

                     Returned = o.Returned,
                     FReturned = o.Returned,
                     ReturnPrice = o.ReturnPrice,
                     ReturnReason = o.ReturnReason,
                  
                     ReturnCount = o.Barcode.ReturnCount,
                 }
                );
                if(query.PickOrderSubIds!=null && query.PickOrderSubIds.Count > 0)
                {
                    rs = rs.Where(o => query.PickOrderSubIds.Contains(o.PickOrderSubId));
                }

                return await rs.GetPagedAsync(pageSize, page, query.Sort);
            }
            else
            {
                var rs = _context.Barcodes.Where(o => false).Select(o => new PTCStore.RanderModels.RReturnOrderAdd()

                );
                return await rs.GetPagedAsync(pageSize, page, query.Sort);


            }




        }



        //
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<RReturnOrderAdd>> ReturnOrderEditQuery(PTCStore.QueryModels.QReturnOrderEdit query, int page, int pageSize = 10)
        {

            var rs = _context.PickOrderSubs.Where(o => o.ReturnOrderId == query.ReturnOrderId)
             .Select(o => new PTCStore.RanderModels.RReturnOrderAdd
             {
                 PickOrderSubId = o.PickOrderSubId,

                 BarcodeId = o.Barcode.BarcodeId,
                 BarcodeValue = o.Barcode.BarcodeValue,
                 FormateId = o.Barcode.Purchase.FormateId,
                 FormateName = o.Barcode.Purchase.Formate.Name,
                 VandorName = o.Barcode.Purchase.Formate.VandorDevice.Vandor.Name,
                 DeviceName = o.Barcode.Purchase.Formate.VandorDevice.Device.Name,
                 WithBarcodes = o.Barcode.Purchase.Formate.WithBarcodes,
                 StockUnitName = o.PickOrder.StockUnit.Name,
                 PurchaseDate = o.Barcode.Purchase.PurchaseDate,
                 PApplyNumber = o.Barcode.Purchase.ApplyNumber,
                 OrgPrice = o.Barcode.Purchase.PurchasePrice / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1),

                 SaleDate = o.PickOrder.SaleOrder.SaleDate,
                 SApplyNumber = o.PickOrder.SaleOrder.ApplyNumber,
                 SalePrice = o.SalePrice,

                 Returned = o.Returned,
                 FReturned = o.Returned,
                 ReturnPrice = o.ReturnPrice,
                 ReturnReason = o.ReturnReason,

                 ReturnCount = o.Barcode.ReturnCount,
             }
            );
            var extmsg = new Dictionary<string, string>
                {
                    { "AllOrgPrice" , $"總成本 : {rs.Sum(o=>o.OrgPrice)}" },
                    { "AllSalePrice" , $"總售本 : {rs.Sum(o=>o.SalePrice)}" },
                    { "AllReturnPrice" , $"總退額 : {rs.Sum(o=>o.ReturnPrice)}" },
                    { "AllCount" , $"總數量 : {rs.Count()}" },
                };

            return await rs.GetPagedAsync(pageSize, page, query.Sort,extmsg);




        }

        [HttpGet("{customerInfoId:int}")]
        public async Task<List<RReturnOrderAdd>> ReturnOrderAddQueryNopage(int customerInfoId)
        {
            var rs = _context.PickOrderSubs.Where(o => o.PickOrder.SaleOrder.Customer.CustomerInfoId == customerInfoId && !o.Returned)
                .Select(o => new PTCStore.RanderModels.RReturnOrderAdd
                {
                    PickOrderSubId = o.PickOrderSubId,

                    BarcodeId = o.Barcode.BarcodeId,
                    BarcodeValue = o.Barcode.BarcodeValue,
                    FormateId = o.Barcode.Purchase.FormateId,
                    FormateName = o.Barcode.Purchase.Formate.Name,
                    VandorName = o.Barcode.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Barcode.Purchase.Formate.VandorDevice.Device.Name,
                    WithBarcodes = o.Barcode.Purchase.Formate.WithBarcodes,

                    PurchaseDate = o.Barcode.Purchase.PurchaseDate,
                    PApplyNumber = o.Barcode.Purchase.ApplyNumber,
                    OrgPrice = o.Barcode.Purchase.PurchasePrice / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1),

                    SaleDate = o.PickOrder.SaleOrder.SaleDate,
                    SApplyNumber = o.PickOrder.SaleOrder.ApplyNumber,
                    SalePrice = o.SalePrice,

                    Returned = o.Returned,
                    FReturned = o.Returned,
                    ReturnPrice = o.ReturnPrice,
                    ReturnReason = o.ReturnReason,

                    ReturnCount = o.Barcode.ReturnCount,
                }
               );
            return await rs.ToListAsync();




        }
        //ReturnOrderAddQueryNopage

        #endregion
    }




}