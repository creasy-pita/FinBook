using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;

namespace Project.API.Application.Queries
{
    public class ProjectQuery : IProjectQuery
    {
        public Task<Domain.AggregatesModel.Project> GetProjectDetail(int projectId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Domain.AggregatesModel.Project>> GetProjectList(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
