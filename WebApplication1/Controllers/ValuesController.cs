using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }


        class api_structure
        {
            public string token;
            public string user;
            public string message;
        }
        
        [HttpPost]
        public string Post(string app_token,string user_key,string text)
        {
            var str = new api_structure() { token = app_token, user = user_key, message = text };
            string ap_url = "https://api.pushover.net/1/messages.json";
            string json = JsonConvert.SerializeObject(str);
            var client = new HttpClient();
            var send_message = client.PostAsync(ap_url, new StringContent(json, Encoding.UTF8, "application/json")).Result;
            if (send_message.IsSuccessStatusCode)
            {
                return "Отправленно";
            }
            else
            {
                string error = send_message.Content.ReadAsStringAsync().Result;
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(error);
                string[] error_name = obj.errors.ToObject<string[]>();

                return "Ошибка: "+ string.Join("",error_name);
            }
        }
    }
}
