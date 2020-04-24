using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace PTCStore.RanderModels
{
    public class RPickOrder
    {
        public int TotalBarcodes { get; set; }
         
        public Guid PickOrderId { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public string ProcessId { get; set; }
        public string Keeper { get; set; }
        public string StockUnitName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int StockUnitId { get; set; }
        public string TrackId { get; set; }
        public bool Processed { get; set; }
        public string Remark { get; set; }
        public Guid? SaleOrderId { get; set; }

        public double? OrgPrice { get; set; }
        public double? SalePriceDefault { get; set; }
    }
}