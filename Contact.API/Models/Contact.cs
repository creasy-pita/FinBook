using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Contact.API.Models
{
    [BsonIgnoreExtraElements]
    public class Contact
    {
        public Contact()
        {
            Tags = new List<string>();
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }

        public List<string> Tags { get; set; }
    }
}