using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class OnLineUserFindRoomUserResponse : BasicResponse
    {
        public List<OnLineUser> Data { get; set; }
    }
}
