using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class UserMaintain : Form
    {
        private readonly IHttpHandler _helper;
        private readonly ILogger _logger;
        private List<string> _accs;

        public UserMaintain(IHttpHandler helper)
        {
            InitializeComponent();
            MaximizeBox = false;

            _helper = helper;
            _logger = LogManager.GetLogger("UIUserMaintain");
        }

        public List<string> Accs
        {
            set
            {
                _accs = value;
            }
        }

        public void SetAccs()
        {
            cbbAcc.Items.Clear();

            if (_accs.Count <= 0)
            {
                btnOk.Enabled = false;
                return;
            }

            foreach (var acc in _accs)
            {
                cbbAcc.Items.Add(acc);
            }

            cbbAcc.SelectedIndex = 0;
            btnOk.Enabled = true;
        }

        public void SetUI(bool isVerify)
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
            var user = _helper.CallApiPut("User/SetErrCountAndStatus", new Dictionary<string, object>
            {
                { "Account", cbbAcc.SelectedItem.ToString() },
                { "ErrorCount", 0 },
                { "Status", UserStatusType.Enable },
            });

            var userResponse = JsonConvert.DeserializeObject<UserSetErrCountAndStatusResponse>(user);

            if (userResponse.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("帳號開通成功.");
                Close();
            }
            else
            {
                MessageBox.Show(userResponse.ErrorMsg);
                return;
            }
        }

        private void Unlock()
        {
            var user = _helper.CallApiPut("User/SetErrCountAndStatus", new Dictionary<string, object>
            {
                { "Account", cbbAcc.SelectedItem.ToString() },
                { "ErrorCount", 0 },
                { "Status", UserStatusType.Unlock },
            });

            var userResponse = JsonConvert.DeserializeObject<UserSetErrCountAndStatusResponse>(user);

            if (userResponse.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("帳號開通成功.");
                Close();
            }
            else
            {
                MessageBox.Show(userResponse.ErrorMsg);
                return;
            }
        }
    }
}
