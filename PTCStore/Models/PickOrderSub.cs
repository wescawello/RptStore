using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace PTCStore.Models
{
    public class PickOrderSub
    {
        public Guid PickOrderSubId { get; set; }

        public Guid PickOrderId { get; set; }
        [JsonIgnore]
        public PickOrder PickOrder { get; set; }


        public double? SalePrice { get; set; }
        public double ReturnPrice { get; set; } = 0;
        public string ReturnReason { get; set; }


        public Guid BarcodeId { get; set; }

        [JsonIgnore]
        public virtual Barcode Barcode { get; set; }

        public bool Returned { get; set; }


        public Guid? ReturnOrderId { get; set; }
        [JsonIgnore]
        public ReturnOrder ReturnOrder { get; set; }

    }
}