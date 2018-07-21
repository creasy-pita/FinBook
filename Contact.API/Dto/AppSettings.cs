using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Dto
{
    public class AppSettings
    {
        public string MongoContactConnectionString { get; set; }
        public string ContactDatabaseName { get; set; }
    }
}
