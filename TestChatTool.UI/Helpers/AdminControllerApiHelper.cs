using Newtonsoft.Json;
using NLog;
using System;
using System.Net.Http;
using System.Text;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Helpers
{
    public class AdminControllerApiHelper : IAdminControllerApiHelper
    {
        private readonly string _serviceUrl;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public AdminControllerApiHelper(
            string serviceUrl,
            IHttpClientFactory httpClientFactory)
        {
            _serviceUrl = serviceUrl;
            _httpClientFactory = httpClientFactory;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public AdminCreateResponse Create(string account, string password, AdminType adminType)
        {
            try
            {
                var parameters = new
                {
                    Account = account,
                    Password = password,
                    AccountType = adminType,
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Post, "Admin/Create")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new AdminCreateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} StatusCode Error" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<AdminCreateResponse>(content);

                if (!result.IsSuccess)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new AdminCreateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }
    }
}
