using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Application.Service
{
    public interface IRecommendService
    {
        Task<bool> IsProjectRecommend(int projectId, int userId);
    }
}
