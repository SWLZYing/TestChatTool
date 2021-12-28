using Microsoft.AspNet.SignalR.Client;
using NLog;
using System;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Helpers.Interface;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Forms
{
    public partial class Room : Form
    {
        private readonly IUserControllerApiHelper _userControllerApi;
        private readonly IOnLineUserControllerApiHelper _onLineUserControllerApi;
        private readonly IChatRoomControllerApiHelper _chatRoomControllerApi;
        private readonly ICallBackEventHandler _callBackEvent;
        private readonly IHubClient _hubClient;
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private User _user;
        private RoomInfo _room;
        private RoomInfo _hall;

        public User User => _user;

        public Room(
            IUserControllerApiHelper userControllerApi,
            IOnLineUserControllerApiHelper onLineUserControllerApi,
            IChatRoomControllerApiHelper chatRoomControllerApi,
            ICallBackEventHandler callBackEvent,
            IHubClient hubClient)
        {
            InitializeComponent();
            MaximizeBox = false;

            _userControllerApi = userControllerApi;
            _onLineUserControllerApi = onLineUserControllerApi;
            _chatRoomControllerApi = chatRoomControllerApi;
            _callBackEvent = callBackEvent;
            _hubClient = hubClient;
            _logger = LogManager.GetLogger("UIRoom");

            _callBackEvent.Add(ChatMessageAppend);

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
            _timer.Start();
            _user = user;

            GetAllRoom();

            txtNickName.Text = _user.NickName;
            cbbRoom.SelectedItem = _hall;
        }

        /// <summary>
        /// 接收聊天訊息
        /// </summary>
        /// <param name="eventData"></param>
        public void ChatMessageAppend(CallBackEventData eventData)
        {
            if (_user == null || eventData.RoomCode != _room.Code)
            {
                return;
            }

            switch (eventData.Action)
            {
                case CallBackActionType.ChatMessage:

                    UpdateMessage($"{eventData.NickName}-{eventData.CreateDateTime?.ToString("HH:mm:ss")}:{eventData.Message}");
                    break;

                case CallBackActionType.EnterRoom:

                    if (eventData.NickName != _user.NickName)
                    {
                        UpdateMessage($"{eventData.NickName} 已進入聊天室");
                    }
                    break;

                case CallBackActionType.LeaveRoom:

                    if (eventData.NickName != _user.NickName)
                    {
                        UpdateMessage($"{eventData.NickName} 已離開聊天室");
                    }
                    break;

                default:
                    break;
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
            // 第一次進入不顯示
            if (_room != null)
            {
                LeaveRoom();
            }

            _room = cbbRoom.SelectedItem as RoomInfo;
            txtMessage.Clear();

            UserOnLineUpsert(false);

            EnterRoom();
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

        private void EnterRoom()
        {
            _hubClient.SendAction(new BroadCastEnterRoomAction()
            {
                NickName = _user.NickName,
                RoomCode = _room.Code,
            });
        }

        private void LeaveRoom()
        {
            _hubClient.SendAction(new BroadCastLeaveRoomAction()
            {
                NickName = _user.NickName,
                RoomCode = _room.Code,
            });
        }

        private void SignOut()
        {
            LeaveRoom();
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
                    _hall = items.FirstOrDefault(f => f.Code == "HALL");

                    // 將聊天室資訊帶入cbb
                    cbbRoom.Items.AddRange(items);
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
