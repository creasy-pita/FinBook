using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;

namespace Contact.API.Repositories
{
    public class ContactApplyRequestRepository : IContactApplyRequestRepository
    {
        public Task<bool> AddRequestAsync(ContactApplyRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApprovalAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetRequestListAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
