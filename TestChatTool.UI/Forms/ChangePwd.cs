using Autofac;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class ChangePwd : Form
    {
        private readonly IHttpHandler _http;
        private readonly ILogger _logger;

        public ChangePwd(IHttpHandler http)
        {
            InitializeComponent();
            MaximizeBox = false;

            _http = http;
            _logger = LogManager.GetLogger("UIChangePwd");
        }

        public void RefreshView()
        {
            // 清空所有欄位
            txtAcc.Clear();
            txtOldPwd.Clear();
            txtNewPwd.Clear();
            txtPwdCheck.Clear();
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

                if (txtOldPwd.Text.IsNullOrWhiteSpace())
                {
                    MessageBox.Show("原密碼為必填");
                    return;
                }

                if (txtNewPwd.Text.IsNullOrWhiteSpace())
                {
                    MessageBox.Show("新密碼為必填");
                    return;
                }

                if (txtPwdCheck.Text != txtNewPwd.Text)
                {
                    MessageBox.Show("密碼確認失敗");
                    return;
                }

                var user = _http.CallApiPut("User/ResetPwd", new Dictionary<string, object>
                {
                    { "Account", txtAcc.Text },
                    { "OldPassWord", txtOldPwd.Text },
                    { "NewPassWord", txtNewPwd.Text },
                });

                var userResponse = JsonConvert.DeserializeObject<UserResetPwdResponse>(user);

                if (userResponse.Code == (int)ErrorType.Success)
                {
                    DialogResult = DialogResult.OK;
                    MessageBox.Show("密碼修改成功 請用新密碼登入.");
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
