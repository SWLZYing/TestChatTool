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
using TestChatTool.UI.Models;
using TestChatTool.UI.SignalR.Interface;

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
        private User _user;
        private RoomInfo _room;
        private RoomInfo[] _rooms;

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

            _callBackEvent.Add(CallBackEvent);
        }

        /// <summary>
        /// 畫面開啟前設定
        /// </summary>
        /// <param name="user"></param>
        public void SetUpUI(User user)
        {
            _user = user;

            txtNickName.Text = _user.NickName;

            GetAllRoom(false);
        }

        /// <summary>
        /// 接收長連結事件
        /// </summary>
        /// <param name="eventData"></param>
        public void CallBackEvent(CallBackEventData eventData)
        {
            if (_user == null)
            {
                return;
            }

            switch (eventData.Action)
            {
                case CallBackActionType.ChatMessage:

                    if (eventData.RoomCode == _room.Code)
                    {
                        UpdateMessage($"{eventData.NickName}-{eventData.CreateDateTime?.ToString("HH:mm:ss")}:{eventData.Message}");
                    }
                    break;

                case CallBackActionType.EnterRoom:

                    if (eventData.Account != _user.Account && eventData.RoomCode == _room.Code)
                    {
                        UpdateMessage($"{eventData.NickName} 已進入聊天室");
                    }
                    break;

                case CallBackActionType.LeaveRoom:

                    if (eventData.Account != _user.Account && eventData.RoomCode == _room.Code)
                    {
                        UpdateMessage($"{eventData.NickName} 已離開聊天室");
                    }
                    break;

                case CallBackActionType.CheckConnect:

                    btnSend.Invoke((Action)ChangeStatus);
                    break;

                case CallBackActionType.UpsertChatRoom:

                    cbbRoom.Invoke((Action<bool>)GetAllRoom, false);
                    break;

                case CallBackActionType.DeleteChatRoom:

                    cbbRoom.Invoke((Action<bool>)GetAllRoom, true);
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
            // 無變更聊天室
            if (_room?.Code == ((RoomInfo)cbbRoom.SelectedItem)?.Code)
            {
                _room = cbbRoom.SelectedItem as RoomInfo;

                return;
            }

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

        private void RoomFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_hubClient.State == ConnectionState.Connected)
            {
                LeaveRoom();
                UserOnLineUpsert(true);
            }
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

                // 連線中更新會員在線狀態
                _hubClient.SendAction(new CheckConnectAction
                {
                    Account = _user.Account,
                    NickName = _user.NickName,
                    RoomCode = _room.Code,
                });
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
            _hubClient.SendAction(new SendEnterRoomAction()
            {
                RoomCode = _room.Code,
                Account = _user.Account,
                NickName = _user.NickName,
            });
        }

        private void LeaveRoom()
        {
            _hubClient.SendAction(new SendLeaveRoomAction()
            {
                RoomCode = _room.Code,
                Account = _user.Account,
                NickName = _user.NickName,
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

        private void GetAllRoom(bool isDelete)
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
                    // 將聊天室資訊帶入cbb
                    _rooms = rooms.Rooms.Select(s => new RoomInfo { Code = s.Code, Name = s.Name }).ToArray();
                    cbbRoom.Items.AddRange(_rooms);
                }

                // 確認原本聊天室
                var mapRoom = _rooms.FirstOrDefault(f => f.Code == _room?.Code);

                if (mapRoom == default)
                {
                    if (isDelete)
                    {
                        MessageBox.Show("當前聊天室已被刪除，所有會員移往大廳");
                    }

                    // 不存在則指向大廳
                    cbbRoom.SelectedItem = _rooms.FirstOrDefault(f => f.Code == "HALL");
                }
                else
                {
                    cbbRoom.SelectedItem = mapRoom;
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
    }
}
