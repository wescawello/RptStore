using System;

namespace PTCStore.RanderModels
{
    public class RSaleOrder
    {
        public string ApplyNumber { get; set; }
        public string CustomerConnecter { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public double? OrgPrice { get; set; }
        public double? SalePrice { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}