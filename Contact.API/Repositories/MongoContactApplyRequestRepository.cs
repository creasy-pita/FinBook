using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Repositories
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private ContactContext _context;

        public MongoContactApplyRequestRepository(ContactContext context)
        {
            _context = context;
        }

        public Task<bool> AddRequestAsync(ContactApplyRequest request, CancellationToken cancellationToken)
        {
            //_context.ContactApplyRequests.InsertOneAsync()
            return Task.FromResult(true);
            throw new NotImplementedException();
        }

        public Task<bool> ApprovalAsync(int applierId, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
            throw new NotImplementedException();
        }


        //TBD  CancellationToken的作用
        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            return (await _context.ContactApplyRequests.FindAsync( r=> r.UserId == userId)).ToList(cancellationToken);
        }
    }
}
