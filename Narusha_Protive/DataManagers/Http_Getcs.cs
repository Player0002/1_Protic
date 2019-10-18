using Narusha_Protive.DashboardPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketClient;

namespace Narusha_Protive.DataManagers
{
    public enum TeamCodeVerify
    {
        TeamNameContains, TeamCodeContains, None, Empty
    }
    class TestWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = System.Threading.Timeout.Infinite;
            return request;
        }
    }
    partial class Http_Getcs
    {
        //JOBJ - GETUSER
        public JObject getUser(string s)
        {
            if(UserData.username != null)
            {
                var obj = new JObject();
                obj.Add("name", UserData.username);
                obj.Add(s, UserData.teamCode);
                obj.Add("email", UserData.email);
                //obj.Add("password", UserData.password);
                return obj;
            }
            return null;
        }

        //GET _ GetMemoText
        public string[] GetMemoText(int id)
        {
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            var obj = new JObject();
            obj.Add("user_id", UserData.id);
            obj.Add("memo_id", id);
            string response = client.UploadString("http://danny-dataserver.kro.kr:8080/getMemoText", obj.ToString());

            return response == null ? null : JsonConvert.DeserializeObject<string[]>(response);
        }



        //INIT - SET TEAMDATA
        public bool InitialzationTeam()
        {
            JObject obj = getUser("teamCode");
            if(obj != null)
            {
                
                var client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = UTF8Encoding.UTF8;
                
                string response = client.UploadString("http://danny-dataserver.kro.kr:8080/getUserTeam", obj.ToString());
                /*
                 * need value
                 * 
                 * teamCode
                 * 
                 */
                JObject data = JObject.Parse(response);
                
                TeamData.id = Convert.ToInt64(data["id"].ToString());
                TeamData.name = data["name"].ToString();
                TeamData.code = data["code"].ToString();
                TeamData.files = data["files"].ToString().Equals("null") ? null : JsonConvert.DeserializeObject<string[]>(data["files"].ToString());//.Values().Select(x => x.Value<string>()).ToArray();
                string str = getNoticeMessage();
                
                TeamData.notice = str.Equals("{\"message\":\"NO NOTICES\",\"errorCode\":\"404\"}") ? null : JsonConvert.DeserializeObject<List<Notice>>(str);

                string str2 = getToDoList();
                TeamData.toDoList = str2.Equals("{\"message\":\"NO ToDoList\",\"errorCode\":\"404\"}") ? null : JsonConvert.DeserializeObject<List<ToDoList>>(str2);
                

                UserData.socket = new WebSocketSharp.WebSocket("ws://danny-dataserver.kro.kr:8080/my-ws/websocket");

                StompMessageSerializer serializer = new StompMessageSerializer();
                UserData.socket.OnOpen += (sender, e) =>
                {
                    
                    var connect = new StompMessage("CONNECT");
                    connect["accept-version"] = "1.1";
                    connect["heart-beat"] = "0,1000";
                    UserData.socket.Send(serializer.Serialize(connect));
                    
                    var sub = new StompMessage(StompFrame.SUBSCRIBE);
                    sub["id"] = "sub-" + UserData.id;
                    sub["destination"] = "/topics/event";
                    UserData.socket.Send(serializer.Serialize(sub));

                    
                    var test = new StompMessage(StompFrame.SEND, "{\"name\":\"Test\"}");
                    test["content-type"] = "application/json";
                    test["destination"] = "/app/hello";
                    UserData.socket.Send(serializer.Serialize(test));
                };
                UserData.socket.OnMessage += (sender, e) =>
                {
                    string deserialized = serializer.Deserialize(e.Data).Body;
                    if (deserialized.Contains("Hello")) Console.WriteLine("Hello~");
                    

                    if (deserialized.Contains("UPDATED_FILES")) // UPDATE FILES
                    {
                        if (UserData.teamCode == JObject.Parse(deserialized)["name"].ToString())
                        {
                            client = new WebClient();
                            client.Headers[HttpRequestHeader.ContentType] = "application/json";
                            client.Encoding = UTF8Encoding.UTF8;
                            response = client.UploadString("http://danny-dataserver.kro.kr:8080/getUserTeam", obj.ToString());

                            data = JObject.Parse(response);
                            Console.WriteLine("Received");
                            TeamData.files = data["files"].ToString().Equals("null") ? null : JsonConvert.DeserializeObject<string[]>(data["files"].ToString());//.Values().Select(x => x.Value<string>()).ToArray()
                            UserData.fileUpdated = true;
                            TeamData.Updated = true;
                        }
                    }

                    if (deserialized.Contains("SetNoticeRead")) // Some player reading...
                    {
                        
                        var strs = getNoticeMessage();
                        
                        TeamData.notice = strs.Equals("{\"message\":\"NO NOTICES\",\"errorCode\":\"404\"}") ? null : JsonConvert.DeserializeObject<List<Notice>>(strs);
                        
                    }

                    if(deserialized.Contains("AddNotices")) // Notice added
                    {
                        var strs = getNoticeMessage();
                        
                        List<Notice> result = new List<Notice>();
                        TeamData.AddNotices = strs.Equals("{\"message\":\"NO NOTICES\",\"errorCode\":\"404\"}") ? null : JsonConvert.DeserializeObject<List<Notice>>(strs);
                        
                        if(TeamData.notice != null)
                        {
                            foreach(Notice n in TeamData.notice)
                                foreach (Notice n2 in TeamData.AddNotices)
                                    if (n.id == n2.id) result.Add(n2);
                            foreach (Notice n in result) TeamData.AddNotices.Remove(n);
                        }

                        
                        UserData.noticeUpdated = true;
                        TeamData.notice = strs.Equals("{\"message\":\"NO NOTICES\",\"errorCode\":\"404\"}") ? null : JsonConvert.DeserializeObject<List<Notice>>(strs);

                    }

                    if (deserialized.Contains("AddMemos")) // Memo Added
                    {
                        JObject objs = JObject.Parse(JObject.Parse(deserialized)["name"].ToString());
                        
                        if (objs["id"].ToString().Equals(UserData.id.ToString())) // IF THIS USER
                        {
                            List<int> current = JsonConvert.DeserializeObject<List<int>>(objs["messages"].ToString());
                            UserData.memos = current.ToArray();
                            UserData.updateMemos = current.ToArray()[current.Count() - 1];
                            UserData.userMemos = GetMemoText(UserData.memos.Length - 1);
                            UserData.memoUpdated = true;
                        }
                    }
                };
                UserData.socket.OnError += (sender, e) =>
                {
                    
                };
                UserData.socket.OnClose += (sender, e) =>
                {
                    MessageBox.Show("SERVER DISCONNECTED");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                };
                UserData.socket.Connect();
                
                return true;
            }
            return false;
        }
        private String getNoticeMessage()
        {
            var s = getUser("code");
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://danny-dataserver.kro.kr:8080/getNotices", s.ToString());
            
            return response2;
        }
        private string getToDoList()
        {
            var s = getUser("code");
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://danny-dataserver.kro.kr:8080/getToDoList", s.ToString());
            return response2;
        }
        //INIT - LOGIN
        public JObject Login(String id, String password)
        {
            if (id == null || password == null || id == "" || password == "") return null;
            string request = "{\"id\":\"" + id + "\",\"password\":\"" + DataFormatter.Encrypt512(password) + "\"}";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response = client.UploadString("http://danny-dataserver.kro.kr:8080/loginUser", request);
            JObject objs = JObject.Parse(response);
            return objs;
        } 
        //INIT - IsUserAutoLogin
        public JObject checkAutoLogin()
        {
            var json = new JObject();
            json.Add("pcName", getSerialNumber());
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response = client.UploadString("http://danny-dataserver.kro.kr:8080/isUserAutoLogin", json.ToString());
            return JObject.Parse(response);

        }
        //POST - REGISTERAutoLogin
        public JObject registerAutologin(String id, String password)
        {
            var json = new JObject();
            json.Add("pcName", getSerialNumber());
            json.Add("name", id);
            json.Add("password", password);
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response = client.UploadString("http://danny-dataserver.kro.kr:8080/registerAutologin", json.ToString());
            return JObject.Parse(response);
        }
        //POST - RemoveAutoLogin
        public bool removeAutoLogin()
        {
            var json = new JObject();
            json.Add("pcName", getSerialNumber());
            json.Add("name", UserData.email);
            json.Add("password", UserData.password);
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response = client.UploadString("http://danny-dataserver.kro.kr:8080/removeUserAutoLogin", json.ToString());
            return response.Equals("true");
        }
        //GET - MainBoard Serial Key
        public static string getSerialNumber()
        {
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            foreach (ManagementObject obj in MOS.Get()) return obj["SerialNumber"].ToString();
            return "null";
        }
    }
}
