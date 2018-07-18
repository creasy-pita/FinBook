using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Application.Service
{
    public interface IRecommendService
    {
        /// <summary>
        /// 查看当前用户需要查看的项目 是否在推荐列表中，在推荐列表才可查看）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsProjectRecommend(int projectId, int userId);
    }
}
