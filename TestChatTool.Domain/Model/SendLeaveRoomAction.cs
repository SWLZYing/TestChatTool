namespace TestChatTool.Domain.Model
{
    public class SendLeaveRoomAction : ActionBase
    {
        public override string Action()
            => "SendLeaveRoom";

        /// <summary>
        /// 聊天室代碼
        /// </summary>
        public string RoomCode { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 被登出暱稱
        /// </summary>
        public string NickName { get; set; }
    }
}
