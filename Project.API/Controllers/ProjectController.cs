using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Application.Commands;
using Project.API.Application.Queries;
using Project.API.Application.Service;
using Project.API.Dto;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private IMediator _mediatR;
        private IRecommendService _recommendService;
        private IProjectQueries _projectQueries;

        public ProjectController(IMediator mediator, IRecommendService recommendService, IProjectQueries projectQueries)
        {
            _mediatR = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProject()
        {
            Domain.AggregatesModel.Project project = new Domain.AggregatesModel.Project();
            project.Introduction = "公司介绍";
            project.Company = "creasypita";
            project.UserId = UserIdentity.UserId;
            CreateProjectCommand command = new CreateProjectCommand { Project = project };
            var result = _mediatR.Send(command, new CancellationToken());
            await result;
            return Ok(result);
        }

        [HttpPost]
        [Route("TBD")]
        public async Task<IActionResult> CreateProject([FromBody]Domain.AggregatesModel.Project project)
        {
            //TBD  [FromBody] 传值有点问题  先用   非[FromBody] 代替   

            if (project==null)
            {
                throw new ArgumentNullException();
            }
            project.UserId = UserIdentity.UserId;
            CreateProjectCommand command = new CreateProjectCommand { Project = project};
            var result =_mediatR.Send(command, new CancellationToken());
            await result;
            return Ok(result);
        }

        [HttpPut]
        [Route("join/{projectId}")]
        public async Task<IActionResult> JoinProject([FromBody]ProjectContributor contributor)
        {
            if (!(await _recommendService.IsProjectRecommend(contributor.ProjectId, UserIdentity.UserId)))
            {
                return BadRequest("没有查看该项目的权限");
            }

            JoinProjectCommand command = new JoinProjectCommand {Contributor= contributor };
            var result = await  _mediatR.Send(command, new CancellationToken());
            return Ok(result);
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            if (!(await _recommendService.IsProjectRecommend(projectId, UserIdentity.UserId)))
            {
                return BadRequest("没有查看该项目的权限");
            }

            UserIdentity identity = UserIdentity;
            ViewProjectCommand command = new ViewProjectCommand
            {
                ProjectId = projectId
                ,
                UserId = identity.UserId
                ,
                UserName = identity.Name
                ,
                Avatar = identity.Avatar
            };
            var result = await _mediatR.Send(command, new CancellationToken());
            return Ok(result);
        }

        /// <summary>
        /// 获取用户 (自己??)的项目列表 //TBD
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _projectQueries.GetProjectsByUserId(UserIdentity.UserId);
            return Ok(result);
        }

        [Route("my/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var result = await _projectQueries.GetProjectDetail(projectId,-1);
            return Ok(result);
        }
        //api/recommend/{projectId}  用户获取推荐项目的 详细信息
        [Route("recommend/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetRecommendProjectDetail(int projectId)
        {
            if (!(await _recommendService.IsProjectRecommend(projectId, UserIdentity.UserId)))
            {
                return BadRequest("没有查看该项目的权限");
            }
            var result = await _projectQueries.GetProjectDetail(projectId, -1);
            return Ok(result);
        }

    }
}
