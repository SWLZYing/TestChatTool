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
    [Route("api/ChatRoom/{Action}")]
    public class ChatRoomController : ApiController
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly ILogger _logger;

        public ChatRoomController(IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _logger = LogManager.GetLogger(nameof(ChatRoomController));
        }

        [HttpPost]
        public ChatRoomCreateResponse Create(ChatRoomCreateRequest request)
        {
            try
            {
                if (request.Code.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(ChatRoomController)}.{nameof(Create)} 房間代碼為必填");
                    return new ChatRoomCreateResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        IsSuccess = false,
                        ErrorMsg = "房間代碼為必填",
                    };
                }

                var result = _chatRoomRepository.Create(new ChatRoom
                {
                    Code = request.Code,
                    Name = request.Name.IsNullOrWhiteSpace() ? request.Code : request.Name,
                });

                if (result.ex != null)
                {
                    if (!result.isSuccess && result.isAccDuplicate)
                    {
                        return new ChatRoomCreateResponse
                        {
                            Code = (int)ErrorType.AccExist,
                            IsSuccess = result.isSuccess,
                            ErrorMsg = "聊天室已存在",
                        };
                    }

                    _logger.Error($"{nameof(ChatRoomController)}.{nameof(Create)} Get Exception");
                    return new ChatRoomCreateResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        IsSuccess = result.isSuccess,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new ChatRoomCreateResponse
                {
                    Code = (int)ErrorType.Success,
                    IsSuccess = result.isSuccess,
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(ChatRoomController)}.{nameof(Create)} Get Exception");
                return new ChatRoomCreateResponse
                {
                    Code = (int)ErrorType.SystemError,
                    IsSuccess = false,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public ChatRoomQueryResponse Query(string code)
        {
            try
            {
                if (code.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(ChatRoomController)}.{nameof(Query)} 未輸入查詢房間代碼");
                    return new ChatRoomQueryResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "未輸入查詢房間代碼",
                    };
                }

                var result = _chatRoomRepository.Query(code);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(ChatRoomController)}.{nameof(Query)} Get Exception");
                    return new ChatRoomQueryResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.room == null)
                {
                    return new ChatRoomQueryResponse
                    {
                        Code = (int)ErrorType.AccError,
                        ErrorMsg = "聊天室不存在",
                    };
                }

                return new ChatRoomQueryResponse
                {
                    Code = (int)ErrorType.Success,
                    Room = result.room
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(ChatRoomController)}.{nameof(Query)} Get Exception");
                return new ChatRoomQueryResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpPut]
        public ChatRoomUpdateResponse Update(ChatRoomUpdateRequest request)
        {
            try
            {
                if (request.Code.IsNullOrWhiteSpace() || request.Name.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(ChatRoomController)}.{nameof(Update)} 房間代碼/名稱必填");
                    return new ChatRoomUpdateResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        ErrorMsg = "房間代碼/名稱必填",
                    };
                }

                var result = _chatRoomRepository.Update(request.Code, request.Name);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(ChatRoomController)}.{nameof(Update)} Get Exception");
                    return new ChatRoomUpdateResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.room == null)
                {
                    return new ChatRoomUpdateResponse
                    {
                        Code = (int)ErrorType.AccError,
                        ErrorMsg = "指定聊天室不存在",
                    };
                }

                return new ChatRoomUpdateResponse
                {
                    Code = (int)ErrorType.Success,
                    Room = result.room
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(ChatRoomController)}.{nameof(Update)} Get Exception");
                return new ChatRoomUpdateResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpDelete]
        public ChatRoomDeleteResponse Delete(string roomCode)
        {
            try
            {
                if (roomCode.IsNullOrWhiteSpace())
                {
                    _logger.Warn($"{nameof(ChatRoomController)}.{nameof(Delete)} 房間代碼必填");
                    return new ChatRoomDeleteResponse
                    {
                        Code = (int)ErrorType.FieldNull,
                        IsSuccess = false,
                        ErrorMsg = "房間代碼為必填",
                    };
                }

                var result = _chatRoomRepository.Delete(roomCode);

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(ChatRoomController)}.{nameof(Delete)} Get Exception");
                    return new ChatRoomDeleteResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        IsSuccess = false,
                        ErrorMsg = result.ex.Message,
                    };
                }

                return new ChatRoomDeleteResponse
                {
                    Code = (int)ErrorType.Success,
                    IsSuccess = result.isSuccess,
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(ChatRoomController)}.{nameof(Delete)} Get Exception");
                return new ChatRoomDeleteResponse
                {
                    Code = (int)ErrorType.SystemError,
                    IsSuccess = false,
                    ErrorMsg = ex.Message,
                };
            }
        }

        [HttpGet]
        public ChatRoomGetAllResponse GetAll()
        {
            try
            {
                var result = _chatRoomRepository.GetAll();

                if (result.ex != null)
                {
                    _logger.Error($"{nameof(ChatRoomController)}.{nameof(GetAll)} Get Exception");
                    return new ChatRoomGetAllResponse
                    {
                        Code = (int)ErrorType.SystemError,
                        ErrorMsg = result.ex.Message,
                    };
                }

                if (result.rooms == null)
                {
                    return new ChatRoomGetAllResponse
                    {
                        Code = (int)ErrorType.Success,
                        Rooms = new List<ChatRoom>(),
                    };
                }

                return new ChatRoomGetAllResponse
                {
                    Code = (int)ErrorType.Success,
                    Rooms = result.rooms,
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(ChatRoomController)}.{nameof(GetAll)} Get Exception");
                return new ChatRoomGetAllResponse
                {
                    Code = (int)ErrorType.SystemError,
                    ErrorMsg = ex.Message,
                };
            }
        }
    }
}
