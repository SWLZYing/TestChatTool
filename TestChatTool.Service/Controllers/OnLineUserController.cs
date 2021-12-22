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
    [Route("api/Online/{Action}")]
    public class OnLineUserController : ApiController
    {
        private readonly IOnLineUserRepository _onLineUserRepository;
        private readonly ILogger _logger;

        public OnLineUserController(IOnLineUserRepository onLineUserRepository)
        {
            _onLineUserRepository = onLineUserRepository;
            _logger = LogManager.GetLogger(nameof(OnLineUserController));
        }

        [HttpPost]
        public OnLineUserUpsertResponse Upsert(OnLineUserUpsertRequest request)
        {
            try
            {
                if (request.Account.IsNullOrWhiteSpace() || request.RoomCode.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(OnLineUserController)}.{nameof(Upsert)} 未輸入帳號/房間代碼");
                    return new OnLineUserUpsertResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "未輸入帳號/房間代碼",
                    };
                }

                var result = _onLineUserRepository.Upsert(new OnLineUser
                {
                    Account = request.Account,
                    NickName = request.NickName.IsNullOrWhiteSpace() ? request.Account : request.NickName,
                    RoomCode = request.RoomCode == "SignOut" ? string.Empty : request.RoomCode
                });

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(OnLineUserController)}.{nameof(Upsert)} Get Exception");
                    return new OnLineUserUpsertResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new OnLineUserUpsertResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.result
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(OnLineUserController)}.{nameof(Upsert)} Get Exception");
                return new OnLineUserUpsertResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public OnLineUserFindRoomUserResponse FindRoomUser(string roomCode)
        {
            try
            {
                if (roomCode.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(OnLineUserController)}.{nameof(Upsert)} 未輸入房間代碼");
                    return new OnLineUserFindRoomUserResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "未輸入房間代碼",
                    };
                }

                var result = _onLineUserRepository.FindRoomUser(roomCode);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(OnLineUserController)}.{nameof(FindRoomUser)} Get Exception");
                    return new OnLineUserFindRoomUserResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new OnLineUserFindRoomUserResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.result
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(OnLineUserController)}.{nameof(FindRoomUser)} Get Exception");
                return new OnLineUserFindRoomUserResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public OnLineUserFindAllUserCountByRoomResponse FindAllUserCountByRoom()
        {
            try
            {
                var result = _onLineUserRepository.FindAllUserCountByRoom();

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(OnLineUserController)}.{nameof(FindAllUserCountByRoom)} Get Exception");
                    return new OnLineUserFindAllUserCountByRoomResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new OnLineUserFindAllUserCountByRoomResponse
                {
                    Code = (int)ErrorType.Success,
                    Data = result.result
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(OnLineUserController)}.{nameof(FindAllUserCountByRoom)} Get Exception");
                return new OnLineUserFindAllUserCountByRoomResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }
    }
}
