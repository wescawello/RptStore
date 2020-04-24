using System;
using System.Collections.Generic;
using System.Text;

namespace PTCStore.QueryModels
{
    public class QStockFormate
    {
        public Guid? StockOrderId { get; set; }
        public int OrgStockUnitId { get; set; }
        public FormateNumbr[] FormateNumbrs { get; set; }

        public Guid[] BarcodeIds { get; set; }
    }
}
