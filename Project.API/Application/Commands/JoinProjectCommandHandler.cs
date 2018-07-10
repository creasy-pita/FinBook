using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Application.Commands
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand,bool>
    {
        private IProjectRepository _projectRepository;
        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        public async Task<bool> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);
            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"Project not found :{request.Contributor.ProjectId}");

            }
            project.AddContributor(request.Contributor);
           // await _projectRepository.UpdateAsync(project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return true;
        }
    }
}
