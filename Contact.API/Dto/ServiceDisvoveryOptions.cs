using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Contact.API.Dto
{
    public class ServiceDisvoveryOptions
    {
        public string UserServiceName { get; set; }
        /// <summary>
        /// 通讯录服务注册到consul dns 中的名称
        /// </summary>
        public string ContactServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
