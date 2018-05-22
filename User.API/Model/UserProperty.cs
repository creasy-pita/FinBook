using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Model
{
    public class UserProperty
    {
        /// <summary>
        /// 公司
        /// </summary>
        public int AppUserId { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
}
