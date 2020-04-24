using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class CustomerInfo
    {
        public int CustomerInfoId { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        /// <summary>
        /// 結帳方式
        /// </summary>
        [JsonPropertyName("PayMethod")]
        public int DefaultPayMethod { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Address { get; set; }

        /// <summary>
        /// 統編
        /// </summary>
        [StringLength(100, MinimumLength = 1)]
        public string  TaxId { get; set; }
        /// <summary>
        /// 聯繫人
        /// </summary>
        [StringLength(100, MinimumLength = 1)]
        public string Connecter { get; set; }



        public bool Status { get; set; }



        [StringLength(100, MinimumLength = 1)]
        public string Phone { get; set; }

        [StringLength(100, MinimumLength = 1)]

        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }




    }
}