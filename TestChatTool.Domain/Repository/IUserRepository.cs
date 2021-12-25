using System;
using System.Collections.Generic;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IUserRepository
    {
        (Exception ex, bool isSuccess, bool isAccDuplicate) Create(User info);
        (Exception ex, User user) Query(string acc);
        (Exception ex, User user) Update(User info);

        (Exception ex, bool isSuccess) ResetPwd(string acc, string oldPwd, string newPwd);
        (Exception ex, bool isSuccess) SetErrCountAndStatus(string acc, int errCount, UserStatusType status = UserStatusType.Disabled);
        (Exception ex, User user) SignInRefresh(string acc);
        (Exception ex, IEnumerable<User> users) GetAllForUserStatus(UserStatusType status);
    }
}
