using System;
using System.Collections.Generic;

namespace Contact.API.Models
{
    public class ContactApplyRequest
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 申请人用户id
        /// </summary>
        public int ApplierId { get; set; }
        /// <summary>
        /// 是否通过 0 未通过 1 已通过
        /// </summary>
        public int Approvaled { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}