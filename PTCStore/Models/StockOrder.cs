using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PTCStore.Models
{
    public class StockOrder
    {
        public Guid StockOrderId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)] 
        public string ApplyNumber { get; set; }

        public int OrgStockUnitId { get; set; } = 1;
        public int StockUnitId { get; set; } = 1;
        [StringLength(100, MinimumLength = 1)]
        public string Keeper { get; set; }

        
        public string TrackId { get; set; }


        public string[] Summary { get; set; }

        public List<StockOrderSub> StockOrderSubs { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string ProcessId { get; set; }

        public StockUnit  StockUnit { get; set; }
        public StockUnit OrgStockUnit { get; set; }
       



    }
}
