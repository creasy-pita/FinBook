using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Events
{
    public class ProjectViewedEvent : INotification
    {
        public ProjectViewer Viewer { get; set; }
    }
}
