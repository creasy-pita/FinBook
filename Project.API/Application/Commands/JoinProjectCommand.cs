using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Application.Commands
{
    public class JoinProjectCommand:IRequest<bool>
    {
        public ProjectContributor Contributor { get; set; }
    }
}
