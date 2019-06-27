using System;
using System.Collections.Generic;

namespace Sentinel.Model.Product.Dto
{
    public class ProductPriceInfo
    {
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public List<string> GroupsToApply { get; set; }
    }
}