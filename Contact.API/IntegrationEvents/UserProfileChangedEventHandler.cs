using Contact.API.Repositories;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.IntegrationEvents
{
    public class UserProfileChangedEventHandler:ICapSubscribe
    {

        private IContactRepository _repository;

        public UserProfileChangedEventHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        [CapSubscribe("finbook_userapi_userprofilechanged")]
        public async Task UpdateContactInfo(UserProfileChangedEvent @event)
        {
            var token = new CancellationToken();
            var result = await _repository.UpdateContactInfo(new Dto.BaseUserInfo {
                Name = @event.Name,
                Avatar = @event.Avatar,
                Company = @event.Company,
                Title = @event.Title,
                UserId = @event.UserId,
            }, token);
        }
    }
}
