using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Dto;
using Contact.API.Models;
using MongoDB.Driver;
namespace Contact.API.Repositories
{
    public class MongoContactRepository : IContactRepository
    {

        private ContactContext _context;

        public MongoContactRepository(ContactContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 用户添加联系人
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="contact">待添加的好友信息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> AddContactAsync(int userId,BaseUserInfo contact,CancellationToken cancellationToken)
        {
            //查看是否用户 有通讯录 contactbook
            if ((await _context.ContactBooks.CountAsync(c => c.UserId == userId)) == 0)
            {
                await _context.ContactBooks.InsertOneAsync(new ContactBook { UserId = userId });
            }
            //ContactBook 加入一条好友信息
            var filter = Builders<Models.ContactBook>.Filter.Eq(c => c.UserId ,contact.UserId);
            var update = Builders<Models.ContactBook>.Update.AddToSet(c => c.Contacts, new Models.Contact
            {
                Avatar = contact.Avatar,
                Company = contact.Company,
                Name = contact.Name,
                Title = contact.Title,
                UserId = contact.UserId
            });
            var result = (await _context.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken));
            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }

        /// <summary>
        /// 通讯录好友信息更新， 则更新 所有用户的通讯录下有该好友的相应信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateContactInfo(BaseUserInfo userInfo, CancellationToken cancellationToken)
        {
            try
            {
                //查找该用户的通讯录，如果没没有通讯录则返回
                var contactBook = (await _context.ContactBooks.FindAsync(c => c.UserId == userInfo.UserId, null, cancellationToken)).FirstOrDefault(cancellationToken);//.ToList(cancellationToken);
                if (contactBook == null)
                {
                    return true;
                }
                //取出该用户的 所有联系人的id
                var contactIds = contactBook.Contacts.Select(c => c.UserId);
                //有通讯录，用mongodb的关联内部查询方式匹配
                //定义 filterdifinition 用and 条件连接符
                //所有联系人的contactBook 
                //contackbook 中的 contact.UserId==userInfo.UserId 
                var filter = Builders<ContactBook>.Filter.And(
                        Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                        Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, contact => contact.UserId == userInfo.UserId)
                    );
                //定义 updatedefinition
                var update = Builders<ContactBook>.Update
                    .Set("Contacts.$.Name", userInfo.Name)
                    .Set("Contacts.$.Avatar", userInfo.Avatar)
                    .Set("Contacts.$.Company", userInfo.Company)
                    .Set("Contacts.$.Title", userInfo.Title);
                var updateResult = _context.ContactBooks.UpdateMany(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
