using PTCStore.Models;
using System.Collections.Generic;

namespace PTCStore.QueryModels
{
    public class QCustomerInfo : CustomerInfo, ISort
    {
        new public string Name { get; set; }
        public bool? QStatus { get; set; }
        new public int? DefaultPayMethod { get; set; }
        public Dictionary<string, string> Sort { get; set; }


    }
}