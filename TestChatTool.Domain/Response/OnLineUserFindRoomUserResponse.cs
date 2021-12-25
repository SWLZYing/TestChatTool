using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class OnLineUserFindRoomUserResponse : BasicResponse
    {
        public IEnumerable<OnLineUser> Users { get; set; }
    }
}
