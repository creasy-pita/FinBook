using Contact.API.Repositories;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
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
        private ILogger<UserProfileChangedEventHandler> _logger;

        public UserProfileChangedEventHandler(IContactRepository repository, ILogger<UserProfileChangedEventHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [CapSubscribe("finbook_userapi_userprofilechanged")]
        public async Task UpdateContactInfo(UserProfileChangedEvent @event)
        {
            var token = new CancellationToken();
            var result = await _repository.UpdateContactInfo(new Dto.BaseUserInfo
            {
                Name = @event.Name,
                Avatar = @event.Avatar,
                Company = @event.Company,
                Title = @event.Title,
                UserId = @event.UserId,
            }, token);
        }
    }
}
