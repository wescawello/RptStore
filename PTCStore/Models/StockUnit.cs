using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PTCStore.Models
{
    public class StockUnit
    {
        public int StockUnitId { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string Keeper { get; set; }


        public string[] OOXX { get; set; }


        [StringLength(100, MinimumLength = 1)]

        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }

    }
}
