using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Helpers.Interface;
using TestChatTool.UI.Models;
using TestChatTool.UI.SignalR.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class RoomMaintain : Form
    {
        private readonly IChatRoomControllerApiHelper _chatRoomControllerApi;
        private readonly IHubClient _hubClient;
        private readonly ILogger _logger;
        private string _btnName;
        private IEnumerable<RoomInfo> _rooms;

        public IEnumerable<RoomInfo> Rooms
        {
            set
            {
                _rooms = value;
            }
        }

        public RoomMaintain(
            IChatRoomControllerApiHelper chatRoomControllerApi,
            IHubClient hubClient)
        {
            InitializeComponent();
            MaximizeBox = false;

            _chatRoomControllerApi = chatRoomControllerApi;
            _hubClient = hubClient;
            _logger = LogManager.GetLogger("UIRoomMaintain");
        }

        public void SetUpUI(string buttonName)
        {
            _btnName = buttonName;

            txtCode.Clear();
            txtName.Clear();
            cbbRoom.Items.Clear();

            switch (buttonName)
            {
                case "btnRoomCreate":

                    lblTitle.Text = "聊天室新增";
                    txtCode.Show();
                    cbbRoom.Hide();
                    txtName.Enabled = true;
                    break;

                case "btnRoomUpdate":

                    GetAllRoom();
                    lblTitle.Text = "聊天室修改";
                    txtCode.Hide();
                    cbbRoom.Show();
                    txtName.Enabled = true;
                    break;

                case "btnRoomDelete":

                    GetAllRoom();
                    lblTitle.Text = "聊天室刪除";
                    txtCode.Hide();
                    cbbRoom.Show();
                    txtName.Enabled = false;
                    break;

                default:
                    break;
            }
        }

        private void SelectedValueChanged(object sender, EventArgs e)
        {
            txtName.Text = _rooms.FirstOrDefault(s => s.Code == cbbRoom.Text).Name;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                switch (_btnName)
                {
                    case "btnRoomCreate":
                        CreateRoom();
                        break;
                    case "btnRoomUpdate":
                        UpdateRoom();
                        break;
                    case "btnRoomDelete":
                        DeleteRoom();
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

        private void DeleteRoom()
        {
            var user = _chatRoomControllerApi.Delete(cbbRoom.Text);

            if (user.Code == (int)ErrorType.Success)
            {
                _hubClient.SendAction(new SendDeleteChatRoomAction());
                MessageBox.Show("刪除成功");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
                return;
            }
        }

        private void UpdateRoom()
        {
            var user = _chatRoomControllerApi.Update(cbbRoom.Text, txtName.Text);

            if (user.Code == (int)ErrorType.Success)
            {
                _hubClient.SendAction(new SendUpsertChatRoomAction());
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
                _hubClient.SendAction(new SendUpsertChatRoomAction());
                MessageBox.Show("創建成功");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
                return;
            }
        }

        private void GetAllRoom()
        {
            try
            {
                cbbRoom.Items.Clear();

                if (_rooms.Any())
                {
                    // 將聊天室資訊帶入cbb
                    var items = _rooms
                        .Where(w => w.Code != "HALL")
                        .Select(s => s.Code)
                        .ToArray();

                    cbbRoom.Items.AddRange(items);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} GetAllRoom Exception");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
