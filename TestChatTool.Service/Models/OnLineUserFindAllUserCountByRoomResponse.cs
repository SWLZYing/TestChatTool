using System.Collections.Generic;

namespace TestChatTool.Service.Models
{
    public class OnLineUserFindAllUserCountByRoomResponse : BasicResponse
    {
        public List<(string, int)> Data { get; set; }
    }
}
