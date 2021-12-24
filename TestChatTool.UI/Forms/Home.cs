using Autofac;
using NLog;
using System;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class Home : Form
    {
        private readonly ISignControllerApiHelper _signControllerApi;
        private readonly IUserControllerApiHelper _userControllerApi;
        private readonly ILogger _logger;
        private ILifetimeScope _scope;

        /// <summary>
        /// 是否為管理者登入
        /// </summary>
        public bool IsAdmin { get; set; }
        public Admin Admin { get; set; }
        public User User { get; set; }

        public Home(
            ISignControllerApiHelper signControllerApi,
            IUserControllerApiHelper userControllerApi)
        {
            InitializeComponent();
            MaximizeBox = false;

            _signControllerApi = signControllerApi;
            _userControllerApi = userControllerApi;
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
            var user = _signControllerApi.UserSignIn(txtAcc.Text, txtPwd.Text);

            if (user.Code == (int)ErrorType.Success)
            {
                // 解鎖後 需變更密碼重啟
                if (user.Data.Status == UserStatusType.Unlock)
                {
                    var changePwd = _scope.Resolve<ChangePwd>();

                    changePwd.RefreshView();

                    if (changePwd.ShowDialog() == DialogResult.OK)
                    {
                        var userStatus = _userControllerApi.SetErrCountAndStatus(
                            user.Data.Account,
                            0,
                            UserStatusType.Enable);

                        if (userStatus.Code != (int)ErrorType.Success)
                        {
                            MessageBox.Show(userStatus.ErrorMsg);
                        }
                    }

                    return;
                }

                User = user.Data;
                // 登入成功 切換User視窗
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
            }
        }

        private void AdminSignIn()
        {
            var admin = _signControllerApi.AdminSignIn(txtAcc.Text, txtPwd.Text);

            if (admin.Code == (int)ErrorType.Success)
            {
                Admin = admin.Data;
                // 關閉登入頁
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(admin.ErrorMsg);
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
