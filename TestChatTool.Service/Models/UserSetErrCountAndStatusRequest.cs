using TestChatTool.Domain.Enum;

namespace TestChatTool.Service.Models
{
    public class UserSetErrCountAndStatusRequest
    {
        public string Account { get; set; }
        public int ErrorCount { get; set; } = 0;
        public UserStatusType Status { get; set; } = UserStatusType.Disabled;
    }
}
