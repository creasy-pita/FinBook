using Contact.API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Repositories
{
    public interface IContactRepository
    {
        Task<bool> UpdateContactInfo(BaseUserInfo userInfo, CancellationToken cancellationToken);
        Task<bool> AddContactAsync(int userId, BaseUserInfo contact, CancellationToken cancellationToken);
    }
}
