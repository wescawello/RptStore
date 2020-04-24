using PTCStore.Models;
using System;
using System.Collections.Generic;
 
using System.Text.Json.Serialization;
namespace PTCStore.QueryModels
{
    public class QSaleOrder: SaleOrder,ISort
    {
        new public Guid SaleOrderId { get; set; }
        new public string ApplyNumber { get; set; }

        public bool AskPre { get; set; }
        public Guid[] BarcodeIds { get; set; }
        public FormateNumbr[] FormateNumbrs { get; set; }
        public Guid[] PickOrderIds { get; set; }
        public List<DateTimeOffset> SaleDateRange { get; set; }

        public string Barcode { get; set; }
        [JsonPropertyName("sort")]
        public Dictionary<string, string> Sort { get; set; }
         
    }
}