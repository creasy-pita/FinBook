using DotNetCore.CAP;
using MediatR;
using Project.Application.IntergrationEvents;
using Project.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Application.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
                var @event = new ProjectCreatedIntergrationEvent { CreateTime = DateTime.Now, ProjectId = notification.Project.Id, UserId = notification.Project.UserId};
                _capPublisher.Publish("finbook.projectapi.projectcreated", @event);
                return Task.CompletedTask;
        }
    }
}
