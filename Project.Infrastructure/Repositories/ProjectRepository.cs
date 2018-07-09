using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository
        : IProjectRepository
    {
        private readonly ProjectContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public ProjectRepository(ProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Domain.AggregatesModel.Project> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.AggregatesModel.Project> AddAsync(Domain.AggregatesModel.Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.AggregatesModel.Project> UpdateAsync(Domain.AggregatesModel.Project project)
        {
            throw new NotImplementedException();
        }
    }
}
