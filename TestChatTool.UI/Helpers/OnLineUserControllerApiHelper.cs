using Newtonsoft.Json;
using NLog;
using System;
using System.Net.Http;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Helpers
{
    public class OnLineUserControllerApiHelper : IOnLineUserControllerApiHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly string _serviceUrl;

        public OnLineUserControllerApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = LogManager.GetLogger("ChatToolUI");
            _serviceUrl = ConfigHelper.ServiceUrl;
        }

        public OnLineUserFindRoomUserResponse FindRoomUser(string roomCode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var uri = new Uri($"{_serviceUrl}Online/FindRoomUser?roomCode={roomCode}");

                var response = client.GetAsync(uri).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new OnLineUserFindRoomUserResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<OnLineUserFindRoomUserResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new OnLineUserFindRoomUserResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }
    }
}
