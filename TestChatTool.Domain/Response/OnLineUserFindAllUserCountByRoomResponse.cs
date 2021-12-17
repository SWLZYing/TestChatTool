using System.Collections.Generic;

namespace TestChatTool.Domain.Response
{
    public class OnLineUserFindAllUserCountByRoomResponse : BasicResponse
    {
        public List<(string, int)> Data { get; set; }
    }
}
