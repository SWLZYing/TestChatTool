using Autofac;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class Home : Form
    {
        private readonly IHttpHandler _handler;
        private readonly ILogger _logger;
        private ILifetimeScope _scope;

        /// <summary>
        /// 是否為管理者登入
        /// </summary>
        public bool IsAdmin { get; set; }
        public Admin Admin { get; set; }
        public User User { get; set; }

        public Home()
        {
            InitializeComponent();
            MaximizeBox = false;

            _handler = AutofacConfig.Container.Resolve<IHttpHandler>();
            _logger = LogManager.GetLogger("UIHome");

            SetAuthType();
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
                    case "btnSignIn":
                        SignIn();
                        break;
                    case "btnReg":
                        Register();
                        break;
                    case "btnChangePwd":
                        ChangePwd();
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

        private void ChangePwd()
        {
            var changePwd = _scope.Resolve<ChangePwd>();

            changePwd.RefreshView();
            changePwd.ShowDialog();
        }

        private void Register()
        {
            var register = _scope.Resolve<Register>();

            register.RefreshView(true);
            register.ShowDialog();
        }

        private void SignIn()
        {
            if (txtAcc.Text.IsNullOrWhiteSpace())
            {
                MessageBox.Show("帳號為必填");
                return;
            }

            if (txtPwd.Text.IsNullOrWhiteSpace())
            {
                MessageBox.Show("密碼為必填");
                return;
            }

            try
            {
                // 管理員登入
                if (cbbAuth.SelectedIndex == default)
                {
                    AdminSignIn();
                    IsAdmin = true;
                    return;
                }

                // 會員登入
                UserSignIn();
                IsAdmin = false;
                return;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} SignIn Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private void UserSignIn()
        {
            var user = _handler.CallApiPost("Sign/UserSignIn", new Dictionary<string, object>
            {
                { "Account", txtAcc.Text },
                { "Password", txtPwd.Text },
            });

            var response = JsonConvert.DeserializeObject<UserSignInResponse>(user);

            if (response.Code == (int)ErrorType.Success)
            {
                // 解鎖後 需變更密碼重啟
                if (response.Data.Status == UserStatusType.Unlock)
                {
                    var changePwd = _scope.Resolve<ChangePwd>();

                    changePwd.RefreshView();
                    changePwd.ShowDialog();

                    var change = _handler.CallApiPut("User/SetErrCountAndStatus", new Dictionary<string, object>
                    {
                        { "Account", response.Data.Account },
                        { "ErrorCount", 0 },
                        { "Status", (int)UserStatusType.Enable },
                    });

                    var changeResponse = JsonConvert.DeserializeObject<UserSetErrCountAndStatusResponse>(change);

                    if (changeResponse.Code != (int)ErrorType.Success)
                    {
                        MessageBox.Show(changeResponse.ErrorMsg);
                    }

                    return;
                }

                User = response.Data;
                // 登入成功 切換User視窗
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(response.ErrorMsg);
            }
        }

        private void AdminSignIn()
        {
            var admin = _handler.CallApiPost("Sign/AdminSignIn", new Dictionary<string, object>
            {
                { "Account", txtAcc.Text },
                { "Password", txtPwd.Text },
            });

            var response = JsonConvert.DeserializeObject<AdminSignInResponse>(admin);

            if (response.Code == (int)ErrorType.Success)
            {
                Admin = response.Data;
                // 關閉登入頁
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(response.ErrorMsg);
            }
        }

        private void SetAuthType()
        {
            cbbAuth.Items.Clear();

            // index 0(default)
            cbbAuth.Items.Add("管理員");
            // index 1
            cbbAuth.Items.Add("會員");

            cbbAuth.SelectedIndex = 1;
        }
    }
}
