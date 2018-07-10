using Project.Domain.SeedWork;
using System;
using System.Threading.Tasks;

namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository: IRepository<Project>
    {
        Task<Project> GetAsync(int id);
        Project Add(Project project);
        Project Update(Project project);
    }
}
