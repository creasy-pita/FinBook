using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Application.IntergrationEvents
{
    public class ProjectJoinedIntergrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        //public string Avatar { get; set; }

        public ProjectContributor Contributor { get; set; }
    }
}
