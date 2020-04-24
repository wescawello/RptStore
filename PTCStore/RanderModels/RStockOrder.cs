using System;

namespace PTCStore.RanderModels
{
    public class RStockOrder
    {
        public string OrgStockUnitName { get; set; }
        public string StockUnitName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public string ApplyNumber { get; set; }
        public string Keeper { get; set; }
        public Guid StockOrderId { get; set; }
        public int TotalBarcodes { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public string ProcessId { get; set; }
        public int OrgStockUnitId { get; set; }
        public string TrackId { get; set; }
        public int StockUnitId { get; set; }
        public bool Processed { get; set; }
    }
}