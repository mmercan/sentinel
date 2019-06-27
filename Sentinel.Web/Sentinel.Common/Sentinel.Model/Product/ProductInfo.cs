using System;
using System.Collections.Generic;
using Sentinel.Model.Product.Dto;

namespace Sentinel.Model.Product
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string ProductUrl { get; set; }
        public bool Active { get; set; }
        public bool UseTabs { get; set; }
        public string Html { get; set; }
        public string Duration { get; set; }
        public string DurationType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int TechnologyId { get; set; }
        public string TechnologyName { get; set; }
        public string TechnologyUrl { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorUrl { get; set; }
        public virtual ProductTabsInfo ProductTabs { get; set; }
        public virtual ProductAssetsInfo ProductAssets {get;set;}
        public virtual List<ProductPriceInfo> ProductPrices { get; set; }

        // public virtual CategoryInfo Category { get; set; }
        // public virtual VendorInfo Vendor { get; set; }
        // public virtual TechnologyInfo TechnologyInfo { get; set; }
    }
}
