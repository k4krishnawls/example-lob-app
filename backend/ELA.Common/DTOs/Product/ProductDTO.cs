using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}
