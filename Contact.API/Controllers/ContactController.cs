using System;
using System.Collections.Generic;
using System.Linq;
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
        private IContactApplyRequestRepository _repository;
        private IUserService _userService;

        public ContactController(IContactApplyRequestRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-request")]
        public async Task<IActionResult> GetApplyRequests(int userId)
        {
            var requests = await _repository.GetRequestListAsync(UserIdentity.UserId);
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
            var result = await _repository.AddRequestAsync(new ContactApplyRequest()
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = baseUserInfo.Name,
                Company = baseUserInfo.Company,
                Title = baseUserInfo.Title,
                CreateTime = DateTime.Now,
                Avatar = baseUserInfo.Avatar//TBD 申请人和被申请人有可能搞乱
            });
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
        public async Task<IActionResult> ApprovalApplyRequest(int applierId)
        {
            //TBD also need parameter userId
            var result = await _repository.ApprovalAsync(applierId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
