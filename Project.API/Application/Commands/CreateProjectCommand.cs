using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Project.API.Application.Commands
{
    public class CreateProjectCommand:IRequest<Domain.AggregatesModel.Project>
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}
