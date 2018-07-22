using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Dto;

namespace User.Identity.Services
{
    public interface IUserService
    {
        Task<BaseUserInfo> CheckOrCreate(string phone);
    }
}
