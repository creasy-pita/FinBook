using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recommend.API.Data;

namespace Recommend.API.Controllers
{
    [Route("api/[controller]")]
    public class RecommendController : BaseController
    {
        private RecommendDbContext _recommendDbContext;

        public RecommendController(RecommendDbContext recommendDbContext)
        {
            _recommendDbContext = recommendDbContext;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult>  Get()
        {
            return Ok(await _recommendDbContext.ProjectRecommends.AsNoTracking()
                .Where(r => r.UserId == this.UserIdentity.UserId).ToListAsync());
        }

    }
}
