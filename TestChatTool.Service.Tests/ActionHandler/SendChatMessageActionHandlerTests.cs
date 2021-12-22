﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler;

namespace TestChatTool.Service.Tests.ActionHandler
{
    [TestClass]
    public class SendChatMessageActionHandlerTests
    {
        [TestMethod]
        public void send_msg()
        {
            var handler = new SendChatMessageActionHandler();
            var result = handler.ExecuteAction(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new SendChatMessageAction("HALL")
                {
                    NickName = "TEST001",
                    Message = "TEST123456",
                    CreateDateTime = DateTime.Now
                })
            });

            Assert.IsNull(result.exception);
            Assert.AreEqual(result.notifyType, NotifyType.BroadCast);
            Assert.IsNotNull(result.actionBase);
        }
    }
}