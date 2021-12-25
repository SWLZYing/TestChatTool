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

        public UserCreateResponse Create(string account, string password, string nickName)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Account", account },
                    { "Password", password },
                    { "NickName", nickName },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Post, "User/Create")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserCreateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} StatusCode Error" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserCreateResponse>(content);

                if (!result.IsSuccess)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new UserCreateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }

        public UserUpdateResponse Update(string account, string nickName)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Account", account },
                    { "NickName", nickName },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Put, "User/Update")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserUpdateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserUpdateResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new UserUpdateResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }

        public UserResetPwdResponse ResetPwd(string account, string oldPassword, string newPassword)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Account", account },
                    { "OldPassWord", oldPassword },
                    { "NewPassWord", newPassword },
                };

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_serviceUrl);

                var request = new HttpRequestMessage(HttpMethod.Put, "User/ResetPwd")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserResetPwdResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserResetPwdResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new UserResetPwdResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
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

                var request = new HttpRequestMessage(HttpMethod.Put, "User/SetErrCountAndStatus")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserSetErrCountAndStatusResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserSetErrCountAndStatusResponse>(content);

                if (result.Code != (int)ErrorType.Success)
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

        public UserQueryAllForUserStatusResponse QueryAllForUserStatus(UserStatusType userStatus = UserStatusType.Disabled)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var uri = new Uri($"{_serviceUrl}User/QueryAllForUserStatus?status={(int)userStatus}");

                var response = client.GetAsync(uri).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    return new UserQueryAllForUserStatusResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
                }

                //取回傳值
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<UserQueryAllForUserStatusResponse>(content);

                if (result.Code != (int)ErrorType.Success)
                {
                    _logger.Warn(result.ErrorMsg, $"{GetType().Name} Excute Exception");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return new UserQueryAllForUserStatusResponse { Code = (int)ErrorType.SystemError, ErrorMsg = $"{GetType().Name} Excute Exception" };
            }
        }
    }
}
