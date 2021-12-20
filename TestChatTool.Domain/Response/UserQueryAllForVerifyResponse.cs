using System.Collections.Generic;

namespace TestChatTool.Domain.Response
{
    public class UserQueryAllForVerifyResponse : BasicResponse
    {
        public List<string> Data { get; set; }
    }
}
