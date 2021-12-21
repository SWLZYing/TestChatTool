using NLog;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Domain.Response;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Controllers
{
    [Route("api/User/{Action}")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = LogManager.GetLogger(nameof(UserController));
        }

        [HttpPost]
        public UserCreateResponse Create(UserCreateRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.Password.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(UserController)}.{nameof(Create)} 未輸入帳號/密碼");
                    return new UserCreateResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        IsSuccess = false,
                        ErrorMsg = "帳號/密碼必填",
                    };
                }

                var now = DateTime.Now;

                var result = _userRepository.Create(new User
                {
                    Account = request.Account,
                    Password = request.Password,
                    NickName = request.NickName.IsNullOrWhiteSpace() ? request.Account : request.NickName,
                    CreateDatetime = now,
                    UpdateDatetime = now,
                });

                if (result.ex != null)
                {
                    if (!result.isSuccess && result.isAccDuplicate)
                    {
                        return new UserCreateResponse
                        {
                            Code = (int)ErrorType.AccExist,
                            IsSuccess = result.isSuccess,
                            ErrorMsg = "帳號已存在",
                        };
                    }

                    _logger.Error($"{nameof(UserController)}.{nameof(Create)} Get Exception");
                    return new UserCreateResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        IsSuccess = result.isSuccess,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new UserCreateResponse
                {
                    Code = (int)ErrorType.Success,
                    IsSuccess = result.isSuccess,
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(Create)} Get Exception");
                return new UserCreateResponse
                {
                    Code = (int)ErrorType.SystemError,
                    IsSuccess = false,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public UserQueryResponse Query(string account)
        {
            try
            {
                if (account.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(UserController)}.{nameof(Query)} 未輸入查詢帳號");
                    return new UserQueryResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "未輸入查詢帳號",
                    };
                }

                var result = _userRepository.Query(account);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(UserController)}.{nameof(Query)} Get Exception");
                    return new UserQueryResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.result == null)
                {
                    return new UserQueryResponse
                    {
                        Code = (int)ErrorType.AccError,
                        ErrorMsg = "查無此帳號",
                    };
                }

                return new UserQueryResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.result
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(Query)} Get Exception");
                return new UserQueryResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpPut]
        public UserUpdateResponse Update(UserUpdateRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(UserController)}.{nameof(Update)} 未輸入帳號");
                    return new UserUpdateResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "帳號為必填",
                    };
                }

                var result = _userRepository.Update(new User
                {
                    Account = request.Account,
                    NickName = request.NickName
                });

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(UserController)}.{nameof(Update)} Get Exception");
                    return new UserUpdateResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.result == null)
                {
                    return new UserUpdateResponse
                    {
                        Code = (int)ErrorType.AccError,
                        ErrorMsg = "查無此帳號",
                    };
                }

                return new UserUpdateResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.result
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(Update)} Get Exception");
                return new UserUpdateResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpPut]
        public UserResetPwdResponse ResetPwd(UserResetPwdRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.OldPassWord.IsNullOrWhiteSpace() || request.NewPassWord.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(UserController)}.{nameof(ResetPwd)} 未輸入帳號/密碼");
                    return new UserResetPwdResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        IsSuccess = false,
                        ErrorMsg = "帳號/密碼為必填",
                    };
                }

                var result = _userRepository.ResetPwd(request.Account, request.OldPassWord, request.NewPassWord);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(UserController)}.{nameof(ResetPwd)} Get Exception");
                    return new UserResetPwdResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        IsSuccess = result.isSuccess,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new UserResetPwdResponse
                {
                    IsSuccess = result.isSuccess,
                    Code = result.isSuccess ? (int)ErrorType.Success : (int)ErrorType.Failed,
                    ErrorMsg = result.isSuccess ? default : "密碼變更失敗 請確認帳號/密碼是否正確",
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(ResetPwd)} Get Exception");
                return new UserResetPwdResponse
                {
                    Code = (int)ErrorType.SystemError,
                    IsSuccess = false,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpPut]
        public UserSetErrCountAndStatusResponse SetErrCountAndStatus(UserSetErrCountAndStatusRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(UserController)}.{nameof(SetErrCountAndStatus)} 未輸入帳號");

                    return new UserSetErrCountAndStatusResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        IsSuccess = false,
                        ErrorMsg = "帳號為必填",
                    };
                }

                var result = _userRepository.SetErrCountAndStatus(request.Account, request.ErrorCount, request.Status);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(UserController)}.{nameof(SetErrCountAndStatus)} Get Exception");
                    return new UserSetErrCountAndStatusResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        IsSuccess = result.isSuccess,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new UserSetErrCountAndStatusResponse
                {
                    IsSuccess = result.isSuccess,
                    Code = result.isSuccess ? (int)ErrorType.Success : (int)ErrorType.Failed,
                    ErrorMsg = result.isSuccess ? default : "狀態變更失敗 請確認帳號是否正確",
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(SetErrCountAndStatus)} Get Exception");
                return new UserSetErrCountAndStatusResponse
                {
                    Code = (int)ErrorType.SystemError,
                    IsSuccess = false,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public UserQueryAllForVerifyResponse QueryAllForVerify()
        {
            try
            {
                var result = _userRepository.GetAllForVerify();

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(UserController)}.{nameof(QueryAllForVerify)} Get Exception");
                    return new UserQueryAllForVerifyResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.accs == null)
                {
                    return new UserQueryAllForVerifyResponse
                    {
                        Code = (int)ErrorType.Success,
                        Data = new List<string>(),
                    };
                }

                return new UserQueryAllForVerifyResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.accs
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(QueryAllForVerify)} Get Exception");
                return new UserQueryAllForVerifyResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public UserQueryAllForUnlockResponse QueryAllForUnlock()
        {
            try
            {
                var result = _userRepository.GetAllForUnlock();

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(UserController)}.{nameof(QueryAllForUnlock)} Get Exception");
                    return new UserQueryAllForUnlockResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.accs == null)
                {
                    return new UserQueryAllForUnlockResponse
                    {
                        Code = (int)ErrorType.Success,
                        Data = new List<string>(),
                    };
                }

                return new UserQueryAllForUnlockResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.accs
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserController)}.{nameof(QueryAllForUnlock)} Get Exception");
                return new UserQueryAllForUnlockResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }
    }
}
