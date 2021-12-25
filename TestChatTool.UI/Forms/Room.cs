using Microsoft.AspNet.SignalR.Client;
using NLog;
using System;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Helpers.Interface;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Forms
{
    public partial class Room : Form
    {
        private readonly IUserControllerApiHelper _userControllerApi;
        private readonly IOnLineUserControllerApiHelper _onLineUserControllerApi;
        private readonly IChatRoomControllerApiHelper _chatRoomControllerApi;
        private readonly IHubClient _hubClient;
        private readonly ILogger _logger;
        private User _user;
        private RoomInfo _room;
        private Timer _timer;

        public User User => _user;

        public Room(
            IUserControllerApiHelper userControllerApi,
            IOnLineUserControllerApiHelper onLineUserControllerApi,
            IChatRoomControllerApiHelper chatRoomControllerApi,
            IHubClient hubClient)
        {
            InitializeComponent();
            MaximizeBox = false;

            _userControllerApi = userControllerApi;
            _onLineUserControllerApi = onLineUserControllerApi;
            _chatRoomControllerApi = chatRoomControllerApi;
            _hubClient = hubClient;
            _logger = LogManager.GetLogger("UIRoom");

            _timer = new Timer { Interval = 500 };
            _timer.Tick += (object sender, EventArgs e) =>
            {
                ChangeStatus();
            };
        }

        /// <summary>
        /// 畫面開啟前設定
        /// </summary>
        /// <param name="user"></param>
        public void SetUpUI(User user)
        {
            _user = user;
            txtNickName.Text = _user.NickName;

            GetAllRoom();
            _timer.Start();
        }

        public void ChatMessageAppend(BroadCastChatMessageAction message)
        {
            if (_user != null)
            {
                if (message.RoomCode != _room.Code)
                {
                    return;
                }

                UpdateMessage($"{message.NickName}-{message.CreateDateTime.ToString("HH:mm:ss")}:{message.Message}");
            }
        }

        public void BroadCastLogout(BroadCastLogoutAction message)
        {
            if (_user != null)
            {
                if (message.RoomCode != _room.Code)
                {
                    return;
                }

                UpdateMessage($"{message.NickName} 已離開聊天室");
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;

                switch (btn.Name)
                {
                    case "btnChange":
                        ChangeNickName();
                        break;
                    case "btnSend":
                        Send();
                        break;
                    case "btnSignOut":
                        SignOut();
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

        private void SelectedValueChanged(object sender, EventArgs e)
        {
            _room = cbbRoom.SelectedItem as RoomInfo;
            txtMessage.Clear();
            UserOnLineUpsert(false);
        }

        private void DropDown(object sender, MouseEventArgs e)
        {
            GetAllRoom();
        }

        /// <summary>
        /// 更新人員在線狀態
        /// </summary>
        /// <param name="isSignOut">是否為會員登出</param>
        private void UserOnLineUpsert(bool isSignOut)
        {
            try
            {
                var roomCode = isSignOut ? "SignOut" : _room.Code;
                var onLine = _onLineUserControllerApi.Upsert(_user.Account, _user.NickName, roomCode);

                if (onLine.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(onLine.ErrorMsg);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} UserOnLineUpsert Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeStatus()
        {
            if (_hubClient.State == ConnectionState.Connected)
            {
                btnSend.Enabled = true;
            }
            else
            {
                btnSend.Enabled = false;
            }
        }

        private void UpdateMessage(string text)
        {
            if (txtMessage.InvokeRequired)
            {
                txtMessage.Invoke(new SafeCallDelegate(UpdateMessage), text);
            }
            else
            {
                if (txtMessage.TextLength > 9999)
                {
                    txtMessage.Clear();
                }

                txtMessage.AppendText($"{text}\r\n");
            }
        }

        private void SignOut()
        {
            _hubClient.SendAction(new BroadCastLogoutAction()
            {
                NickName = _user.NickName,
                RoomCode = _room.Code,
            });

            UserOnLineUpsert(true);

            Close();
        }

        private void Send()
        {
            if (txtTalk.Text.IsNullOrWhiteSpace())
            {
                MessageBox.Show("請輸入訊息!");
                return;
            }

            _hubClient.SendAction(new SendChatMessageAction()
            {
                RoomCode = _room.Code,
                NickName = _user.NickName,
                Message = txtTalk.Text,
                CreateDateTime = DateTime.Now
            });

            UserOnLineUpsert(false);

            txtTalk.Clear();
        }

        private void ChangeNickName()
        {
            if (txtNickName.Text.IsNullOrWhiteSpace())
            {
                txtNickName.Text = _user.NickName;
                MessageBox.Show("暱稱不可為空");
                return;
            }

            var user = _userControllerApi.Update(_user.Account, txtNickName.Text);

            if (user.Code != (int)ErrorType.Success)
            {
                txtNickName.Text = _user.NickName;
                MessageBox.Show(user.ErrorMsg);
                return;
            }

            _user = user.User;
            txtNickName.Text = _user.NickName;
            MessageBox.Show("暱稱修改成功");
        }

        private void GetAllRoom()
        {
            try
            {
                var rooms = _chatRoomControllerApi.GetAll();

                if (rooms.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(rooms.ErrorMsg);
                    return;
                }

                cbbRoom.Items.Clear();

                if (rooms.Rooms.Any())
                {
                    var items = rooms.Rooms.Select(s => new RoomInfo { Code = s.Code, Name = s.Name }).ToArray();

                    // 將聊天室資訊帶入cbb
                    cbbRoom.Items.AddRange(items);

                    cbbRoom.SelectedItem = items.FirstOrDefault(f => f.Code == "HALL");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} GetAllRoom Exception");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 委派傳遞字串
        /// </summary>
        /// <param name="text"></param>
        private delegate void SafeCallDelegate(string text);

        private class RoomInfo
        {
            public string Code { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
