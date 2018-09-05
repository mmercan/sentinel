using System;

namespace Sentinel.Web.Model.Product
{
    public class TechnologyInfo
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int OrderBy { get; set; }
    }
}