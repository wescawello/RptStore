using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        public string Name { get; set; }

        public bool Display { get; set; }

        public List<VandorDevice> VandorDevices { get; set; }

       // [JsonIgnore]
        //public List<Formate> Formates { get; set; }


        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        public string UpDateId { get; set; }
    }
}