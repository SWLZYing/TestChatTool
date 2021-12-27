namespace TestChatTool.Domain.Model
{
    /// <summary>
    /// 通知登出
    /// </summary>
    public class BroadCastLeaveRoomAction : ActionBase
    {
        public override string Action()
            => "BroadCastLeaveRoom";

        /// <summary>
        /// 被登出暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 聊天室編碼
        /// </summary>
        public string RoomCode { get; set; }
    }
}
