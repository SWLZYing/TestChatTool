using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Handlers
{
    public class HttpHandler : IHttpHandler
    {
        private readonly string _serviceUrl;

        public HttpHandler(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        public string CallApiPost(string action, Dictionary<string, object> parameters)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_serviceUrl);

                    var request = new HttpRequestMessage(HttpMethod.Post, action)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                    };

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //取回傳值
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CallApiGet(string action, Dictionary<string, object> parameters)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var paras = parameters == null
                        ? string.Empty
                        : string.Join("&", parameters.Select(s => $"{s.Key}={s.Value}"));

                    var uri = new Uri($"{_serviceUrl}{action}?{paras}");

                    var response = client.GetAsync(uri).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //取回傳值
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CallApiPut(string action, Dictionary<string, object> parameters)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_serviceUrl);

                    var request = new HttpRequestMessage(HttpMethod.Put, action)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json")
                    };

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //取回傳值
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
