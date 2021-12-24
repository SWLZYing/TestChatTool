using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface IAdminControllerApiHelper
    {
        AdminCreateResponse Create(string account, string password, AdminType adminType);
    }
}