﻿using Autofac;
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
    public partial class Register : Form
    {
        private readonly IHttpHandler _handler;
        private readonly ILogger _logger;

        public Register()
        {
            InitializeComponent();
            MaximizeBox = false;

            _handler = AutofacConfig.Container.Resolve<IHttpHandler>();
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
            var user = _handler.CallApiPost("User/Create", new Dictionary<string, object>
                {
                    { "Account", txtAcc.Text },
                    { "Password", txtPwd.Text },
                    { "NickName", txtNickName.Text.IsNullOrWhiteSpace() ? txtAcc.Text : txtNickName.Text },
                });

            var userResponse = JsonConvert.DeserializeObject<UserCreateResponse>(user);

            if (userResponse.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("帳號註冊成功.");
                Close();
            }
            else
            {
                MessageBox.Show(userResponse.ErrorMsg);
            }
        }

        private void CreateAdmin()
        {
            if (cbbAdminType.SelectedIndex == default)
            {
                MessageBox.Show("請確認管理員層級");
                return;
            }

            var admin = _handler.CallApiPost("Admin/Create", new Dictionary<string, object>
                    {
                        { "Account", txtAcc.Text },
                        { "Password", txtPwd.Text },
                        { "AccountType", (int)cbbAdminType.SelectedItem },
                    });

            var adminResponse = JsonConvert.DeserializeObject<AdminCreateResponse>(admin);

            if (adminResponse.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("管理員創建成功.");
                Close();
            }
            else
            {
                MessageBox.Show(adminResponse.ErrorMsg);
            }
        }

        private void SetAdminType()
        {
            cbbAdminType.Items.Clear();
            cbbAdminType.Items.Add("請選擇");

            foreach (var type in Enum.GetValues(typeof(AdminType)))
            {
                cbbAdminType.Items.Add(type);
            }

            cbbAdminType.SelectedIndex = default;
        }
    }
}
