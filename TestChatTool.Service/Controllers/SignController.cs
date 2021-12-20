using NLog;
using System;
using System.Web.Http;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Repository;
using TestChatTool.Domain.Response;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Controllers
{
    [Route("api/Sign/{Action}")]
    public class SignController : ApiController
    {
        private static object _signLock = new object();

        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public SignController(
            IAdminRepository adminRepository,
            IUserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _logger = LogManager.GetLogger(nameof(SignController));
        }

        [HttpPost]
        public AdminSignInResponse AdminSignIn(SignInRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.Password.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(AdminSignIn)} 未輸入帳號/密碼");
                    return new AdminSignInResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "帳號/密碼必填",
                    };
                }

                _signLock = request.Account;
                lock (_signLock)
                {
                    var result = _adminRepository.Query(request.Account);

                    if (result.ex != null)
                    {
                        _logger.Error($"{nameof(AdminSignIn)} Get Exception");
                        return new AdminSignInResponse
                        {
                            Code = (int)ErrorType.SystemError,
                            ErrorMsg = result.ex.Message,
                        };
                    }

                    if (result.result == null)
                    {
                        _logger.Warn($"{nameof(AdminSignIn)} 查無此帳號");
                        return new AdminSignInResponse
                        {
                            Code = (int)ErrorType.AccError,
                            ErrorMsg = "查無此帳號",
                        };
                    }

                    if (result.result.Password != request.Password.ToMD5())
                    {
                        _logger.Warn($"{nameof(AdminSignIn)} 密碼錯誤");
                        return new AdminSignInResponse
                        {
                            Code = (int)ErrorType.PwdError,
                            ErrorMsg = "密碼錯誤",
                        };
                    }

                    return new AdminSignInResponse
                    {
                        Code = (int)ErrorType.Success,
                        Data = result.result,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AdminSignIn)} Get Exception");
                return new AdminSignInResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpPost]
        public UserSignInResponse UserSignIn(SignInRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.Password.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(UserSignIn)} 未輸入帳號/密碼");
                    return new UserSignInResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "帳號/密碼必填",
                    };
                }

                _signLock = request.Account;
                lock (_signLock)
                {
                    var query = _userRepository.Query(request.Account);

                    if (query.ex != null)
                    {
                        _logger.Error($"{nameof(UserSignIn)} Get Exception");
                        return new UserSignInResponse
                        {
                            Code = (int)ErrorType.SystemError,
                            ErrorMsg = query.ex.Message,
                        };
                    }

                    if (query.result == null)
                    {
                        _logger.Warn($"{nameof(UserSignIn)} 查無此帳號");
                        return new UserSignInResponse
                        {
                            Code = (int)ErrorType.AccError,
                            ErrorMsg = "查無此帳號",
                        };
                    }

                    // 帳號狀態 未啟用/鎖定
                    if (query.result.Status == UserStatusType.Disabled || query.result.Status == UserStatusType.Lock)
                    {
                        return new UserSignInResponse
                        {
                            Code = query.result.Status == 0 ? (int)ErrorType.AccNotVerify : (int)ErrorType.AccLock,
                            ErrorMsg = query.result.Status == 0 ? "帳號待審核" : "帳號鎖定中 請聯繫管理員解鎖",
                        };
                    }

                    // 帳號狀態 啟用/解鎖(未更換密碼)
                    // 密碼錯誤
                    if (query.result.Password != request.Password.ToMD5())
                    {
                        var errCount = query.result.ErrCount + 1;

                        // 更新錯誤次數 連續錯誤三次帳號鎖定
                        var err = errCount < 3
                            ? _userRepository.SetErrCountAndStatus(query.result.Account, errCount)
                            : _userRepository.SetErrCountAndStatus(query.result.Account, errCount, UserStatusType.Lock);

                        if (err.ex != null)
                        {
                            _logger.Error($"{nameof(UserSignIn)} Get Exception");
                            return new UserSignInResponse
                            {
                                Code = (int)ErrorType.SystemError,
                                ErrorMsg = err.ex.Message,
                            };
                        }

                        _logger.Warn($"{nameof(UserSignIn)} 密碼錯誤 次數:{errCount}");
                        return new UserSignInResponse
                        {
                            Code = errCount < 3 ? (int)ErrorType.PwdError : (int)ErrorType.PwdErrorToLock,
                            ErrorMsg = errCount < 3 ? "密碼錯誤" : "密碼錯誤三次 帳號已鎖定",
                        };
                    }

                    // 密碼正確
                    var signIn = _userRepository.SignInRefresh(query.result.Account);

                    if (signIn.ex != null)
                    {
                        _logger.Error($"{nameof(UserSignIn)} Get Exception");
                        return new UserSignInResponse
                        {
                            Code = (int)ErrorType.SystemError,
                            ErrorMsg = signIn.ex.Message,
                        };
                    }

                    return new UserSignInResponse
                    {
                        Code = (int)ErrorType.Success,
                        Data = signIn.result,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UserSignIn)} Get Exception");
                return new UserSignInResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }
    }
}
