using Autofac;
using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class Backstage : Form
    {
        private readonly IHttpHandler _helper;
        private readonly ILogger _logger;
        private ILifetimeScope _scope;


        public Backstage(IHttpHandler helper)
        {
            InitializeComponent();
            MaximizeBox = false;

            _helper = helper;
            _logger = LogManager.GetLogger("UIBackstage");
        }

        public ILifetimeScope Scope
        {
            set
            {
                _scope = value;
            }
        }

        public void SetUpUI(bool isNormal)
        {
            if (isNormal)
            {
                btnCreate.Hide();
            }
            else
            {
                btnCreate.Show();
            }

            GetAllRoom();
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

        private void CreateAdmin()
        {
            var register = _scope.Resolve<Register>();

            register.RefreshView(false);
            register.ShowDialog();
        }

        private void Send()
        {
            throw new NotImplementedException();
        }

        private void RoomMaintain()
        {
            var changePwd = _scope.Resolve<RoomMaintain>();

            changePwd.RefreshView();
            changePwd.ShowDialog();
        }

        private void Unlock()
        {
            var users = _helper.CallApiGet("User/QueryAllForUnlock", null);

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
            var users = _helper.CallApiGet("User/QueryAllForVerify", null);

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
                var rooms = _helper.CallApiGet("ChatRoom/GetAll", null);

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
