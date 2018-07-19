using Recommend.API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.Services
{
    public interface IUserService
    {
        Task<UserIdentity> GetBaseUserInfoAsync(int userId);
    }
}
