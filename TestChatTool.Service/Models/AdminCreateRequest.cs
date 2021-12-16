using TestChatTool.Domain.Enum;

namespace TestChatTool.Service.Models
{
    public class AdminCreateRequest
    {
        public string Account { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 使用者類型 1-Admin 2-Normal
        /// </summary>
        public AdminType AccountType { get; set; }
    }
}
