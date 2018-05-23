using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Services;

namespace User.Identity.Test
{
    public class TestAuthCodeService : IAuthCodeService
    {
        public bool Validate(string phone, string authcode)
        {
            return true;
            throw new NotImplementedException();
        }
    }
}
