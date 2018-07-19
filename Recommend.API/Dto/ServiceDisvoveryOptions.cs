using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Recommend.API.Dto
{
    public class ServiceDisvoveryOptions
    {
        public string UserServiceName { get; set; }

        public string ContactServiceName { get; set; }
        public ConsulOptions Consul { get; set; }
    }
}
