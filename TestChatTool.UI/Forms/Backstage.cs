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
    public partial class Backstage : Form
    {
        private readonly IOnLineUserControllerApiHelper _onLineUserControllerApi;
        private readonly IChatRoomControllerApiHelper _chatRoomControllerApi;
        private readonly ICallBackEventHandler _callBackEvent;
        private readonly IHubClient _hubClient;
        private readonly Register _register;
        private readonly RoomMaintain _roomMaintain;
        private readonly UserStatusMaintain _userMaintain;
        private readonly ILogger _logger;
        private Admin _admin;
        private RoomInfo _room;
        private RoomInfo[] _rooms;

        public Backstage(
            IOnLineUserControllerApiHelper onLineUserControllerApi,
            IChatRoomControllerApiHelper chatRoomControllerApi,
            ICallBackEventHandler callBackEvent,
            IHubClient hubClient,
            Register register,
            RoomMaintain roomMaintain,
            UserStatusMaintain userMaintain)
        {
            InitializeComponent();
            MaximizeBox = false;

            _onLineUserControllerApi = onLineUserControllerApi;
            _chatRoomControllerApi = chatRoomControllerApi;
            _callBackEvent = callBackEvent;
            _hubClient = hubClient;
            _register = register;
            _roomMaintain = roomMaintain;
            _userMaintain = userMaintain;
            _logger = LogManager.GetLogger("UIBackstage");

            _callBackEvent.Add(CallBackEvent);
        }

        /// <summary>
        /// 畫面開啟前設定
        /// </summary>
        /// <param name="admin"></param>
        public void SetUpUI(Admin admin)
        {
            _admin = admin;

            if (admin.AccountType == AdminType.Normal)
            {
                btnCreate.Hide();
            }
            else
            {
                btnCreate.Show();
            }

            GetAllRoom();
            UpdateRoomUser();
        }

        /// <summary>
        /// 接收長連結事件
        /// </summary>
        /// <param name="eventData"></param>
        public void CallBackEvent(CallBackEventData eventData)
        {
            if (_admin == null)
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

                    if (eventData.RoomCode == _room.Code)
                    {
                        UpdateMessage($"{eventData.NickName} 已進入聊天室");
                        UpdateRoomUser();
                    }
                    break;

                case CallBackActionType.LeaveRoom:

                    if (eventData.RoomCode == _room.Code)
                    {
                        UpdateMessage($"{eventData.NickName} 已離開聊天室");
                        UpdateRoomUser();
                    }
                    break;

                case CallBackActionType.CheckConnect:

                    btnSend.Invoke((Action)ChangeStatus);
                    break;

                case CallBackActionType.UpsertChatRoom:
                case CallBackActionType.DeleteChatRoom:

                    cbbRoom.Invoke((Action)GetAllRoom);
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
                    // 帳號審核
                    case "btnVerify":
                        Verify();
                        break;

                    // 帳號解鎖
                    case "btnUnlock":
                        Unlock();
                        break;

                    // 聊天室新增
                    case "btnRoomCreate":
                        RoomMaintain(btn.Name);
                        break;

                    // 聊天室修改
                    case "btnRoomUpdate":
                        RoomMaintain(btn.Name);
                        break;

                    // 聊天室刪除
                    case "btnRoomDelete":
                        RoomMaintain(btn.Name);
                        break;

                    // 管理員發話
                    case "btnSend":
                        Send();
                        break;

                    // 創建後台帳號
                    case "btnCreate":
                        CreateAdmin();
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
            if (_room?.Code != ((RoomInfo)cbbRoom.SelectedItem)?.Code)
            {
                txtMessage.Clear();
            }

            _room = cbbRoom.SelectedItem as RoomInfo;
            GetRoomAllUsers();
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

        private void UpdateRoomUser()
        {
            if (txtRoomUsers.InvokeRequired)
            {
                txtRoomUsers.Invoke((Action)GetRoomAllUsers);
            }
            else
            {
                GetRoomAllUsers();
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

        private void GetRoomAllUsers()
        {
            try
            {
                if (_room == null)
                {
                    return;
                }

                var users = _onLineUserControllerApi.FindRoomUser(_room.Code);

                if (users.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(users.ErrorMsg);
                    return;
                }

                txtRoomUsers.Text = string.Join("\r\n", users.Users.Select(s => s.NickName));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} GetRoomAllUsers Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateAdmin()
        {
            _register.RefreshView(false);
            _register.ShowDialog();
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
                NickName = "Admin",
                Message = txtTalk.Text,
                CreateDateTime = DateTime.Now
            });

            txtTalk.Clear();
        }

        private void RoomMaintain(string btnName)
        {
            if (btnName != "btnRoomCreate")
            {
                _roomMaintain.Rooms = _rooms;
            }

            _roomMaintain.SetUpUI(btnName);
            _roomMaintain.ShowDialog();
        }

        private void Unlock()
        {
            _userMaintain.SetUpUI(false);
            _userMaintain.ShowDialog();
        }

        private void Verify()
        {
            _userMaintain.SetUpUI(true);
            _userMaintain.ShowDialog();
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
                    // 將聊天室資訊帶入cbb
                    _rooms = rooms.Rooms.Select(s => new RoomInfo { Code = s.Code, Name = s.Name }).ToArray();
                    cbbRoom.Items.AddRange(_rooms);
                }

                // 確認原本聊天室
                var mapRoom = _rooms.FirstOrDefault(f => f.Code == _room?.Code);

                if (mapRoom == default)
                {
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
