using Contact.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Task<bool> AddRequestAsync(ContactApplyRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 通过好友申请请求
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken);

        /// <summary>
        /// 获取好友申请请求列表
        /// </summary>
        /// <returns></returns>
        Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken);
    }
}
