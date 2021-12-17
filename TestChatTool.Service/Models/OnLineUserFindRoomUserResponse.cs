using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class OnLineUserFindRoomUserResponse : BasicResponse
    {
        public List<OnLineUser> Data { get; set; }
    }
}
