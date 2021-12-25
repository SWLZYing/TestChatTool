using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class UserQueryAllForUserStatusResponse : BasicResponse
    {
        public IEnumerable<User> Users { get; set; }
    }
}
