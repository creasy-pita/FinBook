using MediatR;
using Project.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Application.EventHandler
{
    public class ProjectViewedEventHandler : INotificationHandler<ProjectViewedEvent>
    {
        public Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}