using ELA.Common.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.BusinessLogic
{
    public interface IInteractiveUserQueryService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
    }
}
