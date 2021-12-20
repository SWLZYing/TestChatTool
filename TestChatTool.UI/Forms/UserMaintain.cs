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

            if(_accs.Count <= 0)
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

        private void ButtonClick(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ButtonClick Exception");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
