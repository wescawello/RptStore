using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTCStore.Models
{
    public class Purchase
    {
        public Guid PurchaseId { get; set; }
        [Required]
        public string ApplyNumber { get; set; }

        public int VandorId { get; set; }
        public int DeviceId { get; set; }

        public Formate Formate { get; set; }
        public int FormateId { get; set; }

        public int Quantity { get; set; }

        public string Remark { get; set; }

        public DateTimeOffset ApplyDate { get; set; }

        public DateTimeOffset PurchaseDate { get; set; }
        public double? PurchasePrice { get; set; }

        public double? SalePrice  { get; set; }
        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        public string UpDateId { get; set; }
        public List<Barcode> Barcodes { get; set; }

    }
}