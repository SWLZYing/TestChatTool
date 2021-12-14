using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Repository;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Controllers
{
    public class SignController : ApiController
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _user;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly ILogger _logger;

        public SignController(
            IAdminRepository adminRepository,
            IUserRepository user,
            IChatRoomRepository chatRoomRepository)
        {
            _adminRepository = adminRepository;
            _user = user;
            _chatRoomRepository = chatRoomRepository;
            _logger = LogManager.GetLogger(nameof(SignController));
        }

        [HttpPost]
        public HttpResponseMessage AdminSignIn([FromBody] AdminSignInRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.Password.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(AdminSignIn)} 未輸入帳號/密碼");

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("帳號/密碼必填")
                    };
                }

                var result = _adminRepository.Query(request.Account);

                if (result.ex != null)
                {
                    throw result.ex;
                }

                if (result.result == null)
                {
                    _logger.Warn($"{nameof(AdminSignIn)} 查無此帳號");

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("帳號/密碼錯誤")
                    };
                }

                if (result.result.Password != request.Password.ToMD5())
                {
                    _logger.Warn($"{nameof(AdminSignIn)} 密碼錯誤");

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("帳號/密碼錯誤")
                    };
                }

                // 更新登入時間
                // todo

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(result.result.ToString())
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AdminSignIn)} Get Exception");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
