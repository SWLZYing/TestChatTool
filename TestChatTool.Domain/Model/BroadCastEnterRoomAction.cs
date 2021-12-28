namespace TestChatTool.Domain.Model
{
    public class BroadCastEnterRoomAction : ActionBase
    {
        public override string Action()
            => "BroadCastEnterRoom";

        /// <summary>
        /// 聊天室代碼
        /// </summary>
        public string RoomCode { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登入暱稱
        /// </summary>
        public string NickName { get; set; }
    }
}
