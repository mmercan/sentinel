using System;

namespace Sentinel.Web.Model.Product
{
    public class VendorInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlPart { get; set; }
        public string HeaderImage { get; set; }
        public string LogoImage { get; set; }
        public string Base64HeaderImage { get; set; }
        public string Base64LogoImage { get; set; }
        public string VideoURL { get; set; }
        public string Html { get; set; }
        public string CertificationHTML { get; set; }
        public int OrderId { get; set; }
        public bool Active { get; set; }
        public bool ShowOnPartnerPage { get; set; }
        public string CountryCodes { get; set; }

    }
}