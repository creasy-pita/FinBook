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
    public class ProjectJointedDomainEventHandler : INotificationHandler<ProjectJoinedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectJointedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedIntergrationEvent {
                Company = notification.Company
                ,Introduction = notification.Introduction
                ,Contributor = notification.Contributor
            };
            _capPublisher.Publish("finbook.projectapi.projectjoined", @event);
            return Task.CompletedTask;
        }

    }
}
