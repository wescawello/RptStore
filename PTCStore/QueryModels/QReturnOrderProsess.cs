using System;

namespace PTCStore.QueryModels
{
    public class QReturnOrderProsess
    {
        public Guid PickOrderSubId { get; set; }
        public Guid ReturnOrderId { get; set; }
        public string ReturnReason { get; set; }
        public double ReturnPrice { get; set; }
    }
}