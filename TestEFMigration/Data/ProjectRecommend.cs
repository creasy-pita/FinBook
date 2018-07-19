using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestEFMigration.Data;

namespace Recommend.API.Model
{
    public class ProjectRecommend
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 所属用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 来源用户id
        /// </summary>
        public int FromUserId { get; set; }
        /// <summary>
        /// 来源用户名
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string FromUserAvatar { get; set; }

        public EnumRecommendType RecommednType { set; get; }
        //TBD  MySql.Data.EntityFrameworkCore 版本问题， 
        //此版本迁移时 MySQLNumberTypeMapping 不支持enum 需解决，临时用int类型
        //public int RecommednType { get; set; }

        public int ProjectId { get; set; }

        public string ProjectAvatar { get; set; }

        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Tags { get; set; }

        public string FinStage { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime RecommendTime { get; set; }

        //public List<string> ReferenceUsers { get; set; }


    }
}
