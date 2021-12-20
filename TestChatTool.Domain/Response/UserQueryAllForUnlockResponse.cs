using System.Collections.Generic;

namespace TestChatTool.Domain.Response
{
    public class UserQueryAllForUnlockResponse : BasicResponse
    {
        public List<string> Data { get; set; }
    }
}
