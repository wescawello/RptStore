using PTCStore.Models;
using System;

namespace PTCStore.RanderModels
{
    public class RSaleOrderMap
    {
        public Guid PickOrderSubId { get; set; }
        public Barcode Barcode { get; set; }
        public double? OrgPrice { get; set; }
        public double? SalePrice { get; set; }
    }
}