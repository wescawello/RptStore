using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class Vandor
    {
        public int VandorId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool TaxStatus { get; set; }
        public bool SupplierFlag { get; set; }
        public bool ServiceFlag { get; set; }
        [JsonIgnore]
        public List<VandorDevice> VandorDevices { get; set; }


        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        public string UpDateId { get; set; }
    }
}