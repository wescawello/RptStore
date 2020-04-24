using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class VandorDevice
    {
 
        public int VandorId { get; set; }
        [JsonIgnore]
        public Vandor Vandor { get; set; }

        public virtual string Name { get { return Vandor?.Name; } }

        public int DeviceId { get; set; }

        [JsonIgnore]
        public  Device Device { get; set; }
        [JsonIgnore]
        public List<Formate> Formates { get; set; }

        //public virtual string DeviceName { get { return Device.Name; } }

    }
}