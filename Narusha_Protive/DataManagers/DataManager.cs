using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Narusha_Protive.DataManagers
{
    class DataManager
    {
        internal static TeamCodeVerify CheckStr(string str)
        {
        if (str.Equals("")) return TeamCodeVerify.Empty;
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://localhost:8080/teamNameOrTeamCode", str);
            var objs = JObject.Parse(response2);
            Console.WriteLine("NAME " + objs["name"].ToString());
            Console.WriteLine("Code " + objs["code"].ToString());
            if (objs["code"].ToString().Equals("True")) return TeamCodeVerify.TeamCodeContains;
            if (objs["name"].ToString().Equals("True")) return TeamCodeVerify.TeamNameContains;
            return TeamCodeVerify.None;
        }


        internal static string GetTeamName(string code)
        {
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://localhost:8080/getTeamFCode", code);
            var objs = JObject.Parse(response2);            if (response2 == "{\"message\":\"NO NOTICES\",\"errorCode\":\"404\"}") return null;
            else return objs["name"].ToString();

        }

        internal static bool isCanUseEmail(string email)
        {
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://localhost:8080/getCanUseThisEmail", email);
            return response2.ToString().Equals("false");
        }

        internal static bool isCanUseUsername(string text)
        {
            var client = new WebClient();

            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string rse = client.UploadString("http://localhost:8080/getUser", text);
            return rse.ToString().Equals("{\"message\":\"Can't find user name\",\"errorCode\":\"404\"}");
        }

        internal static void EndVerifyCode(string email)
        {
            var client = new WebClient();

            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string rse = client.UploadString("http://localhost:8080/endVerifity", email);
            if (rse.ToString().Equals("false")) MessageBox.Show("ERROR");
        }

        internal static bool CheckVerifyCode(string email, string code)
        {
            var client = new WebClient();

            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;

            var obj = new JObject();
            obj.Add("email", email);
            obj.Add("code", code);

            string res = client.UploadString("http://localhost:8080/checkverify", obj.ToString());

            return res.ToString().Equals("true");
        }

        internal static void RegisterUser(string name, string id, string password, string team, TeamCodeVerify teamCodeVerify)
        {
            if (teamCodeVerify == TeamCodeVerify.None) MakeNewTeam(team, team);

            string code = (teamCodeVerify == TeamCodeVerify.TeamCodeContains ? team : GetTeamCode(team));

            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            JObject obj = new JObject();
            obj.Add("name", name);
            obj.Add("email", id);
            obj.Add("password", DataFormatter.Encrypt512(password));
            obj.Add("teamCode", teamCodeVerify == TeamCodeVerify.TeamCodeContains ? team : GetTeamCode(team));
            client.UploadString("http://localhost:8080/addUser", obj.ToString());
        }

        private static void MakeNewTeam(string team, string name)
        {
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            JObject obj = new JObject();
            obj.Add("name", team);
            obj.Add("code", name);
            string response2 = client.UploadString("http://localhost:8080/addTeam", obj.ToString());
            Console.WriteLine(response2);
        }

        private static string GetTeamCode(string teamname)
        {
            Console.WriteLine(teamname);
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://localhost:8080/getTeamFCode", teamname);
            var objs = JObject.Parse(response2);
            Console.WriteLine("\n\n" + response2);
            if (response2 == "{\"message\":\"Cant find team name\",\"errorCode\":\"404\"}") return null;
            return objs["code"].ToString();
        }
        internal static string VerifyCode(string email)
        {
            var client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Encoding = UTF8Encoding.UTF8;
            string response2 = client.UploadString("http://localhost:8080/makeNewVerify", email);
            return response2;
        }
    }
}
