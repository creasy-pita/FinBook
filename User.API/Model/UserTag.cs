using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Model
{
    public class UserTag
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int AppUserId { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Tag { get; set; }

    }
}
