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
    public partial class QueryController
    {
        [HttpGet("{de:int}/{tag}/{did:int?}")]
        public async Task<IEnumerable<NameId>> ForVD(int de, string tag, int? did)
        {
            switch (tag)
            {
                case "vandors":
                    if (de > 0)
                        return await _context.VandorDevices.Where(o => o.DeviceId == de).Select(o => o.Vandor).OrderBy(o => o.Status).Select(o => new XNameId { Id = o.VandorId, Name = $"{o.Name}{ (o.Status ? "" : "(停用)")  }" }).ToListAsync();
                    else
                        return await _context.Vandors.OrderBy(o => o.Status).Select(o => new XNameId { Id = o.VandorId, Name = $"{o.Name}{ (o.Status ? "" : "(停用)")  }" }).ToListAsync();
                //break;
                case "devices":
                    if (de > 0)
                        return await _context.VandorDevices.Where(o => o.VandorId == de).Select(o => o.Device).OrderBy(o => o.Display).Select(o => new XNameId { Id = o.DeviceId, Name = $"{o.Name}{ (o.Display ? "" : "(不顯示)")  }" }).ToListAsync();
                    else
                        return await _context.Devices.OrderBy(o => o.Display).Select(o => new XNameId { Id = o.DeviceId, Name = $"{o.Name}{ (o.Display ? "" : "(不顯示)")  }" }).ToListAsync();
                //break;
                case "formates":
                    if (de > 0 && did > 0)
                    {

                        var vd = await _context.VandorDevices.Include(o => o.Formates).SingleAsync(o => o.VandorId == de && o.DeviceId == did);
                        return vd.Formates.OrderBy(o => o.Status).Select(o => new XNameId { Id = o.FormateId, Name = $"{o.Name}{ (o.Status ? "" : "(停用)")  }" });
                    }
                    else
                    {
                        return Enumerable.Empty<NameId>();
                    }
                case "formatesnob":
                    if (de > 0 && did > 0)
                    {

                        var vd = await _context.VandorDevices.Include(o => o.Formates).SingleAsync(o => o.VandorId == de && o.DeviceId == did);
                        return vd.Formates.Where(o => !o.WithBarcodes).OrderBy(o => o.Status).Select(o => new XNameId { Id = o.FormateId, Name = $"{o.Name}{ (o.Status ? "" : "(停用)")  }" });
                    }
                    else
                    {
                        return Enumerable.Empty<NameId>();
                    }
                case "stockunits":
                    return await _context.StockUnits.OrderBy(o => o.StockUnitId).Select(o => new XNameId { Id = o.StockUnitId, Name = $"{o.Name}" }).Where(x=>x.Id!=99 && x.Id != 100).ToListAsync();
                case "stockunitsAdmin":
                    return await _context.StockUnits.OrderBy(o => o.StockUnitId).Select(o => new XNameId { Id = o.StockUnitId, Name = $"{o.Name}" }).ToListAsync();

                default:

                    break;

            }
            return Enumerable.Empty<NameId>();
        }


        [HttpGet("{fid:int}")]
        public async Task<IEnumerable<FNameId>> StockUnitBarcodes(int fid)
        {
            //var pre = _context.Barcodes.Include(o => o.Purchase).ThenInclude(o => o.Formate).Where(o => o.StockUnitId == fid && !o.Purchase.Formate.WithBarcodes);
            //var rsFCounts = new List<FNameId>();

            var rs = _context.Barcodes.Include(o => o.Purchase).ThenInclude(o => o.Formate).Where(o => o.StockUnitId == fid && o.Purchase.Formate.WithBarcodes && !o.Saled && !o.Picked).Select(o => new FNameId
            {
                BarcodeId = o.BarcodeId,
                BarcodeValue = o.BarcodeValue,
                FormateId = o.Purchase.FormateId,
                FormateName = o.Purchase.Formate.Name,
                VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                WithBarcodes = true,
                Count = 1,
                SetCount = 1
            });

            return await rs.ToListAsync();
        }


     
       

    
        [HttpGet]
        public async Task<IEnumerable<NameId>> ForDropDown(string dlType)
        {
            return dlType switch
            {
                "Vendors" => Enumerable.Empty<NameId>(),
                //var rs = await _contextx.Vendors.Select(o => new XNameId
                //{
                //    Id = o.Id,
                //    Name = $"{o.Name}{ (o.Status == 1 ? "" : "(停用)")  }",
                //    EmsId = o.EmsId,
                //}
                //    ).ToListAsync();
                ////return rs;
                //foreach (var o in rs)
                //{
                //    o.Items =
                //     await _contextx.EquipmeatFixTypes.Where(k => _contextx.EquipmeatVendors.Where(p => p.VendorsId == o.EmsId).Select(q => q.EquipmeatId).Contains(k.EmsId)).Select(e => new NameId
                //     {
                //         Id = e.EmsId,
                //         Name = e.Name
                //     }).ToListAsync();
                //}
                //return rs.Select(o => new NameId { Id = o.Id, Name = o.Name, Items = o.Items });
                "MVendors" => await _context.Vandors.OrderBy(o => o.Status).Select(o => new XNameId { Id = o.VandorId, Name = $"{o.Name}{ (o.Status ? "" : "(停用)")  }" }).ToListAsync(),

                _ => Enumerable.Empty<NameId>(),
            };
        }

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<StockUnit>> StockUnits(PTCStore.QueryModels.QStockUnit query, int page, int pageSize = 10)
        {
            var rs = _context.StockUnits.Where(query.GetPredicate());//.Select(o=>new RPurchase());
            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }



        #region 轉倉
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<RStockOrder>> StockOrders(PTCStore.QueryModels.QStockOrder query, int page, int pageSize = 10)
        {
            var rs = _context.StockOrders.Where(query.GetPredicate()).Select(o => new RStockOrder
            {
                StockOrderId = o.StockOrderId,
                ProcessDate = o.ProcessDate,
                ProcessId = o.ProcessId,
                ApplyNumber = o.ApplyNumber,
                Keeper = o.Keeper,
                OrgStockUnitName = o.OrgStockUnit.Name,
                StockUnitName = o.StockUnit.Name,
                CreateDate = o.CreateDate,
                OrgStockUnitId = o.OrgStockUnitId,
                StockUnitId = o.StockUnitId,
                TrackId = o.TrackId,
                Processed = o.ProcessDate.HasValue,
                TotalBarcodes = o.StockOrderSubs.Count()
            });
            if (!query.Sort.ContainsKey("Processed"))
            {
                query.Sort = (new Dictionary<string, string> { { "Processed", "asc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }

            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<FNameId>> StockOrderAddQuery(PTCStore.QueryModels.QStockOrder query, int page, int pageSize = 10)
        {

            if (!query.Sort.ContainsKey("WithBarcodes"))
            {
                query.Sort = (new Dictionary<string, string> { { "WithBarcodes", "desc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }
            if (!query.AskPre)
            {
                var rs = _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId && !o.Picked && !o.Saled).Select(o => new
                {
                    BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                    BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = o.Key.WithBarcodes ? 1 : 0
                });
                return await rs.GetPagedAsync(pageSize, page, query.Sort); ;
            }
            else
            {

                var fids = Enumerable.Empty<Guid>();
                if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
                {
                    fids = fids.Concat(query.BarcodeIds);
                }
                query.FormateNumbrs.ToList().ForEach(f =>
                {
                    fids = fids.Concat(_context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId && o.Purchase.FormateId == f.FormateId).OrderBy(o => o.Purchase.CreateDate).Take((int)f.SetCount).Select(o => o.BarcodeId));
                });


                var rs = _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId).Where(o => fids.Contains(o.BarcodeId)).Select(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = 1
                });
                return await rs.GetPagedAsync(pageSize, page, query.Sort); ;


            }


        }

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<FNameId>> StockOrderEditQuery(PTCStore.QueryModels.QStockOrder query, int page, int pageSize = 10)
        {
            if (!query.Sort.ContainsKey("WithBarcodes"))
            {
                query.Sort = (new Dictionary<string, string> { { "WithBarcodes", "desc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }
            if (!query.AskPre)
            {

                var rs = query.ProcessDate.HasValue ? _context.StockOrderSubs.Where(o => o.StockOrderId == query.StockOrderId).Select(o => o.Barcode).Select(o => new
                {
                    BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                    BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = o.Key.WithBarcodes ? 1 : o.Count(),
                    //Selected=true
                })
                :
                _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId).Select(o => new
                {
                    BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                    BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = o.Key.WithBarcodes ? 1 : 0
                });

                return await rs.GetPagedAsync(pageSize, page, query.Sort); ;
            }
            else
            {
                if (query.ProcessDate.HasValue)
                {
                    var fids = Enumerable.Empty<Guid>();
                    if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
                    {
                        fids = fids.Concat(query.BarcodeIds);
                    }
                    query.FormateNumbrs.ToList().ForEach(f =>
                    {
                        fids = fids.Concat(_context.StockOrderSubs.Where(o => o.StockOrderId == query.StockOrderId).Select(o => o.Barcode).Where(o => o.Purchase.FormateId == f.FormateId).OrderBy(o => o.Purchase.CreateDate).Take((int)f.SetCount).Select(o => o.BarcodeId));
                    });




                    var rs = _context.StockOrderSubs.Where(o => o.StockOrderId == query.StockOrderId).Select(o => o.Barcode)
                        .Where(o => fids.Contains(o.BarcodeId)).Select(o => new
                        {
                            o.BarcodeId,
                            o.BarcodeValue,
                            o.Purchase.FormateId,
                            FormateName = o.Purchase.Formate.Name,
                            VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                            DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                            o.Purchase.Formate.WithBarcodes,
                        }).GroupBy(o => new
                        {
                            o.BarcodeId,
                            o.BarcodeValue,
                            o.FormateId,
                            o.FormateName,
                            o.VandorName,
                            o.DeviceName,
                            o.WithBarcodes,
                        }).Select(o => new FNameId
                        {
                            BarcodeId = o.Key.BarcodeId,
                            BarcodeValue = o.Key.BarcodeValue,
                            FormateId = o.Key.FormateId,
                            FormateName = o.Key.FormateName,
                            VandorName = o.Key.VandorName,
                            DeviceName = o.Key.DeviceName,
                            WithBarcodes = o.Key.WithBarcodes,
                            Count = o.Count(),
                            SetCount = 1,
                            //Selected = true
                        });
                    return await rs.GetPagedAsync(pageSize, page, query.Sort); ;

                }
                else
                {


                    var fids = Enumerable.Empty<Guid>();
                    if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
                    {
                        fids = fids.Concat(query.BarcodeIds);
                    }
                    query.FormateNumbrs.ToList().ForEach(f =>
                    {
                        fids = fids.Concat(_context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId && o.Purchase.FormateId == f.FormateId && !o.Picked && !o.Saled).OrderBy(o => o.Purchase.CreateDate).Take((int)f.SetCount).Select(o => o.BarcodeId));
                    });


                    var rs = _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId).Where(o => fids.Contains(o.BarcodeId)).Select(o => new
                    {
                        o.BarcodeId,
                        o.BarcodeValue,
                        o.Purchase.FormateId,
                        FormateName = o.Purchase.Formate.Name,
                        VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                        DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                        o.Purchase.Formate.WithBarcodes,
                    }).GroupBy(o => new
                    {
                        o.BarcodeId,
                        o.BarcodeValue,
                        o.FormateId,
                        o.FormateName,
                        o.VandorName,
                        o.DeviceName,
                        o.WithBarcodes,
                    }).Select(o => new FNameId
                    {
                        BarcodeId = o.Key.BarcodeId,
                        BarcodeValue = o.Key.BarcodeValue,
                        FormateId = o.Key.FormateId,
                        FormateName = o.Key.FormateName,
                        VandorName = o.Key.VandorName,
                        DeviceName = o.Key.DeviceName,
                        WithBarcodes = o.Key.WithBarcodes,
                        Count = o.Count(),
                        SetCount = 1
                    });
                    return await rs.GetPagedAsync(pageSize, page, query.Sort); ;
                }
            }
        }

        [HttpPost]
        public async Task<IEnumerable<Guid>> AskFormateBarcodes(QStockFormate query)
        {
            var fids = Enumerable.Empty<Guid>();
            foreach (var f in query.FormateNumbrs)
            {
                fids = fids.Concat(await _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId && o.Purchase.FormateId == f.FormateId && !o.Picked && !o.Saled).OrderBy(o => o.Purchase.CreateDate).Take(f.SetCount).Select(o => o.BarcodeId).ToListAsync());
                //StockOrderId
            }
            return fids;
        }
        //
        [HttpPost]
        public async Task<IEnumerable<StockOrderSub>> AskStockOrderFormateBarcodes(QStockFormate query)
        {
            var fids = Enumerable.Empty<StockOrderSub>();
            if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
            {

                fids = fids.Concat(await _context.StockOrderSubs.Where(o => o.StockOrderId == query.StockOrderId).Where(o => query.BarcodeIds.Contains(o.BarcodeId)).OrderBy(o => o.Barcode.Purchase.CreateDate).ToListAsync());
            }

            foreach (var f in query.FormateNumbrs)
            {

                fids = fids.Concat(await _context.StockOrderSubs.Where(o => o.StockOrderId == query.StockOrderId).Where(o => o.Barcode.Purchase.FormateId == f.FormateId).OrderBy(o => o.Barcode.Purchase.CreateDate).Take(f.SetCount).ToListAsync());

            }
            return fids;
        }
        [HttpPost]
        public async Task<ExecResult> StockOrderProcess(RStockOrder query)
        {
            try
            {
                var stockOrder = await _context.StockOrders.Include(o => o.StockOrderSubs).ThenInclude(s => s.Barcode).FirstAsync(o => o.StockOrderId == query.StockOrderId);
                if (stockOrder != null)
                {
                    stockOrder.ProcessId = query.ProcessId;
                    stockOrder.ProcessDate = query.ProcessDate;
                    if (stockOrder.ProcessDate.HasValue)
                    {
                        var barcodes = stockOrder.StockOrderSubs.Select(o => o.Barcode).Distinct().ToList();
                        barcodes.ForEach(o => o.StockUnitId = stockOrder.StockUnitId);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("無效StockOrderId");
                }




                return new ExecResult { Success = true, Message = $"執行完成" };
            }
            catch (Exception e)
            {

                return new ExecResult { Success = false, Message = $"執行失敗({e.Message})" };
            }
        }

        [HttpGet("{fid:guid}")]
        public async Task<IEnumerable<FNameId>> StockOrderBarcodes(Guid fid)
        {
            var rs=_context.Barcodes.Where(o => o.StockUnitId == _context.StockOrders.Find(fid).OrgStockUnitId && !o.Picked   
            && !o.Saled && o.Purchase.Formate.WithBarcodes)
                .Select(o => new FNameId
                {
                    BarcodeId = o.BarcodeId,
                    BarcodeValue = o.BarcodeValue,
                    FormateId = o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    WithBarcodes = true,
                    Count = 1,
                    SetCount = 1
                });
            return await rs.ToListAsync();
        }


        [HttpGet("{fid:guid}")]
        public async Task<IEnumerable<FNameId>> StockOrderEditMap(Guid fid)
        {

            var rs = _context.StockOrderSubs.Where(o => o.StockOrderId == fid && !o.Barcode.Saled).Select(o => o.Barcode).Select(o => new
            {
                BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                o.Purchase.FormateId,
                FormateName = o.Purchase.Formate.Name,
                VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                o.Purchase.Formate.WithBarcodes,
            }).GroupBy(o => new
            {
                o.BarcodeId,
                o.BarcodeValue,
                o.FormateId,
                o.FormateName,
                o.VandorName,
                o.DeviceName,
                o.WithBarcodes,
            }).Select(o => new FNameId
            {
                BarcodeId = o.Key.BarcodeId,
                BarcodeValue = o.Key.BarcodeValue,
                FormateId = o.Key.FormateId,
                FormateName = o.Key.FormateName,
                VandorName = o.Key.VandorName,
                DeviceName = o.Key.DeviceName,
                WithBarcodes = o.Key.WithBarcodes,
                Count = o.Count(),
                SetCount = o.Key.WithBarcodes ? 1 : o.Count(),
                Selected = true
            });
            return await rs.ToListAsync();
        }



        [HttpGet]
        public async Task<Boolean> AllStockOrderProsess()
        {
            return !await _context.StockOrders.AnyAsync(o => !o.ProcessDate.HasValue);

        }
        #endregion


        #region 客戶

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<CustomerInfo>> CustomerInfos(QCustomerInfo query, int page, int pageSize = 10)
        {
            var rs = _context.CustomerInfos.Where(query.GetPredicate());//.Select(o=>new RPurchase());
            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }
        #endregion

        #region 檢貨
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<RPickOrder>> PickOrders(PTCStore.QueryModels.QPickOrder query, int page, int pageSize = 10)
        {
            var rs = _context.PickOrders.Where(query.GetPredicate()).Select(o => new RPickOrder
            {
                PickOrderId = o.PickOrderId,
                ProcessDate = o.ProcessDate,
                ProcessId = o.ProcessId,
                Remark=o.Remark,
                Keeper = o.Keeper,
                SaleOrderId = o.SaleOrderId,
                StockUnitName = o.StockUnit.Name,
                CreateDate = o.CreateDate,
 
                StockUnitId = o.StockUnitId,
                TrackId = o.TrackId,
                Processed = o.ProcessDate.HasValue,
                TotalBarcodes = o.PickOrderSubs.Count(),
                OrgPrice = o.PickOrderSubs.Select(b=>b.Barcode.Purchase).Select(p =>  p.PurchasePrice/  (p.Formate.DoTimes ? p.Formate.SetCount : 1)).Sum(),
                SalePriceDefault= o.PickOrderSubs.Select(b => b.Barcode.Purchase).Select(p => p.Formate.SalePriceDefault / (p.Formate.DoTimes ? p.Formate.SetCount : 1)).Sum(),
            });
            if (!query.Sort.ContainsKey("Processed"))
            {
                query.Sort = (new Dictionary<string, string> { { "Processed", "asc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }
            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<FNameId>> PickOrderAddQuery(PTCStore.QueryModels.QStockOrder query, int page, int pageSize = 10)
        {

            if (!query.Sort.ContainsKey("WithBarcodes"))
            {
                query.Sort = (new Dictionary<string, string> { { "WithBarcodes", "desc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }
            if (!query.AskPre)
            {
                var rs = _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId && !o.Picked).Select(o => new
                {
                    BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                    BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = o.Key.WithBarcodes ? 1 : 0
                });
                return await rs.GetPagedAsync(pageSize, page, query.Sort); ;
            }
            else
            {

                var fids = Enumerable.Empty<Guid>();
                if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
                {
                    fids = fids.Concat(query.BarcodeIds);
                }
                query.FormateNumbrs.ToList().ForEach(f =>
                {
                    fids = fids.Concat(_context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId && o.Purchase.FormateId == f.FormateId && !o.Picked)
                        .OrderBy(o => o.Purchase.CreateDate).Take((int)f.SetCount).Select(o => o.BarcodeId));
                });


                var rs = _context.Barcodes.Where(o => o.StockUnitId == query.OrgStockUnitId  && !o.Picked).Where(o => fids.Contains(o.BarcodeId)).Select(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                    OrgPrice = o.Purchase.PurchasePrice / (o.Purchase.Formate.DoTimes ? o.Purchase.Formate.SetCount : 1)
                }).GroupBy(o => new
                {
                    o.OrgPrice,
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = 1,
                    OrgPrice=o.Key.OrgPrice
                });
                var extmsg = new Dictionary<string, string>
                {
                    { "allOrgPrice" , $"總成本 : {rs.Sum(o=>o.OrgPrice)}" }
                };

                return await rs.GetPagedAsync(pageSize, page, query.Sort, extmsg); ;


            }


        }

        [HttpGet("{fid:guid}")]
        public async Task<IEnumerable<FNameId>> PickOrderBarcodes(Guid fid)
        {
            var rs = _context.Barcodes.Where(o => o.StockUnitId == _context.PickOrders.Find(fid).StockUnitId
            && o.Purchase.Formate.WithBarcodes
            && (!o.Picked || _context.PickOrderSubs.Where(p => p.PickOrderId == fid).Select(p => p.BarcodeId).Contains(o.BarcodeId)) && !o.Saled)
              .Select(o => new FNameId
              {
                  BarcodeId = o.BarcodeId,
                  BarcodeValue = o.BarcodeValue,
                  FormateId = o.Purchase.FormateId,
                  FormateName = o.Purchase.Formate.Name,
                  VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                  DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                  WithBarcodes = true,
                  Count = 1,
                  SetCount = 1
              });

            return await rs.ToListAsync();
        }



        [HttpGet("{fid:guid}")]
        public async Task<IEnumerable<FNameId>> PickOrderEditMap(Guid fid)
        {

            var rs = _context.PickOrderSubs.Where(o => o.PickOrderId == fid).Select(o => o.Barcode).Select(o => new
            {
                BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                o.Purchase.FormateId,
                FormateName = o.Purchase.Formate.Name,
                VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                o.Purchase.Formate.WithBarcodes,
            }).GroupBy(o => new
            {
                o.BarcodeId,
                o.BarcodeValue,
                o.FormateId,
                o.FormateName,
                o.VandorName,
                o.DeviceName,
                o.WithBarcodes,
            }).Select(o => new FNameId
            {
                BarcodeId = o.Key.BarcodeId,
                BarcodeValue = o.Key.BarcodeValue,
                FormateId = o.Key.FormateId,
                FormateName = o.Key.FormateName,
                VandorName = o.Key.VandorName,
                DeviceName = o.Key.DeviceName,
                WithBarcodes = o.Key.WithBarcodes,
                Count = o.Count(),
                SetCount = o.Key.WithBarcodes ? 1 : o.Count(),
                Selected = true
            });
            return await rs.ToListAsync();
        }

        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<FNameId>> PickOrderEditQuery(PTCStore.QueryModels.QPickOrder query, int page, int pageSize = 10)
        {
            if (!query.Sort.ContainsKey("WithBarcodes"))
            {
                query.Sort = (new Dictionary<string, string> { { "WithBarcodes", "desc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }
            if (!query.AskPre)
            {

                var rs = query.ProcessDate.HasValue ? _context.PickOrderSubs.Where(o => o.PickOrderId == query.PickOrderId).Select(o => o.Barcode).Select(o => new
                {
                    BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                    BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = o.Key.WithBarcodes ? 1 : o.Count(),
                    //Selected=true
                })
                :
                _context.Barcodes.Where(o => o.StockUnitId == query.StockUnitId).Select(o => new
                {
                    BarcodeId = o.Purchase.Formate.WithBarcodes ? (Guid?)o.BarcodeId : null,
                    BarcodeValue = o.Purchase.Formate.WithBarcodes ? o.BarcodeValue : null,
                    o.Purchase.FormateId,
                    FormateName = o.Purchase.Formate.Name,
                    VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                    DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                    o.Purchase.Formate.WithBarcodes,
                }).GroupBy(o => new
                {
                    o.BarcodeId,
                    o.BarcodeValue,
                    o.FormateId,
                    o.FormateName,
                    o.VandorName,
                    o.DeviceName,
                    o.WithBarcodes,
                }).Select(o => new FNameId
                {
                    BarcodeId = o.Key.BarcodeId,
                    BarcodeValue = o.Key.BarcodeValue,
                    FormateId = o.Key.FormateId,
                    FormateName = o.Key.FormateName,
                    VandorName = o.Key.VandorName,
                    DeviceName = o.Key.DeviceName,
                    WithBarcodes = o.Key.WithBarcodes,
                    Count = o.Count(),
                    SetCount = o.Key.WithBarcodes ? 1 : 0
                });

                return await rs.GetPagedAsync(pageSize, page, query.Sort); ;
            }
            else
            {
                if (query.ProcessDate.HasValue)
                {
                    var fids = Enumerable.Empty<Guid>();
                    if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
                    {
                        fids = fids.Concat(query.BarcodeIds);
                    }
                    query.FormateNumbrs.ToList().ForEach(f =>
                    {
                        fids = fids.Concat(_context.PickOrderSubs.Where(o => o.PickOrderId == query.PickOrderId).Select(o => o.Barcode).Where(o => o.Purchase.FormateId == f.FormateId).OrderBy(o => o.Purchase.CreateDate).Take((int)f.SetCount).Select(o => o.BarcodeId));
                    });




                    var rs = _context.PickOrderSubs.Where(o => o.PickOrderId == query.PickOrderId).Select(o => o.Barcode)
                        .Where(o => fids.Contains(o.BarcodeId)).Select(o => new
                        {
                            SalePriceDefault = o.Purchase.Formate.SalePriceDefault / (o.Purchase.Formate.DoTimes ? o.Purchase.Formate.SetCount : 1),

                            OrgPrice = o.Purchase.PurchasePrice / (o.Purchase.Formate.DoTimes ? o.Purchase.Formate.SetCount : 1),
                            o.BarcodeId,
                            o.BarcodeValue,
                            o.Purchase.FormateId,
                            FormateName = o.Purchase.Formate.Name,
                            VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                            DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                            o.Purchase.Formate.WithBarcodes,
                        }).GroupBy(o => new
                        {
                            o.SalePriceDefault,
                            o.OrgPrice,
                            o.BarcodeId,
                            o.BarcodeValue,
                            o.FormateId,
                            o.FormateName,
                            o.VandorName,
                            o.DeviceName,
                            o.WithBarcodes,
                        }).Select(o => new FNameId
                        {
                            SalePriceDefault=o.Key.SalePriceDefault,
                            OrgPrice= o.Key.OrgPrice,
                            BarcodeId = o.Key.BarcodeId,
                            BarcodeValue = o.Key.BarcodeValue,
                            FormateId = o.Key.FormateId,
                            FormateName = o.Key.FormateName,
                            VandorName = o.Key.VandorName,
                            DeviceName = o.Key.DeviceName,
                            WithBarcodes = o.Key.WithBarcodes,
                            Count = o.Count(),
                            SetCount = 1,
                            //Selected = true
                        });
                    var extmsg = new Dictionary<string, string>
                {
                    { "allOrgPrice" , $"總成本 : {rs.Sum(o=>o.OrgPrice)}" }
                };
                    return await rs.GetPagedAsync(pageSize, page, query.Sort, extmsg); ;

                }
                else
                {


                    var fids = Enumerable.Empty<Guid>();
                    if (query.BarcodeIds != null && query.BarcodeIds.Length > 0)
                    {
                        fids = fids.Concat(query.BarcodeIds);
                    }
                    
                    
                    
                    query.FormateNumbrs.ToList().ForEach(f =>
                    {
                        fids = fids.Concat(_context.Barcodes.Where(o => o.StockUnitId == query.StockUnitId && o.Purchase.FormateId == f.FormateId && (!o.Picked || _context.PickOrderSubs.Where(p => p.PickOrderId == query.PickOrderId).Select(p => p.Barcode.BarcodeId).Contains(o.BarcodeId) ) && !o.Saled).OrderBy(o => o.Purchase.CreateDate).Take((int)f.SetCount).Select(o => o.BarcodeId));
                    });


                    var rs = _context.Barcodes.Where(o => o.StockUnitId == query.StockUnitId).Where(o => fids.Contains(o.BarcodeId)).Select(o => new
                    {
                        SalePriceDefault = o.Purchase.Formate.SalePriceDefault / (o.Purchase.Formate.DoTimes ? o.Purchase.Formate.SetCount : 1),
                        OrgPrice = o.Purchase.PurchasePrice / (o.Purchase.Formate.DoTimes ? o.Purchase.Formate.SetCount : 1),
                        o.BarcodeId,
                        o.BarcodeValue,
                        o.Purchase.FormateId,
                        FormateName = o.Purchase.Formate.Name,
                        VandorName = o.Purchase.Formate.VandorDevice.Vandor.Name,
                        DeviceName = o.Purchase.Formate.VandorDevice.Device.Name,
                        o.Purchase.Formate.WithBarcodes,
                    }).GroupBy(o => new
                    {
                        o.SalePriceDefault           ,
                        o.OrgPrice,
                        o.BarcodeId,
                        o.BarcodeValue,
                        o.FormateId,
                        o.FormateName,
                        o.VandorName,
                        o.DeviceName,
                        o.WithBarcodes,
                    }).Select(o => new FNameId
                    {
                        SalePriceDefault=o.Key.SalePriceDefault,
                        OrgPrice = o.Key.OrgPrice,
                        BarcodeId = o.Key.BarcodeId,
                        BarcodeValue = o.Key.BarcodeValue,
                        FormateId = o.Key.FormateId,
                        FormateName = o.Key.FormateName,
                        VandorName = o.Key.VandorName,
                        DeviceName = o.Key.DeviceName,
                        WithBarcodes = o.Key.WithBarcodes,
                        Count = o.Count(),
                        SetCount = 1
                    });
                    var extmsg = new Dictionary<string, string>
                {
                    { "allOrgPrice" , $"總成本 : {rs.Sum(o=>o.OrgPrice)}" }
                };
                    return await rs.GetPagedAsync(pageSize, page, query.Sort, extmsg); ;
                }
            }
        }


        [HttpPost]
        public async Task<IEnumerable<Guid>> AskPickOrderFormateBarcodes(QPickFormate query)
        {
            var fids = Enumerable.Empty<Guid>();
            foreach (var f in query.FormateNumbrs)
            {
                fids = fids.Concat(await _context.Barcodes.Where(o => o.StockUnitId == query.StockUnitId && o.Purchase.FormateId == f.FormateId && (!o.Picked || _context.PickOrderSubs.Where(p=>p.PickOrderId==query.PickOrderId).Select(p=>p.BarcodeId).Contains(o.BarcodeId))&& !o.Saled).OrderBy(o => o.Purchase.CreateDate).Take(f.SetCount).Select(o => o.BarcodeId).ToListAsync());
            }
            return fids;
        }

        #endregion





        #region 售貨
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<RSaleOrder>> SaleOrders(PTCStore.QueryModels.QSaleOrder query, int page, int pageSize = 10)
        {
            var rs = _context.SaleOrders.Where(query.GetPredicate()).Select(o => new RSaleOrder
            {
                ApplyNumber = o.ApplyNumber,
                CustomerConnecter = o.Customer.Connecter,
                CustomerName = o.Customer.Name,
                ItemCount = o.PickOrders.SelectMany(p => p.PickOrderSubs).Count(),
                OrgPrice = o.PickOrders.SelectMany(p => p.PickOrderSubs).Select(p => p.Barcode).Select(b => b.Purchase.PurchasePrice / (b.Purchase.Formate.DoTimes ? b.Purchase.Formate.SetCount : 1)).Sum(),
                SalePrice = o.PickOrders.SelectMany(p => p.PickOrderSubs).Sum(s=>s.SalePrice),
                CreateDate = o.CreateDate,
            });;



            return await rs.GetPagedAsync(pageSize, page, query.Sort);
        }

 [HttpPost]
        public async Task< List<RSaleOrderMap>> SaleOrderMap(PTCStore.QueryModels.QSaleOrder query)
        {

           
            var pickordersubs = !query.SaleOrderId.Equals(Guid.Empty) ? _context.PickOrders.Where(o => o.SaleOrderId == query.SaleOrderId).SelectMany(o => o.PickOrderSubs)
                :
                _context.PickOrders.Where(o => query.PickOrderIds.Contains(o.PickOrderId)).SelectMany(o => o.PickOrderSubs);
            var rs=pickordersubs.Select(o => new RSaleOrderMap {
            PickOrderSubId=o.PickOrderSubId,
            Barcode=o.Barcode,
                OrgPrice= o.Barcode.Purchase.PurchasePrice / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1),
                SalePrice =query.Magnification *  o.Barcode.Purchase.Formate.SalePriceDefault / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1) ,
            });
            return await rs.ToListAsync();
        }
        [HttpPost]
        public async Task<bool> FixOrderPrices(List<RSaleOrderMap> subs)
        {
            try
            {
                var rs = await _context.PickOrderSubs.Where(o => subs.Select(s => s.PickOrderSubId).Contains(o.PickOrderSubId)).ToListAsync();

                rs.ForEach(r =>
                {
                    r.SalePrice = subs.FirstOrDefault(s => s.PickOrderSubId == r.PickOrderSubId).SalePrice;


                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //SaleOrdersAddQuery
        [HttpPost("{pageSize:int}/{page:int}")]
        public async Task<PagedResult<RSaleOrderDetail>> SaleOrdersDetail(PTCStore.QueryModels.QSaleOrder query, int page, int pageSize = 10)
        {
            if (!query.Sort.ContainsKey("WithBarcodes"))
            {
                query.Sort = (new Dictionary<string, string> { { "WithBarcodes", "desc" } }).Concat(query.Sort).ToDictionary(k => k.Key, v => v.Value);
            }
            var pickordersubs = !query.SaleOrderId.Equals(Guid.Empty) ? _context.PickOrders.Where(o => o.SaleOrderId == query.SaleOrderId).SelectMany(o => o.PickOrderSubs)
           :
           _context.PickOrders.Where(o => query.PickOrderIds.Contains(o.PickOrderId)).SelectMany(o => o.PickOrderSubs);

            var rs = pickordersubs.Select(o => new RSaleOrderDetail
            {
                PickOrderSubId = o.PickOrderSubId,
                Barcode = o.Barcode,
                BarcodeValue = o.Barcode.BarcodeValue,
                WithBarcodes=o.Barcode.Purchase.Formate.WithBarcodes,
                VandorName = o.Barcode.Purchase.Formate.VandorDevice.Vandor.Name,
                DeviceName= o.Barcode.Purchase.Formate.VandorDevice.Device.Name,
                FormateName= o.Barcode.Purchase.Formate.Name ,
                StockUnitName=   o.Barcode.StockUnit.Name,
                OrgPrice = o.Barcode.Purchase.PurchasePrice / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1),
                SalePrice = query.Magnification * o.Barcode.Purchase.Formate.SalePriceDefault / (o.Barcode.Purchase.Formate.DoTimes ? o.Barcode.Purchase.Formate.SetCount : 1),
            });
            var extmsg = new Dictionary<string, string>
                {
                    { "allOrgPrice" , $"總成本 : {rs.Sum(o=>o.OrgPrice)}" }
                };
            return await rs.GetPagedAsync(pageSize, page, query.Sort, extmsg);
        }
        #endregion
    }
}
