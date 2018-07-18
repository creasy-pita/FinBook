using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectEntity =  Project.Domain.AggregatesModel.Project;
namespace Project.Application.IntergrationEvents
{
    public class ProjectCreatedIntergrationEvent
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
