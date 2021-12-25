using NLog;
using System;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class RoomMaintain : Form
    {
        private readonly IChatRoomControllerApiHelper _chatRoomControllerApi;
        private readonly ILogger _logger;

        public RoomMaintain(IChatRoomControllerApiHelper chatRoomControllerApi)
        {
            InitializeComponent();
            MaximizeBox = false;

            _chatRoomControllerApi = chatRoomControllerApi;
            _logger = LogManager.GetLogger("UIRoomMaintain");
        }

        internal void RefreshView()
        {
            txtCode.Clear();
            txtName.Clear();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;

                switch (btn.Name)
                {
                    case "btnCreate":
                        CreateRoom();
                        break;
                    case "btnUpdate":
                        UpdateRoom();
                        break;
                    default:
                        MessageBox.Show("無效的選項");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ButtonClick Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateRoom()
        {
            var user = _chatRoomControllerApi.Update(txtCode.Text, txtName.Text);

            if (user.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("更新成功");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
                return;
            }
        }

        private void CreateRoom()
        {
            var user = _chatRoomControllerApi.Create(txtCode.Text, txtName.Text);

            if (user.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("創建成功");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
                return;
            }
        }
    }
}
