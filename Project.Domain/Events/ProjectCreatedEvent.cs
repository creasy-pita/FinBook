using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectEntity =  Project.Domain.AggregatesModel.Project;
namespace Project.Domain.Events
{
    public class ProjectCreatedEvent: INotification
    {
        public ProjectEntity Project { get; set; }
    }
}
