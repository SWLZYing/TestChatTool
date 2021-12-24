using Autofac;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Forms
{
    public partial class Backstage : Form
    {
        private readonly IHttpHandler _http;
        private readonly IHubClient _hubClient;
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private readonly Timer _timerForGetRoomUsers;
        private ILifetimeScope _scope;
        private Admin _admin;
        private RoomInfo _room;

        public Backstage(
            IHttpHandler http,
            IHubClient hubClient)
        {
            InitializeComponent();
            MaximizeBox = false;

            _http = http;
            _hubClient = hubClient;
            _logger = LogManager.GetLogger("UIBackstage");

            // 確認連線狀況
            _timer = new Timer { Interval = 500 };
            _timer.Tick += (object sender, EventArgs e) =>
            {
                ChangeStatus();
            };

            // 定時更新房間會員
            _timerForGetRoomUsers = new Timer { Interval = 5000 };
            _timerForGetRoomUsers.Tick += (object sender, EventArgs e) =>
            {
                if (_hubClient.State == ConnectionState.Connected)
                {
                    GetRoomAllUsers();
                }
            };
        }

        public ILifetimeScope Scope
        {
            set
            {
                _scope = value;
            }
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
            _timer.Start();
            _timerForGetRoomUsers.Start();
        }

        /// <summary>
        /// 接收聊天訊息
        /// </summary>
        /// <param name="roomCode"></param>
        /// <param name="message"></param>
        public void ChatMessageAppend(BroadCastChatMessageAction message)
        {
            if (_admin != null)
            {
                if (message.RoomCode != _room.Code)
                {
                    return;
                }

                UpdateMessage($"{message.NickName}-{message.CreateDateTime.ToString("HH:mm:ss")}:{message.Message}");
            }
        }

        /// <summary>
        /// 接收登出訊息
        /// </summary>
        /// <param name="message"></param>
        public void BroadCastLogout(BroadCastLogoutAction message)
        {
            if (_admin != null)
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
                    // 帳號審核
                    case "btnVerify":
                        Verify();
                        break;

                    // 帳號解鎖
                    case "btnUnlock":
                        Unlock();
                        break;

                    // 聊天室設定
                    case "btnRoomMaintain":
                        RoomMaintain();
                        GetAllRoom();
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
            _room = cbbRoom.SelectedItem as RoomInfo;
            txtMessage.Clear();
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

                var users = _http.CallApiGet("Online/FindRoomUser", new Dictionary<string, object>
                {
                    { "roomCode", _room.Code },
                });

                var response = JsonConvert.DeserializeObject<OnLineUserFindRoomUserResponse>(users);

                if (response.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(response.ErrorMsg);
                    return;
                }

                txtRoomUsers.Text = string.Join("\r\n", response.Data.Select(s => s.NickName));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} GetRoomAllUsers Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateAdmin()
        {
            var register = _scope.Resolve<Register>();

            register.RefreshView(false);
            register.ShowDialog();
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

        private void RoomMaintain()
        {
            var changePwd = _scope.Resolve<RoomMaintain>();

            changePwd.RefreshView();
            changePwd.ShowDialog();
        }

        private void Unlock()
        {
            var users = _http.CallApiGet("User/QueryAllForUnlock", null);

            var response = JsonConvert.DeserializeObject<UserQueryAllForUnlockResponse>(users);

            if (response.Code != (int)ErrorType.Success)
            {
                MessageBox.Show(response.ErrorMsg);
                return;
            }

            // 將需審核帳號帶入
            var userMaintain = _scope.Resolve<UserStatusMaintain>();

            userMaintain.Accs = response.Data;
            userMaintain.SetAccs();
            userMaintain.SetUpUI(false);
            userMaintain.ShowDialog();
        }

        private void Verify()
        {
            var users = _http.CallApiGet("User/QueryAllForVerify", null);

            var response = JsonConvert.DeserializeObject<UserQueryAllForVerifyResponse>(users);

            if (response.Code != (int)ErrorType.Success)
            {
                MessageBox.Show(response.ErrorMsg);
                return;
            }

            // 將需審核帳號帶入
            var userMaintain = _scope.Resolve<UserStatusMaintain>();

            userMaintain.Accs = response.Data;
            userMaintain.SetAccs();
            userMaintain.SetUpUI(true);
            userMaintain.ShowDialog();
        }

        private void GetAllRoom()
        {
            try
            {
                var rooms = _http.CallApiGet("ChatRoom/GetAll", null);

                var response = JsonConvert.DeserializeObject<ChatRoomGetAllResponse>(rooms);

                if (response.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(response.ErrorMsg);
                    return;
                }

                cbbRoom.Items.Clear();

                if (response.Data.Any())
                {
                    // 將聊天室資訊帶入cbb
                    var items = response.Data.Select(s => new RoomInfo { Code = s.Code, Name = s.Name }).ToArray();

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
