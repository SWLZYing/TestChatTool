using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface IUserControllerApiHelper
    {
        UserSetErrCountAndStatusResponse SetErrCountAndStatus(string account, int errorCount, UserStatusType userStatus = UserStatusType.Disabled);
    }
}
