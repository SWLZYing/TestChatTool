using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface IChatRoomControllerApiHelper
    {
        ChatRoomCreateResponse Create(string code, string name);
        ChatRoomUpdateResponse Update(string code, string name);
        ChatRoomDeleteResponse Delete(string code);
        ChatRoomGetAllResponse GetAll();
    }
}
