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
    /// MemoMessage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MemoMessage : Page
    {
        string message;
        bool started = false;
        public MemoMessage()
        {
            InitializeComponent();
            
        }
        public void setMessage(String message)
        {
            new Task(() =>
            {
                while (started)
                {
                    Dispatcher.Invoke(() =>{ Message.Content = "Reading..."; });
                    System.Threading.Thread.Sleep(1);
                }
            }).Start();
            new Task(() =>
            {
                System.Threading.Thread.Sleep(5);
                Dispatcher.Invoke(() =>
                {
                    this.message = message;
                    
                    string s = message;
                   int start = 0;
                   int idx = 1;
                   int idx2 = idx;
                    double currentsize = 60;
                    FormattedText text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Message.FontFamily.ToString()), Message.FontSize, Brushes.Black); //전체길이
                    started = true;
                    
                    idx = text.Width > this.ActualWidth ? getSmallText(text) - 5 : s.Length;
                    idx2 = idx;
                    String results = "Reading...";
                    do
                    {
                        double size = text.Height;
                        results = (results == "Reading..." ? "" : results) + s.Substring(start, idx2) + (start + idx2 < s.Length - 1 ? "\n" : "");
                        if (start + idx2 == s.Length - 1) break;
                        start = idx;
                        if (((idx + idx2) > (s.Length - 1))) idx2 = s.Length - start; idx = idx2 + idx;

                        if (currentsize < UserData.board.ActualHeight / 4 && UserData.showd != null)
                        {
                            this.Height += size;
                            currentsize += size;
                            UserData.showd.Height += size;
                        }
                        if (idx2 == 0) break;
                    } while (true);
                    Message.Content = results;
                    
                    started = false;

                });
            }).Start();
        }
        private int getSmallText(FormattedText text)
        {
            String s = text.Text;
            int idx = 1;
            while((new FormattedText(s.Substring(0, idx++), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Message.FontFamily.ToString()), Message.FontSize , Brushes.Black)).WidthIncludingTrailingWhitespace < this.ActualWidth);
            return idx > 3 ? idx - 3 : idx;
        }
    }
}
