using Narusha_Protive.DataManagers;
using System;
using System.Collections.Generic;
using System.Globalization;

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

namespace Narusha_Protive.Pages.Popup
{
    /// <summary>
    /// NoticeMessage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class NoticeMessage : Page
    {
        private string message;
        private bool started;
        private Notice notice;
        public NoticeMessage()
        {
            InitializeComponent();
        }
        public void setMessage(Notice notice)
        {
            this.notice = notice;
            new Task(() =>
            {
                while (started)
                {
                    Dispatcher.Invoke(() => { Message.Content = "Reading..."; });
                    System.Threading.Thread.Sleep(1);
                }
            }).Start();
            new Task(() =>
            {
                System.Threading.Thread.Sleep(20);
                Dispatcher.Invoke(() =>
                {
                    int idx = 0;
                    this.message = notice.message; //메시지 카피
                    started = true;
                    StringBuilder builder = new StringBuilder();
                    while (true)
                    {
                        while (idx++ < message.Length && (new FormattedText(message.Substring(0, idx), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Message.FontFamily.ToString()), Message.FontSize, Brushes.Black)).WidthIncludingTrailingWhitespace < (Viewer.ActualWidth / 13.2) * 12);
                        if (idx-1 == 0) break;
                        builder.Append(message.Substring(0, idx-1) + "\n");
                        message = message.Substring(idx-1);
                        idx = 0;
                    }
                    Message.Content = builder.ToString();
                    Date.Content = DataFormatter.toDate(notice.when);
                    /* while (text.WidthIncludingTrailingWhitespace > this.ActualWidth)
                     {
                         int idx = getSmallText(text);
                         if (idx == 0) break;
                         Message.Content = (Message.Content == null ? "" : Message.Content) + s.Substring(0, idx) + "\n";
                         s = s.Substring(idx );
                         Console.WriteLine();
                         Console.WriteLine("IDX : " + idx);
                         Console.WriteLine();
                         Console.WriteLine(s);
                         text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Message.FontFamily.ToString()), Message.FontSize, Brushes.Black);                        text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Message.FontFamily.ToString()), Message.FontSize + 2, Brushes.Black);
                         double size =text.Height; 
                         //Console.WriteLine("CURRENT DATA : " + text.Text);
                         this.Height += size;
                         UserData.showd.Height += size;
                     }
                     if (text.WidthIncludingTrailingWhitespace <= this.ActualWidth) Message.Content = Message.Content + text.Text;*/
                });
            }).Start();
        }
        private int getSmallText(String text)
        {
            int idx = -1;
            while ((new FormattedText(text.Substring(0, ++idx), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Message.FontFamily.ToString()), Message.FontSize, Brushes.Black)).WidthIncludingTrailingWhitespace < this.ActualWidth) ;
            return idx > 3 ? idx - 3 : idx;
        }
    }
}
