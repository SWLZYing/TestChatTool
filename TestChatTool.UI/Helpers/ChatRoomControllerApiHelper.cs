﻿using Newtonsoft.Json;
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
    public class ChatRoomControllerApiHelper : IChatRoomControllerApiHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly string _serviceUrl;

        public ChatRoomControllerApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = LogManager.GetLogger("ChatToolUI");
            _serviceUrl = ConfigHelper.ServiceUrl;
        }

        public ChatRoomCreateResponse Create(string code, string name)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Code", code },
                    { "Name", name },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Post, "ChatRoom/Create")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new ChatRoomCreateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} StatusCode Error" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<ChatRoomCreateResponse>(content);

                if (!result.IsSuccess)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new ChatRoomCreateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }

        public ChatRoomUpdateResponse Update(string code, string name)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Code", code },
                    { "Name", name },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Put, "ChatRoom/Update")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new ChatRoomUpdateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<ChatRoomUpdateResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new ChatRoomUpdateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }

        public ChatRoomGetAllResponse GetAll()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var uri = new Uri($"{_serviceUrl}ChatRoom/GetAll");

                var response = client.GetAsync(uri).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new ChatRoomGetAllResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<ChatRoomGetAllResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new ChatRoomGetAllResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }
    }
}
