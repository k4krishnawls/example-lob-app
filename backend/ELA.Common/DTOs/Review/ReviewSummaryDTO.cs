using ELA.Common.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Review
{
    public class ReviewSummaryDTO
    {
        public int Id { get; set; }
        public RatingScore Rating { get; set; }

        public ReviewStatus Status { get; set; }
        public string Comment { get; set; }
        public CustomerSummaryDTO Customer { get; set; }
    }
}
