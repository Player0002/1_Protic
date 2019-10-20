using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

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
                string response = client.UploadString("http://danny-dataserver.kro.kr:8080/setNoticeRead", obj.ToString());
                
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
                string response = client.UploadString("http://danny-dataserver.kro.kr:8080/setNoticeNotRead", obj.ToString());
                return response.ToString().Equals("true");
            }
            return false;
        }
        public void downloadFile(int idx, Dispatcher dispatcher)
        {
            string fileName = TeamData.files[idx];
            int cnt = 0;
            WebClient client = new WebClient();
            var log = new CommonOpenFileDialog();
            log.IsFolderPicker = true;
            CommonFileDialogResult res;
            if ((res = log.ShowDialog()) == CommonFileDialogResult.Ok)
            {
                var datas = DownloadData.getInstance();

                string identity = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                string resName = fileName.Substring(0, fileName.LastIndexOf('.'));
                string path = log.FileName;
                if (File.Exists(path + fileName))
                {
                    resName = resName + " (-1)";
                    for (int i = 0; i > -1; i++)
                    {
                        resName = resName.Substring(0, resName.LastIndexOf("(")) + "(" + (i + 1) + ")";
                        if (!File.Exists(path + resName + "." + identity)) break;
                    }
                }
                path += resName + "." + identity;

                client.DownloadDataCompleted += (sed, events) =>
                {
                    new Thread(() => {
                        var data = events.Result;
                        
                        using (Stream fileStream = File.OpenWrite(path))
                        {
                            fileStream.Write(data, 0, data.Length);
                        }
                        dispatcher.Invoke(()=>
                        {
                            datas.fileStatus[resName + "." + identity].percentage = 100;
                            datas.updateProgress();
                            datas.endProgress();
                        });
                    }).Start();
                };
                client.DownloadProgressChanged += (secnder, events) =>
                {
                    if (cnt++ % 500 == 0)
                    {
                        new Thread(() =>
                        {
                            var data = DownloadData.getInstance();
                            data.fileStatus[resName + "." + identity] = new FileInfo { Date = DateTime.Now.ToString("yyyy.MM.dd"), fileLocation = log.FileName, maxFileSize = events.TotalBytesToReceive / (1024 * 1024), percentage = events.ProgressPercentage };
                            dispatcher.Invoke(() => { data.updateProgress(); });
                        }).Start();
                    }
                };
                client.DownloadDataAsync(new Uri("http://danny-dataserver.kro.kr:8080/getFileTest?Name=" + TeamData.name + "&Index=" + idx));
                Console.WriteLine(TeamData.name + " . " + idx + " . " + fileName);
                datas.addProgress();
            }
        }
    }
}
