using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.DTOs.Product
{
    public class ProductTypeDTO
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
