using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Response;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Forms
{
    public partial class Room : Form
    {
        private readonly IHttpHandler _helper;
        private readonly ILogger _logger;
        private User _user;

        public Room(IHttpHandler helper)
        {
            InitializeComponent();
            MaximizeBox = false;

            _helper = helper;
            _logger = LogManager.GetLogger("UIRoom");
        }

        public void SetUpUI(User user)
        {
            _user = user;

            txtNickName.Text = user.NickName;
            GetAllRoom();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;

                switch (btn.Name)
                {
                    case "btnChange":
                        ChangeNickName();
                        break;
                    case "btnSend":
                        Send();
                        break;
                    case "btnSignOut":
                        SignOut();
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

        private void SignOut()
        {
            throw new NotImplementedException();
        }

        private void Send()
        {
            throw new NotImplementedException();
        }

        private void ChangeNickName()
        {
            if (txtNickName.Text.IsNullOrWhiteSpace())
            {
                txtNickName.Text = _user.NickName;
                MessageBox.Show("暱稱不可為空");
                return;
            }

            var user = _helper.CallApiPut("User/Update", new Dictionary<string, object>
            {
                { "Account", _user.Account },
                { "NickName", txtNickName.Text },
            });

            var response = JsonConvert.DeserializeObject<UserUpdateResponse>(user);

            if (response.Code != (int)ErrorType.Success)
            {
                MessageBox.Show(response.ErrorMsg);
                return;
            }

            _user = response.Data;
            txtNickName.Text = response.Data.NickName;
            MessageBox.Show("暱稱修改成功");
        }

        private void GetAllRoom()
        {
            try
            {
                var rooms = _helper.CallApiGet("ChatRoom/GetAll", null);

                var response = JsonConvert.DeserializeObject<ChatRoomGetAllResponse>(rooms);

                if (response.Code != (int)ErrorType.Success)
                {
                    MessageBox.Show(response.ErrorMsg);
                    return;
                }

                cbbRoom.Items.Clear();

                if (response.Data.Any())
                {
                    var items = response.Data.Select(s => new RoomInfo { Code = s.Code, Name = s.Name }).ToArray();

                    // 將聊天室資訊帶入cbb
                    cbbRoom.Items.AddRange(items);

                    cbbRoom.SelectedItem = items.FirstOrDefault(f => f.Code == "HALL");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} GetAllRoom Exception");
                MessageBox.Show(ex.Message);
            }
        }

        private class RoomInfo
        {
            public string Code { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
