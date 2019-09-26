using System;
using System.Collections.Generic;
using Sentinel.Model.Product.Dto;

namespace Sentinel.Model.Product
{
    public class ProductImage
    {
        public int width { get; set; }
        public int height { get; set; }
        public string Base64mage { get; set; }
        public string src { get; set; }
    }
}