using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Services
{
    public class UserServcie: IUserService
    {
        private readonly string userServiceUrl = "http://localhost:56688/";
        private HttpClient _httpClient;

        public UserServcie(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CheckOrCreate(string phone)
        {
            Dictionary<string, string> form = new Dictionary<string, string> { { "phone", phone } };
            var content = new FormUrlEncodedContent(form);

            var response = await _httpClient.PostAsync(userServiceUrl+"api/user/check-or-create", content);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                int.TryParse(result, out int userId);
                return userId;
            }
            return 0;
        }
    }
}
