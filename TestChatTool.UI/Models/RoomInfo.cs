namespace TestChatTool.UI.Models
{
    public class RoomInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
