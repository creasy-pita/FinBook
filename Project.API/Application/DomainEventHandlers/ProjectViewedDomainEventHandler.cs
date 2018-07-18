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
    public class ProjectViewedDomainEventHandler : INotificationHandler<ProjectViewedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectViewedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewedIntergrationEvent
            {
                Company = notification.Company
                ,Introduction = notification.Introduction
                ,Avatar = notification.Avatar
                ,Viewer = notification.Viewer
            };
            _capPublisher.Publish("finbook.projectapi.projectviewed", @event);
            return Task.CompletedTask;
        }
    }
}