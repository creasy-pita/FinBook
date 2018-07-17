using MediatR;
using Project.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Application.EventHandler
{
    public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
