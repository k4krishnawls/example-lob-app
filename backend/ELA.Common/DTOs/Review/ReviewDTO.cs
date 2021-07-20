using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Review
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public RatingScore Rating { get; set; }
        public string Comment { get; set; }
    }

    public enum RatingScore
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
}
