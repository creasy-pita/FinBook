using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Data;
using User.API.Model;

namespace User.API.Controllers
{
    [Route("api/[controller]")]

    public class UserController : BaseController
    {
        private AppUserDbContext _userContext;
        private ILogger<UserController> _logger;

        public UserController(AppUserDbContext userContext, ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        // GET api/user/CheckOrCreate
        [Route("check-or-create")]
        [HttpPost]
        public int CheckOrCreate(string phone)
        {
            //System.IO.Stream s = HttpContext.Request.;
            //byte[] buffer = new byte[s.Length];
            //s.Read(buffer, 0, buffer.Length);
            //string sss = System.Text.Encoding.UTF8.GetString(buffer);

            if (phone == "2")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var user = _userContext.Users.AsTracking()
                .Include(u => u.properties)
                .SingleOrDefault(u => u.Id == UserIdentity.UserId);
            //（使用当前用户的id）获取当前用户，一般非用户界面的获取，而是其他代码的获取，不能获取到时 需要异常处理
            if (user == null)
            {
                _logger.LogError($"错误的用户上下文id{UserIdentity.UserId}");
                throw new Exceptions.UserOperationException($"错误的用户上下文id{UserIdentity.UserId}");
            }
            return Json(user);
        }

        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<AppUser> appUserpatch )
        {
            //TBD handle users.Properties case
            //TBD 记录 ef core sql日志   resource :https://docs.microsoft.com/en-us/ef/core/miscellaneous/logging

            var user = await _userContext.Users
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            appUserpatch.ApplyTo(user);

             await _userContext.SaveChangesAsync();
            return Json(user);
        }

        [HttpPost]
        [Route("tags")]
        public async Task<IActionResult> GetUserTags()
        {
            return Json(await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync());
        }
        /// <summary>
        /// 手机号可以查询，但给一天次数的限制
        /// userid 查找则需要限制，是 内部或外部api 查询时可以，外部api 比如 用户好友api
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search/{phone}")]
        public async Task<IActionResult> Search(string phone)
        {
            return Ok(await _userContext.Users.Include(u => u.properties).SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId));
        }

        //TBD  FromBody 的api 调用问题待解决
        [HttpPut]
        [Route("tags")]
        public async Task<IActionResult> UpdateUserTags([FromBody]List<string> tags)
        {
            var originTags = await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
            var newTags = tags.Except(originTags.Select(t => t.Tag));

            await _userContext.UserTags.AddRangeAsync(newTags.Select(t => new Model.UserTag
            {
                CreateTime = DateTime.Now,
                UserId = UserIdentity.UserId,
                Tag = t
            }));
            await _userContext.SaveChangesAsync();
            return Ok();
        }
    }
}
