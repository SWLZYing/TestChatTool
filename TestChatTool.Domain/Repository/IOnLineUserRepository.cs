using System;
using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IOnLineUserRepository
    {
        (Exception ex, OnLineUser user) Upsert(OnLineUser info);

        (Exception ex, IEnumerable<OnLineUser> users) FindRoomUser(string code);
        (Exception ex, IEnumerable<(string roomCode, int userCount)> result) FindAllUserCountByRoom();
    }
}
