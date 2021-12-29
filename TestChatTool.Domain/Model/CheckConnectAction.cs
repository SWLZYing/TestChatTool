namespace TestChatTool.Domain.Model
{
    public class CheckConnectAction : ActionBase
    {
        public override string Action()
            => "CheckConnect";

        /// <summary>
        /// 身分
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string NickName { get; set; }
    }
}
