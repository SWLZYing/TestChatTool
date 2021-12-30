using NLog;
using System;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class UserStatusMaintain : Form
    {
        private readonly IUserControllerApiHelper _userControllerApi;
        private readonly ILogger _logger;

        public UserStatusMaintain(IUserControllerApiHelper userControllerApi)
        {
            InitializeComponent();
            MaximizeBox = false;

            _userControllerApi = userControllerApi;
            _logger = LogManager.GetLogger("UIUserStatusMaintain");
        }

        public void SetUpUI(bool isVerify)
        {
            btnOk.Text = isVerify ? "開通" : "解鎖";
            SetAccs(isVerify ? UserStatusType.Disabled : UserStatusType.Lock);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;

                switch (btn.Text)
                {
                    case "開通":
                        Verify();
                        break;
                    case "解鎖":
                        Unlock();
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

        private void SetAccs(UserStatusType statusType)
        {
            cbbAcc.Items.Clear();
            btnOk.Enabled = false;

            var users = _userControllerApi.QueryAllForUserStatus(statusType);

            if (users.Code != (int)ErrorType.Success)
            {
                MessageBox.Show(users.ErrorMsg);
                return;
            }

            if (users.Users.Any())
            {
                foreach (var user in users.Users)
                {
                    cbbAcc.Items.Add(user.Account);
                }

                cbbAcc.SelectedIndex = 0;
                btnOk.Enabled = true;
            }
        }

        private void Verify()
        {
            var user = _userControllerApi.SetErrCountAndStatus(
                cbbAcc.SelectedItem.ToString(),
                0,
                UserStatusType.Enable);

            if (user.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("已開通.");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
                return;
            }
        }

        private void Unlock()
        {
            var user = _userControllerApi.SetErrCountAndStatus(
                cbbAcc.SelectedItem.ToString(),
                0,
                UserStatusType.Unlock);

            if (user.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("已解鎖.");
                Close();
            }
            else
            {
                MessageBox.Show(user.ErrorMsg);
                return;
            }
        }
    }
}
