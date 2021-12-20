using Autofac;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestChatTool.UI.Forms
{
    public partial class Home : Form
    {
        private readonly ILogger _logger;
        private ILifetimeScope _scope;

        public Home()
        {
            InitializeComponent();
            MaximizeBox = false;

            _logger = LogManager.GetLogger("UIHome");
            SetAuth();
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

        private void Register()
        {
            var register = _scope.Resolve<Register>();

            register.RefreshView(true);
            register.ShowDialog();
        }

        private void SignIn()
        {
            MessageBox.Show("SignIn");
        }

        private void SetAuth()
        {
            cbbAuth.Items.Clear();

            cbbAuth.Items.Add("管理員");
            cbbAuth.Items.Add("會員");

            cbbAuth.SelectedIndex = 1;
        }
    }
}
