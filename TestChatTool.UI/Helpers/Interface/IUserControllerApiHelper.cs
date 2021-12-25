using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface IUserControllerApiHelper
    {
        UserCreateResponse Create(string account, string password, string nickName);
        UserUpdateResponse Update(string account, string nickName);
        UserResetPwdResponse ResetPwd(string account, string oldPassword, string newPassword);
        UserSetErrCountAndStatusResponse SetErrCountAndStatus(string account, int errorCount, UserStatusType userStatus = UserStatusType.Disabled);
        UserQueryAllForUserStatusResponse QueryAllForUserStatus(UserStatusType userStatus = UserStatusType.Disabled);
    }
}
