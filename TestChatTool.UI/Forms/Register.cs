using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class Register : Form
    {
        private readonly IHttpHandler _helper;
        private readonly ILogger _logger;

        public Register(IHttpHandler helper)
        {
            InitializeComponent();
            MaximizeBox = false;

            _helper = helper;
            _logger = LogManager.GetLogger("UIRegister");
        }

        public void RefreshView()
        {
            txtAcc.Text = string.Empty;
            txtPwd.Text = string.Empty;
            txtPwdCheck.Text = string.Empty;
            txtNickName.Text = string.Empty;
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

                var result = _helper.CallApiPost("User/Create", new Dictionary<string, object>
                {
                    { "Account", txtAcc.Text },
                    { "Password", txtPwd.Text },
                    { "NickName", txtNickName.Text.IsNullOrWhiteSpace() ? txtAcc.Text : txtNickName.Text },
                });

                var response = JsonConvert.DeserializeObject<UserCreateResponse>(result);

                if (response.Code == (int)ErrorType.Success)
                {
                    MessageBox.Show("帳號註冊成功.");
                    Close();
                }
                else
                {
                    MessageBox.Show(response.ErrorMsg);
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
