using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dto;

namespace Contact.API.Repositories
{
    public class MongoContactRepository : IContactRepository
    {
        /// <summary>
        /// 通讯录好友信息更新， 则更新 所有用户的通讯录下有该好友的相应信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> UpdateContactInfo(BaseUserInfo userInfo, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
