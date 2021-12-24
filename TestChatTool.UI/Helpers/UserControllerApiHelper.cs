using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Helpers
{
    public class UserControllerApiHelper : IUserControllerApiHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly string _serviceUrl;

        public UserControllerApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = LogManager.GetLogger("ChatToolUI");
            _serviceUrl = ConfigHelper.ServiceUrl;
        }

        public UserSetErrCountAndStatusResponse SetErrCountAndStatus(string account, int errorCount, UserStatusType userStatus = UserStatusType.Disabled)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Account", account },
                    { "ErrorCount", errorCount },
                    { "Status", (int)userStatus },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Put, "Admin/Create")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserSetErrCountAndStatusResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} StatusCode Error" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserSetErrCountAndStatusResponse>(content);

                if (!result.IsSuccess)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new UserSetErrCountAndStatusResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }
    }
}
