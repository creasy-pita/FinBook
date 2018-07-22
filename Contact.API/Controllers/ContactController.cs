using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using Contact.API.Repositories;
using Contact.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _contactApplyRequestRepository;
        private IContactRepository _contactRepository;
        private IUserService _userService;

        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository
            , IUserService userService
            ,IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;
        }
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult> GetApplyRequests(int userId)
        {
            var requests = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId,new CancellationToken());
            return Json(requests);
        }

        /// <summary>
        /// 申请人选择 用户 进行好友申请
        /// </summary>
        /// <param name="userId">被申请人id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-request")]
        public async Task<IActionResult> AddApplyRequest(int userId)
        {
            var baseUserInfo = await _userService.GetBaseUserInfoAsync(userId);
            if (baseUserInfo == null)
            {
                throw new Exception("用户参数错误");
            }
            var result = await _contactApplyRequestRepository.AddRequestAsync(new ContactApplyRequest()
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = baseUserInfo.Name,
                Company = baseUserInfo.Company,
                Title = baseUserInfo.Title,
                CreateTime = DateTime.Now,
                Avatar = baseUserInfo.Avatar//TBD 申请人和被申请人有可能搞乱
            }, new CancellationToken());
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// 通过好友申请请求
        /// </summary>
        /// <param name="applierId">申请人Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-request")]
        public async Task<IActionResult> ApprovalApplyRequest(int applierId, CancellationToken cancellationToken)
        {

            //获取 当前上下文的用户  和申请人的  信息
            var user = await _userService.GetBaseUserInfoAsync(UserIdentity.UserId);
            var applier = await _userService.GetBaseUserInfoAsync(applierId);
            //当前上下文的用户id 添加好友
            await _contactRepository.AddContactAsync(user.UserId, new Dto.BaseUserInfo
            {
                Avatar = applier.Avatar,
                Company = applier.Company,
                Name = applier.Name,
                Title = applier.Title,
                UserId = applier.UserId
            }, cancellationToken);

            //对方好友也要添加当前用户作为好友
            await _contactRepository.AddContactAsync(applierId, new Dto.BaseUserInfo
            {
                Avatar = user.Avatar,
                Company = user.Company,
                Name = user.Name,
                Title = user.Title,
                UserId = user.UserId
            }, cancellationToken);

            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId, new CancellationToken());
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
