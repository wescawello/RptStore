using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTCStore.Models
{
    public class ReturnOrder
    {
        public Guid ReturnOrderId { get; set; }

        public string ApplyNumber { get; set; }

        public string MainResson { get; set; }
        public string Remark { get; set; }
        public Customer Customer { get; set; }
        public List<PickOrderSub> ReturnOrderSubs { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }


        [StringLength(100, MinimumLength = 1)]
        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }
    }
}