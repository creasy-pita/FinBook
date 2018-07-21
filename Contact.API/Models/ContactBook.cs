using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Models
{
    public class ContactBook
    {
        public int UserId { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
