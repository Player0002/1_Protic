using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narusha_Protive
{
    public class FileInfo
    {
        public double percentage;
        public double maxFileSize;
        public string Date;
        public string fileLocation;
    }
    public class DownloadData
    {
        public static DownloadData instance;
        public Dictionary<string, FileInfo> fileStatus = new Dictionary<string, FileInfo>();

        public delegate void updated();
        public event updated update;
        public event updated fileAdd;
        public event updated fileCompleted;
        public List<string> accessFiles = new List<string>();

        public DownloadData()
        {
            accessFiles.Add("ai");
            accessFiles.Add("avi");
            accessFiles.Add("css");
            accessFiles.Add("csv");
            accessFiles.Add("dbf");
            accessFiles.Add("doc");
            accessFiles.Add("docx");
            accessFiles.Add("dwg");
            accessFiles.Add("exe");
            accessFiles.Add("file");
            accessFiles.Add("fla");
            accessFiles.Add("html");
            accessFiles.Add("iso");
            accessFiles.Add("jpg");
            accessFiles.Add("js");
            accessFiles.Add("json");
            accessFiles.Add("mp3");
            accessFiles.Add("mp4");
            accessFiles.Add("pdf");
            accessFiles.Add("ppt");
            accessFiles.Add("pptx");
            accessFiles.Add("psd");
            accessFiles.Add("rtf");
            accessFiles.Add("svg");
            accessFiles.Add("txt");
            accessFiles.Add("xls");
            accessFiles.Add("xml");
            accessFiles.Add("zip");
        }

        public static DownloadData getInstance()
        {
            if (instance == null) instance = new DownloadData();
            return instance;
        }
        public void updateProgress()
        {
            update();
        }
        public void addProgress()
        {
            fileAdd();
        }
        public void endProgress()
        {
            fileCompleted();
        }


    }
}
