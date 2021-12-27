namespace TestChatTool.Domain.Model
{
    public class BroadCastEnterRoomAction : ActionBase
    {
        public override string Action()
            => "BroadCastEnterRoom";

        /// <summary>
        /// 登入暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 聊天室編碼
        /// </summary>
        public string RoomCode { get; set; }
    }
}
