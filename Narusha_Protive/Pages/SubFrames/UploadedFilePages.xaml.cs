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
using Narusha_Protive.DataManagers;

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
            string fileName = TeamData.files[0]; // files에서 첫번째 값 가져오기

            this.Loaded += (sender, args) => {
                updates(fileName, DownloadData.getInstance().accessFiles);
            };
            this.PreviewMouseDown += (sender, args) =>
            {
                Http_Getcs csget = new Http_Getcs();
                csget.downloadFile(0, Dispatcher);
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
