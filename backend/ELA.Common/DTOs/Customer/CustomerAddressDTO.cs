using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Customer
{
    public class CustomerAddressDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateAbbr { get; set; }
        public string ZipCode { get; set; }
        public DateTime ArchivedOn { get; set; }
    }
}
