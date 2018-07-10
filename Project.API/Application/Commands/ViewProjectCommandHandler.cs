using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Application.Commands
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand,bool>
    {
        private IProjectRepository _projectRepository;
        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        public async Task<bool> Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);
            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"Project not found :{request.ProjectId}");
            }
            project.AddViewer(request.UserId, request.UserName, request.Avatar);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return true;
        }
    }
}
