using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace PTCStore.Models
{
    public class PickOrder
    {
        public Guid PickOrderId { get; set; }
        public Guid? SaleOrderId { get; set; }
        public int? CustomerInfoId { get; set; }




        [JsonIgnore] 
        public SaleOrder SaleOrder { get; set; }


        /// <summary>
        /// 運單號
        /// </summary>
        public string TrackId { get; set; }


        /// <summary>
        /// 負責人
        /// </summary>
        [StringLength(100, MinimumLength = 1)]
        
        
        public string Keeper { get; set; }
        public string Remark { get; set; }

        public int StockUnitId { get; set; }
        public StockUnit StockUnit { get; set; }

        public DateTimeOffset? ProcessDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string ProcessId { get; set; }

        public List<PickOrderSub> PickOrderSubs { get; set; }


        [StringLength(100, MinimumLength = 1)]

        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }



    }
}