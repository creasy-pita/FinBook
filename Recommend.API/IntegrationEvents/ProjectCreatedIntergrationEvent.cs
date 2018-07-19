using System;
using System.Collections.Generic;
using System.Text;
namespace Recommend.API.IntegrationEvents
{
    public class ProjectCreatedIntergrationEvent
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Company { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public int FinStage { get; set; }
        /// <summary>
        /// 项目基本介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 项目logo
        /// </summary>
        public string ProjectAvatar { get; set; }

        public string Tags { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
