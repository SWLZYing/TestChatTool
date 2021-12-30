using NLog;
using System;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class Register : Form
    {
        private readonly IAdminControllerApiHelper _adminControllerApi;
        private readonly IUserControllerApiHelper _userControllerApi;
        private readonly ILogger _logger;

        public Register(
            IAdminControllerApiHelper adminControllerApi,
            IUserControllerApiHelper userControllerApi)
        {
            InitializeComponent();
            MaximizeBox = false;

            _userControllerApi = userControllerApi;
            _adminControllerApi = adminControllerApi;
            _logger = LogManager.GetLogger("UIRegister");

            SetAdminType();
        }

        public void RefreshView(bool isUser)
        {
            // 清空所有欄位
            txtAcc.Clear();
            txtPwd.Clear();
            txtPwdCheck.Clear();
            txtNickName.Clear();
            cbbAdminType.SelectedIndex = default;

            // 設定創建類型
            if (isUser)
            {
                lblTitle.Text = "會員註冊";
                lblNameOrType.Text = "暱稱";
                txtNickName.Show();
                txtNickName.IsAccessible = true;
                cbbAdminType.Hide();
                cbbAdminType.IsAccessible = false;
                btnRegister.Text = "註冊";
            }
            else
            {
                lblTitle.Text = "管理員創建";
                lblNameOrType.Text = "管理員層級";
                txtNickName.Hide();
                txtNickName.IsAccessible = false;
                cbbAdminType.Show();
                cbbAdminType.IsAccessible = true;
                btnRegister.Text = "創建";
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
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

                if (txtPwdCheck.Text != txtPwd.Text)
                {
                    MessageBox.Show("密碼確認失敗");
                    return;
                }

                // 管理員創建
                if (cbbAdminType.IsAccessible)
                {
                    CreateAdmin();
                    return;
                }

                // 會員註冊
                CreateUser();
                return;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ButtonClick Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateUser()
        {
            var nickName = txtNickName.Text.IsNullOrWhiteSpace() ? txtAcc.Text : txtNickName.Text;
            var user = _userControllerApi.Create(txtAcc.Text, txtPwd.Text, nickName);

            if (user.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("帳號註冊成功.");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
            }
        }

        private void CreateAdmin()
        {
            if (cbbAdminType.SelectedIndex == default)
            {
                MessageBox.Show("請確認管理員層級");
                return;
            }

            var response = _adminControllerApi.Create(
                txtAcc.Text,
                txtPwd.Text,
                (AdminType)cbbAdminType.SelectedItem);

            if (response.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("管理員創建成功.");
                Close();
            }
            else
            {
                MessageBox.Show(response.ErrorMsg);
            }
        }

        private void SetAdminType()
        {
            cbbAdminType.Items.Clear();

            foreach (var type in Enum.GetValues(typeof(AdminType)))
            {
                cbbAdminType.Items.Add(type);
            }
        }
    }
}
