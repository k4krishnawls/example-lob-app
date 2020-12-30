using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELA.App.Controllers.Frontend.Models
{
    public class CreateUserResponseModel
    {

        public CreateUserResponseModel(int newId)
        {
            Id = newId;
        }

        public int Id { get; set; }
    }
}
