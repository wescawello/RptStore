using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PTCStore.Models
{
    public class SaleOrder
    {
      
        public Guid SaleOrderId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)] 
        public string ApplyNumber { get; set; }
        public string Invoice { get; set; }


        public DateTimeOffset? SaleDate { get; set; }

        public Customer Customer { get; set; }


        public List<PickOrder> PickOrders { get; set; }





        [StringLength(100, MinimumLength = 1)]

        public string CreateId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpDateDate { get; set; }
        [StringLength(100, MinimumLength = 1)]
        public string UpDateId { get; set; }

        public double Magnification { get; set; } = 1;
    }


    [Owned]
    public class Customer
    {
        public int CustomerInfoId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 結帳方式
        /// </summary>
        public int PayMethod { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Address { get; set; }

        /// <summary>
        /// 統編
        /// </summary>
        [StringLength(100, MinimumLength = 1)]
        public string TaxId { get; set; }
        /// <summary>
        /// 聯繫人
        /// </summary>
        [StringLength(100, MinimumLength = 1)]
        public string Connecter { get; set; }


        [StringLength(100, MinimumLength = 1)]
        public string Phone { get; set; }
    }
}