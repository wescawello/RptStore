using System;

namespace PTCStore.RanderModels
{
    public class RReturnOrder
    {
        public Guid ReturnOrderId { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }
        public string CustomerName { get; set; }
        public double? ReturnPrice { get; set; }
        public int ItemCount { get; set; }
        public double? SalePrice { get; set; }
        public string CustomerConnecter { get; set; }
        public double? OrgPrice { get; set; }
        public string ApplyNumber { get; set; }
    }
}