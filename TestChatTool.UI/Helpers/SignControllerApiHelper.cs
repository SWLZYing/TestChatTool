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
    public class SignControllerApiHelper : ISignControllerApiHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly string _serviceUrl;

        public SignControllerApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = LogManager.GetLogger("ChatToolUI");
            _serviceUrl = ConfigHelper.ServiceUrl;
        }

        public AdminSignInResponse AdminSignIn(string account, string password)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Account", account },
                    { "Password", password },
                };
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Post, "Sign/AdminSignIn")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new AdminSignInResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} StatusCode Error" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<AdminSignInResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new AdminSignInResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }

        public UserSignInResponse UserSignIn(string account, string password)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Account", account },
                    { "Password", password },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Post, "Sign/UserSignIn")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserSignInResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} StatusCode Error" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserSignInResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new UserSignInResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }
    }
}
