using NLog;
using System;
using System.Collections.Generic;
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
        private IEnumerable<string> _accs;

        public UserStatusMaintain(IUserControllerApiHelper userControllerApi)
        {
            InitializeComponent();
            MaximizeBox = false;

            _userControllerApi = userControllerApi;
            _logger = LogManager.GetLogger("UIUserStatusMaintain");
        }

        public IEnumerable<string> Accs
        {
            set
            {
                _accs = value;
            }
        }

        public void SetAccs()
        {
            cbbAcc.Items.Clear();

            btnOk.Enabled = false;

            if (_accs.Any())
            {
                foreach (var acc in _accs)
                {
                    cbbAcc.Items.Add(acc);
                }

                cbbAcc.SelectedIndex = 0;
                btnOk.Enabled = true;
            }
        }

        public void SetUpUI(bool isVerify)
        {
            btnOk.Text = isVerify ? "開通" : "解鎖";
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
