using Autofac;
using Newtonsoft.Json;
using NLog;
using System;
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

                    // 建立聊天室
                    case "btnAddRoom":
                        AddRoom();
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
            throw new NotImplementedException();
        }

        private void Send()
        {
            throw new NotImplementedException();
        }

        private void AddRoom()
        {
            throw new NotImplementedException();
        }

        private void Unlock()
        {
            throw new NotImplementedException();
        }

        private void Verify()
        {
            try
            {
                var user = _helper.CallApiGet("User/QueryAllForVerify", null);

                var response = JsonConvert.DeserializeObject<UserQueryAllForVerifyResponse>(user);

                if (response.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(response.ErrorMsg);
                    return;
                }

                // 將需審核帳號帶入
                var userMaintain = _scope.Resolve<UserMaintain>();

                userMaintain.Accs = response.Data;
                userMaintain.SetAccs();
                userMaintain.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ButtonClick Exception");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
