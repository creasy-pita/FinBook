using Recommend.API.Data;
using Recommend.API.IntegrationEvents;
using Recommend.API.Model;
using Recommend.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEventHandler
{
    public class ProjectCreatedIntegrationEventHandler//:ICapSubscribe
    {
        private RecommendDbContext _context;
        //private ICapSubscribe _capSubscribe;
        private IUserService _userService;

        public ProjectCreatedIntegrationEventHandler(RecommendDbContext context, IUserService userService)//, ICapSubscribe capSubscribe)
        {
            _context = context;
            _userService = userService;
            //_capSubscribe = capSubscribe;
        }

        public async Task CreateRecommendFromProject(ProjectCreatedIntergrationEvent @event)
        {
            //获取fromuser 信息 使用consul 服务发现 找到User 服务地址，获取用户基本信息
            var fromUser = await _userService.GetBaseUserInfoAsync(@event.UserId);
            if (fromUser == null)
            {
                //TBD 记录没有获取到指定userId的用户信息
            }
            //通过 Event传入项目的用户id,及项目其他基本信息  
            ProjectRecommend recommend = new ProjectRecommend {
                Company = @event.Company,
                FinStage = @event.Company,
                CreateTime = DateTime.Now,
                FromUserId = @event.UserId,
                Introduction = @event.Introduction,
                ProjectAvatar = @event.ProjectAvatar,
                ProjectId = @event.ProjectId,
                FromUserAvatar = fromUser.Avatar,
                FromUserName = fromUser.Name,
                RecommednType = EnumRecommendType.Friend
            };

            //使用通讯录查找所有好友，给这些好友添加 推荐记录


            //return Task.CompletedTask;
        }
    }
}
