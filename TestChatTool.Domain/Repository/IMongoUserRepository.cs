using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IMongoUserRepository
    {
        (Exception ex, User result) CreateUser(User info);
        (Exception ex, User result) QueryUser(string acc);
        (Exception ex, User result) UpdateUser(User info);
        (Exception ex, bool isSuccess) ResetPwd(string acc, string oldPwd, string newPwd);
    }
}
