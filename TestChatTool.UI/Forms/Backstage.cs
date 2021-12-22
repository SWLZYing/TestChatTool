using Autofac;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using NLog;
using System;
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
        private readonly IHttpHandler _handler;
        private readonly IHubClient _hubClient;
        private readonly ILogger _logger;
        private ILifetimeScope _scope;
        private Admin _admin;
        private RoomInfo _room;
        private Timer _timer;

        public Backstage()
        {
            InitializeComponent();
            MaximizeBox = false;

            _handler = AutofacConfig.Container.Resolve<IHttpHandler>();
            _hubClient = AutofacConfig.Container.Resolve<IHubClient>();
            _logger = LogManager.GetLogger("UIBackstage");

            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Tick += (object sender, EventArgs e) =>
            {
                ChangeStatus();
            };
        }

        public ILifetimeScope Scope
        {
            set
            {
                _scope = value;
            }
        }

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
        }

        public void ChatMessageAppend(string roomCode, BroadCastChatMessageAction message)
        {
            if (_admin != null)
            {
                if (roomCode != _room.Code)
                {
                    return;
                }

                UpdateMessage($"{message.NickName}-{message.CreateDateTime.ToString("HH:mm:ss")}:{message.Message}");
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
            ChangeStatus();
            _timer.Start();
            _room = cbbRoom.SelectedItem as RoomInfo;
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

            _hubClient.SendAction(new SendChatMessageAction(_room.Code)
            {
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
            var users = _handler.CallApiGet("User/QueryAllForUnlock", null);

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
            var users = _handler.CallApiGet("User/QueryAllForVerify", null);

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
                var rooms = _handler.CallApiGet("ChatRoom/GetAll", null);

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
