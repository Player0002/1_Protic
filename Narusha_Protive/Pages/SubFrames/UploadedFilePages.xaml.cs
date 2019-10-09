using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            List<string> accessFiles = new List<string>();
            if (TeamData.files == null) return;
            string fileName = TeamData.files[TeamData.files.Length - 1]; // files에서 첫번째 값 가져오기

            //Add file icons
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



            this.Loaded += (sender, args) => {
                updates(fileName, accessFiles);
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
                Console.WriteLine(" / " + identity + " / " + realName);

                while ((new FormattedText(fileName.Substring(0, idx++), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FileFullName.FontFamily.ToString()), FileFullName.FontSize + 2, Brushes.Black).WidthIncludingTrailingWhitespace < this.ActualWidth / 6 * 5)) ; //글자 오버를 체크하기 위해 생성한 Formatted 텍스트
                newText = realName.Substring(0, idx - 7);
                newText += "...";
            }
            FileFullName.Content = newText + "." + identity;
            Console.WriteLine(identity + " / " + accessFiles.Contains(identity));
            if (accessFiles.Contains(identity)) Img.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/" + identity + ".png"));
            else Img.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/file.png"));
        }
    }
}
