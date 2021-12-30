namespace TestChatTool.Domain.Model
{
    public class CheckConnectAction : ActionBase
    {
        public override string Action()
            => "CheckConnect";

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 聊天室代碼
        /// </summary>
        public string RoomCode { get; set; }
    }
}
