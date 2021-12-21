using System;
using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IChatRoomRepository
    {
        (Exception ex, bool isSuccess, bool isAccDuplicate) Create(ChatRoom info);
        (Exception ex, ChatRoom result) Query(string code);
        (Exception ex, ChatRoom result) Update(string code, string name);
        (Exception ex, List<(string code, string name)> rooms) GetAll();
    }
}
