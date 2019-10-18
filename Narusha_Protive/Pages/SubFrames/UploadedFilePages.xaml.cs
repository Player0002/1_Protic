using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace Narusha_Protive.Pages.SubFrames
{
    /// <summary>
    /// UploadedFilePages.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UploadedFilePages : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public UploadedFilePages()
        {
            InitializeComponent();
            if (TeamData.files == null) return;
            string fileName = TeamData.files[TeamData.files.Length - 1]; // files에서 첫번째 값 가져오기



            this.Loaded += (sender, args) => {
                updates(fileName, DownloadData.getInstance().accessFiles);
            };
            this.PreviewMouseDown += (sender, args) =>
            {
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
                            Dispatcher.Invoke(() =>
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
                                Dispatcher.Invoke(() => { data.updateProgress(); });
                                
                            }).Start();
                        }
                    };
                    client.DownloadDataAsync(new Uri("http://danny-dataserver.kro.kr:8080/getFileTest?Name=" + TeamData.name + "&Index=0"));
                    datas.addProgress();
                }
            };
        }
        public void updates(string fileName, List<string> accessFiles)
        {
            string identity = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
            string realName = fileName.Substring(0, fileName.LastIndexOf('.'));
            string newText = realName;
            int idx = 0;
            if (new FormattedText(fileName, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FileFullName.FontFamily.ToString()), FileFullName.FontSize + 2, Brushes.Black).WidthIncludingTrailingWhitespace > this.ActualWidth / 6 * 5) // 글자수 오버 체크
            {
                
                while ((new FormattedText(fileName.Substring(0, idx++), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FileFullName.FontFamily.ToString()), FileFullName.FontSize + 2, Brushes.Black).WidthIncludingTrailingWhitespace < this.ActualWidth / 6 * 5)) ; //글자 오버를 체크하기 위해 생성한 Formatted 텍스트
                newText = realName.Substring(0, idx - 7);
                newText += "...";
            }
            FileFullName.Content = newText + "." + identity;
            
            if (accessFiles.Contains(identity)) Img.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/" + identity + ".png"));
            else Img.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/file.png"));
        }
    }
}
