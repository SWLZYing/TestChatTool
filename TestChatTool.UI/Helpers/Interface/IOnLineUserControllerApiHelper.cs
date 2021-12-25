using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface IOnLineUserControllerApiHelper
    {
        OnLineUserFindRoomUserResponse FindRoomUser(string roomCode);
    }
}
