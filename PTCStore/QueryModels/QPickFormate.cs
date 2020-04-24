using System;

namespace PTCStore.QueryModels
{
    public class QPickFormate
    {
        public Guid? PickOrderId { get; set; }
        public int StockUnitId { get; set; }
        public FormateNumbr[] FormateNumbrs { get; set; }

        public Guid[] BarcodeIds { get; set; }
    }
}