using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Application.Queries
{
    interface IProjectQuery
    {
        Task<List<Project.Domain.AggregatesModel.Project>> GetProjectList(int userId);
        Task<Project.Domain.AggregatesModel.Project> GetProjectDetail(int projectId, int userId);
    }
}
