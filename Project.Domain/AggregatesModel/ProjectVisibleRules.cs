using Project.Domain.SeedWork;
using System;

namespace Project.Domain.AggregatesModel
{
    public class ProjectVisibleRules:Entity
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 部分可见 时 标签设置
        /// </summary>
        public string Tags { get; set; }
    }
}
