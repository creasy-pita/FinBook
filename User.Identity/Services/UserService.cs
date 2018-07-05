using BuildingBlocks.Resilience.Http;
using DnsClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using User.API.Identity.Dto;

namespace User.Identity.Services
{
    public class UserServcie: IUserService
    {
        private string userServiceUrl = "http://localhost:56688/";
        //private HttpClient _httpClient;
        private IHttpClient _httpClient;
        private IDnsQuery _dns;
        private IOptions<ServiceDisvoveryOptions> _options;

        public UserServcie(IHttpClient httpClient,IDnsQuery dns, IOptions<ServiceDisvoveryOptions> options)
        {
            _dns = dns ?? throw new ArgumentNullException(nameof(dns));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient;
        }

        public async Task<int> CheckOrCreate(string phone)
        {
            var result = await _dns.ResolveServiceAsync("service.consul", _options.Value.ServiceName);

            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName.TrimEnd('.');
            var port = result.First().Port;
            userServiceUrl = $"http://{address}:{port}/";
            

            Dictionary<string, string> form = new Dictionary<string, string> { { "phone", phone } };
            var content = new FormUrlEncodedContent(form);

            var response = await _httpClient.PostAsync(userServiceUrl+"api/user/check-or-create", content);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result1 = await response.Content.ReadAsStringAsync();
                int.TryParse(result1, out int userId);
                return userId;
            }
            return 0;
        }
    }
}
