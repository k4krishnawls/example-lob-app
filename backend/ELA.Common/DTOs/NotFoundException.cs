using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.DTOs
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string dtoName, object id)
            : base($"{dtoName} with id {id} could not be found")
        {
            Data.Add("DTO", dtoName);
            Data.Add("Id", id);
        }
    }
}
