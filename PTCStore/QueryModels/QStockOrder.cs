using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PTCStore.QueryModels
{
  public  class QStockOrder: StockOrder, ISort
    {
        new public Guid StockOrderId { get; set; }
       new public string ApplyNumber { get; set; }


        public bool AskPre { get; set; }
        public Guid[] BarcodeIds { get; set; }

        public FormateNumbr[] FormateNumbrs { get; set; }

         

        [JsonPropertyName("sort")] 
        public Dictionary<string, string> Sort { get; set; }

       
    }
    public class FormateNumbr
    {
        public int FormateId { get; set; }
        public int SetCount { get; set; }
        public int? StockUnitId { get; set; }
    }
}
