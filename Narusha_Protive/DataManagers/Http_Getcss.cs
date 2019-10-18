using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Narusha_Protive.DataManagers
{
    partial class Http_Getcs
    {
        public bool setUserNoticeRead(Notice notice)
        {
            if(UserData.id > 0)
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = UTF8Encoding.UTF8;
                JObject obj = new JObject();
                obj.Add("notice_id", notice.id);
                obj.Add("user_id", UserData.id);
                string response = client.UploadString("http://localhost:8080/setNoticeRead", obj.ToString());
                
                return response.ToString().Equals("true");
            }
            return false;
        }
        public bool setUserNoticeNotRead(Notice notice)
        {
            if (UserData.id > 0)
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = UTF8Encoding.UTF8;
                JObject obj = new JObject();
                obj.Add("notice_id", notice.id);
                obj.Add("user_id", UserData.id);
                string response = client.UploadString("http://localhost:8080/setNoticeNotRead", obj.ToString());
                return response.ToString().Equals("true");
            }
            return false;
        }
    }
}
