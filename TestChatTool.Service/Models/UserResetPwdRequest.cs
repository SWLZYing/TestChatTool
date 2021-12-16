namespace TestChatTool.Service.Models
{
    public class UserResetPwdRequest
    {
        public string Account { get; set; }
        public string OldPassWord { get; set; }
        public string NewPassWord { get; set; }
    }
}
