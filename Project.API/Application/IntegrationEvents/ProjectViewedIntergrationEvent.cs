using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Application.IntergrationEvents
{
    public class ProjectViewedIntergrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Avatar { get; set; }

        public ProjectViewer Viewer { get; set; }
    }
}
