using Project.Domain.SeedWork;
using System;

namespace Project.Domain.AggregatesModel
{
    public class ProjectContributor : Entity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 是否介结束方
        /// </summary>
        public bool IsCloser { get; set; }

        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 投资类型 1 财务顾问 2 投资机构
        /// </summary>
        public int ContributorType { get; set; }
    }
}
