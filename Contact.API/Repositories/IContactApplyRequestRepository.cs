using Contact.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Repositories
{
    public interface IContactApplyRequestRepository
    {
        /// <summary>
        /// 添加好友申请 请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddRequestAsync(ContactApplyRequest request);

        /// <summary>
        /// 通过好友申请请求
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int applierId);

        /// <summary>
        /// 获取好友申请请求列表
        /// </summary>
        /// <returns></returns>
        Task<bool> GetRequestListAsync(int userId);
    }
}
