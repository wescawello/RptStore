using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTCStore.RanderModels
{
    public class RPurchase : Purchase
    {
        //public Guid PurchaseId { get; set; }
 
        //public string ApplyNumber { get; set; }

        //public int VandorId { get; set; }
        //public int DeviceId { get; set; }

        //public Models.Formate Formate { get; set; }
        //public int FormateId { get; set; }

        //public int Quantity { get; set; }
        //public DateTimeOffset ApplyDate { get; set; }

        //public DateTimeOffset PurchaseDate { get; set; }
        //public double? PurchasePrice { get; set; }

        //public double? SalePrice { get; set; }
        //public string CreateId { get; set; }
        //public DateTimeOffset CreateDate { get; set; }
        //public DateTimeOffset? UpDateDate { get; set; }
        //public string UpDateId { get; set; }
        //public List<Models.Barcode> Barcodes { get; set; }
        public string VandorName { get; set; }
        public string DeviceName { get; set; }
        public string FormateName { get; set; }
        public double? TotalPrice { get; set; }
        //public string VandorName { get { return this.Formate.VandorDevice.Vandor.Name; } }
        //public string DeviceName { get { return this.Formate.VandorDevice.Device.Name; } }
        //public string FormateName { get { return this.Formate.Name; } }



      
    }
}
