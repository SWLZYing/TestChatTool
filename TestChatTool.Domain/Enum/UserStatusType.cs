namespace TestChatTool.Domain.Enum
{
    public enum UserStatusType
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// 啟用
        /// </summary>
        Enable = 1,

        /// <summary>
        /// 鎖定
        /// </summary>
        Lock = 2,

        /// <summary>
        /// 解鎖(未重啟)
        /// </summary>
        Unlock = 3,
    }
}
