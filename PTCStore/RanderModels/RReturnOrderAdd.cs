using System;

namespace PTCStore.RanderModels
{
    public class RReturnOrderAdd
    {
        public Guid PickOrderSubId { get; set; }

        public Guid? BarcodeId { get; set; }
        public string BarcodeValue { get; set; }
        public string FormateName { get; set; }
        public string VandorName { get; set; }
        public string DeviceName { get; set; }
        public int FormateId { get;   set; }
        public bool WithBarcodes { get;   set; }
        public double? OrgPrice { get; set; }
        public double? SalePrice { get; set; }
        public int ReturnCount { get; set; }
        public bool Returned { get; set; }
        public bool FReturned { get; set; }
        public double? ReturnPrice { get; set; }
        public string ReturnReason { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public DateTimeOffset? SaleDate { get; set; }
        public string SApplyNumber { get; set; }
        public string PApplyNumber { get; set; }
        public string StockUnitName { get; set; }
    }
}