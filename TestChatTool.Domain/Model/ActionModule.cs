namespace TestChatTool.Domain.Model
{
    /// <summary>
    /// 長連結通訊結構
    /// </summary>
    public class ActionModule
    {
        /// <summary>
        /// 指令
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 指令內容
        /// </summary>
        public string Content { get; set; }
    }
}
