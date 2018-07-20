using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Dto
{
    public class JsonErrorResponse
    {
        public string Message { get; set; }
        public string DeveloperMessage { get; set; }
    }
}
