using NLog;
using System;
using System.Web.Http;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Domain.Response;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Controllers
{
    [Route("api/Admin/{Action}")]

    public class AdminController : ApiController
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger _logger;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
            _logger = LogManager.GetLogger(nameof(AdminController));
        }

        [HttpPost]
        public AdminCreateResponse Create(AdminCreateRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.Password.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(AdminController)}.{nameof(Create)} 未輸入帳號/密碼");
                    return new AdminCreateResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        IsSuccess = false,
                        ErrorMsg = "帳號/密碼必填",
                    };
                }

                var now = DateTime.Now;

                var result = _adminRepository.Create(new Admin
                {
                    Account = request.Account,
                    Password = request.Password,
                    AccountType = request.AccountType,
                    CreateDatetime = now,
                    UpdateDatetime = now,
                });

                if (result.ex != null)
                {
                    if (!result.isSuccess && result.isAccDuplicate)
                    {
                        return new AdminCreateResponse
                        {
                            Code = (int)ErrorType.AccExist,
                            IsSuccess = result.isSuccess,
                            ErrorMsg = "帳號已存在",
                        };
                    }

                    _logger.Error($"{nameof(AdminController)}.{nameof(Create)} Get Exception");
                    return new AdminCreateResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        IsSuccess = result.isSuccess,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new AdminCreateResponse
                {
                    Code = (int)ErrorType.Success,
                    IsSuccess = result.isSuccess,
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AdminController)}.{nameof(Create)} Get Exception");
                return new AdminCreateResponse
                {
                    Code = (int)ErrorType.SystemError,
                    IsSuccess = false,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public AdminQueryResponse Query(string account)
        {
            try
            {
                if (account.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(AdminController)}.{nameof(Query)} 未輸入查詢帳號");

                    return new AdminQueryResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "未輸入查詢帳號",
                    };
                }

                var result = _adminRepository.Query(account);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(AdminController)}.{nameof(Query)} Get Exception");
                    return new AdminQueryResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.result == null)
                {
                    return new AdminQueryResponse
                    {
                        Code = (int)ErrorType.AccError,
                        ErrorMsg = "查無此帳號",
                    };
                }

                return new AdminQueryResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.result
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AdminController)}.{nameof(Query)} Get Exception");
                return new AdminQueryResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }
    }
}
