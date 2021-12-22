using Autofac;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class RoomMaintain : Form
    {
        private readonly IHttpHandler _handler;
        private readonly ILogger _logger;

        public RoomMaintain()
        {
            InitializeComponent();
            MaximizeBox = false;

            _handler = AutofacConfig.Container.Resolve<IHttpHandler>();
            _logger = LogManager.GetLogger("UIRoomMaintain");
        }

        internal void RefreshView()
        {
            txtCode.Clear();
            txtName.Clear();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;

                switch (btn.Name)
                {
                    case "btnCreate":
                        CreateRoom();
                        break;
                    case "btnUpdate":
                        UpdateRoom();
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

        private void UpdateRoom()
        {
            var user = _handler.CallApiPut("ChatRoom/Update", new Dictionary<string, object>
            {
                { "Code", txtCode.Text },
                { "Name", txtName.Text },
            });

            var response = JsonConvert.DeserializeObject<ChatRoomUpdateResponse>(user);

            if (response.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("更新成功");
                Close();
            }
            else
            {
                MessageBox.Show(response.ErrorMsg);
                return;
            }
        }

        private void CreateRoom()
        {
            var user = _handler.CallApiPost("ChatRoom/Create", new Dictionary<string, object>
            {
                { "Code", txtCode.Text },
                { "Name", txtName.Text },
            });

            var response = JsonConvert.DeserializeObject<ChatRoomCreateResponse>(user);

            if (response.Code == (int)ErrorType.Success)
            {
                MessageBox.Show("創建成功");
                Close();
            }
            else
            {
                MessageBox.Show(response.ErrorMsg);
                return;
            }
        }
    }
}
