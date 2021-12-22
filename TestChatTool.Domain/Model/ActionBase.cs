using Newtonsoft.Json;

namespace TestChatTool.Domain.Model
{
    /// <summary>
    /// Action抽象類別
    /// </summary>
    public abstract class ActionBase
    {
        /// <summary>
        /// 指令
        /// </summary>
        /// <returns></returns>
        public abstract string Action();

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
