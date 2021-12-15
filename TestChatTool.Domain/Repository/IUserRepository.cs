using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IUserRepository
    {
        (Exception ex, User result) Create(User info);
        (Exception ex, User result) Query(string acc);
        (Exception ex, User result) Update(User info);

        (Exception ex, bool isSuccess) ResetPwd(string acc, string oldPwd, string newPwd);
        (Exception ex, bool isSuccess) SetErrCountAndStatus(string acc, int errCount, int status = 0);
        (Exception ex, User result) SignInRefresh(string acc);
    }
}
