using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Application.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Domain.AggregatesModel.Project>
    {
        private IProjectRepository _projectRepository;
        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }

        public async Task<Domain.AggregatesModel.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            await _projectRepository.AddAsync(request.Project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();

            return request.Project;
        }
    }
}
