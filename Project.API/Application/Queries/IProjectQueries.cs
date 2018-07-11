using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Application.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetProjectsByUserId(int userId);
        Task<dynamic> GetProjectDetail(int projectId, int userId);
    }
}
