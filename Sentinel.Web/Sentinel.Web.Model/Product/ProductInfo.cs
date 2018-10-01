using System;

namespace Sentinel.Web.Model.Product
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public bool Active { get; set; }
        public bool UseTabs { get; set; }
        public string Html { get; set; }
        public string DescriptionHtml { get; set; }
        public string ObjectivesHtml { get; set; }
        public string AudienceHtml { get; set; }
        public string PrerequisitesHtml { get; set; }
        public string TopicsHtml { get; set; }
        public string RelatedHtml { get; set; }
        public string RoadmapsHtml { get; set; }

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

        public virtual VendorInfo Vendor { get; set; }
        public virtual TechnologyInfo TechnologyInfo { get; set; }
    }
}
