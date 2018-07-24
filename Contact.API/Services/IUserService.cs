using Contact.API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Services
{
    public interface IUserService
    {
        Task<BaseUserInfo> GetBaseUserInfoAsync(int userId,CancellationToken cancellationToken);
    }
}
