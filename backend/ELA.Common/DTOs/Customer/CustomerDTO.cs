using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.DTOs.Customer
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime? NewsletterOptIn { get; set; }
        public List<int> CustomerGroupIds { get; set; }
    }
}
