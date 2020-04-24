using PTCStore.Models;
using System;

namespace PTCStore.RanderModels
{
    public class RSaleOrderDetail
    {
 
 
        public double? OrgPrice { get; set; }
        public double? SalePrice { get; set; }
        public Barcode Barcode { get; set; }
        public Guid PickOrderSubId { get; set; }
        public string VandorName { get; set; }
        public string DeviceName { get; set; }
        public string FormateName { get; set; }
        public string StockUnitName { get; set; }
        public bool WithBarcodes { get; set; }
        public string BarcodeValue { get; set; }
    }
}