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
using System.Threading;
using Narusha_Protive.Pages.Popup;
using Narusha_Protive.DataManagers;
using Narusha_Protive.Pages.SubFrames;
using WebSocketClient;
using Newtonsoft.Json.Linq;

namespace Narusha_Protive.CustomControls
{
    /// <summary>
    /// NewNoticeUI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewNoticeUI : UserControl
    {
        private string originalString;
        private string whenCreated;
        private string newText;
        private FormattedText formattedText;
        private Frame showedFrame;

        private NewNoticePages pages;

        private bool isInside;

        private Notice notice;

        public void setNotice(Notice notice)
        {
            this.notice = notice;
        }
        public void setDelegate(NewNoticePages pages)
        {
            this.pages = pages;
        }
        public void check()
        {
            new Task(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    originalString = NoticeLabel.Content.ToString();
                    formattedText = new FormattedText(originalString, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(NoticeLabel.FontFamily.ToString()), NoticeLabel.FontSize + 2, Brushes.Black); //글자 오버를 체크하기 위해 생성한 Formatted 텍스트

                    if (General.ActualWidth < formattedText.WidthIncludingTrailingWhitespace) //글자가 화면을 초과함을 확인
                    {
                        
                        int idx = 0;
                        while ((new FormattedText(originalString.Substring(0, idx++), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(NoticeLabel.FontFamily.ToString()), NoticeLabel.FontSize + 2, Brushes.Black).WidthIncludingTrailingWhitespace < this.ActualWidth)) ; //글자 오버를 체크하기 위해 생성한 Formatted 텍스트
                        newText = originalString.Substring(0, idx > 3 ? idx - 3 : idx);
                        NoticeLabel.Content = newText + "...";
                    }
                    DateLabel.Content = DataFormatter.toDate(notice.when);
                    
                });
            }).Start();
            
        }
        public NewNoticeUI()
        {
            InitializeComponent();
            originalString = NoticeLabel.Content.ToString(); //원본 문자열 확보
            whenCreated = DateLabel.Content.ToString(); //언제 생성되었는지 확보
            this.Loaded += (sender, argss) =>
            {
                this.check();
            };
            
            //글자수를 초과할경우 ...으로 Replace
           /* new Task(() =>
            {
                Thread.Sleep(50); //화면이 완전 로딩될때 까지 대기
                Dispatcher.Invoke(() =>
                {
                    originalString = NoticeLabel.Content.ToString();
                    formattedText = new FormattedText(originalString, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(NoticeLabel.FontFamily.ToString()), NoticeLabel.FontSize + 2, Brushes.Black); //글자 오버를 체크하기 위해 생성한 Formatted 텍스트
                    
                    if (General.ActualWidth < formattedText.WidthIncludingTrailingWhitespace) //글자가 화면을 초과함을 확인
                    {
                        int idx = 0;
                        while ((new FormattedText(originalString.Substring(0, idx++), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(NoticeLabel.FontFamily.ToString()), NoticeLabel.FontSize + 2, Brushes.Black).WidthIncludingTrailingWhitespace < this.ActualWidth)) ; //글자 오버를 체크하기 위해 생성한 Formatted 텍스트
                        newText = originalString.Substring(0, idx - 3);
                        NoticeLabel.Content = newText + "...";
                    }
                    DateLabel.Content = DataFormatter.toDate(notice.when);
                    
                });
            }).Start();*/

            this.PreviewMouseDown += (sender, args) => // 공지사항을 눌렀을때 작동할 코드
            {
                NewNoticeDataM.IsShowed = !NewNoticeDataM.IsShowed; //현재 보여지고 있음을 표시
                if (showedFrame != null)
                {
                    showedFrame.IsHitTestVisible = true;
                    NewNoticeDataM.showedUI = this;
                }
                else NewNoticeDataM.showedUI = null;

            };
            this.MouseEnter += (sender, args) =>
            {
                if (NewNoticeDataM.IsShowed) return;
                if (isInside) return;

                isInside = true;
                showedFrame = null;

                showedFrame = new Frame();
                showedFrame.Visibility = Visibility.Hidden;
                Point mousePoint = Mouse.GetPosition(UserData.board); //메인 화면에 대한 마우스의 위치 반환
                showedFrame.Source = new Uri("Pages/Popup/NoticeMessage.xaml", UriKind.Relative); // 프레임에 보여줄 값 설정

                new Task(() => // Frame이 로딩될때까지 기다림
                {
                    Thread.Sleep(20);
                    Dispatcher.Invoke(() => // Call back
                    {
                        if (showedFrame != null) // NULL EXCEPTION 방지
                        {
                            double max = UserData.board.ActualWidth / 5;
                            double maxh = UserData.board.ActualHeight / 8;

                            showedFrame.Width = 300;
                            showedFrame.Height = 300;

                            ((NoticeMessage)showedFrame.Content).setMessage(notice);
                            showedFrame.Margin = new Thickness(mousePoint.X, mousePoint.Y, 0, 0);
                            showedFrame.HorizontalAlignment = HorizontalAlignment.Left;
                            showedFrame.VerticalAlignment = VerticalAlignment.Top;
                            showedFrame.IsHitTestVisible = false;

                            UserData.board.MainGeneral.Children.Add(showedFrame);
                        }
                    });
                    Thread.Sleep(20); // 위 작업이 완료될때까지 대기
                    Dispatcher.Invoke(() =>
                    {
                        if(showedFrame != null) // 위 작입이 실행되다 종료될수 있기에 확인
                        {
                            reset(); //위치 확인 - 화면을 넘어가면 위치 변경
                            showedFrame.Visibility = Visibility.Visible;
                        }
                    });
                }).Start();
            };
            this.MouseMove += (sender, args) =>
            {
                if (showedFrame != null && !NewNoticeDataM.IsShowed) reset();
            };
            this.MouseLeave += (sender, args) =>
            {
                if (isInside && !NewNoticeDataM.IsShowed)
                {
                    UserData.board.MainGeneral.Children.Remove(showedFrame);
                    NewNoticeDataM.showedUI = null;
                    NewNoticeDataM.IsShowed = false;
                    showedFrame = null;
                    isInside = false;
                }
            };
        }
        public void remove()
        {
            Http_Getcs httpGets = new Http_Getcs();

            httpGets.setUserNoticeRead(notice);

            TeamData.notice.Remove(notice); // 읽었기 때문에 제거한다.
            UserData.board.MainGeneral.Children.Remove(showedFrame);
            NewNoticeDataM.showedUI = null;
            NewNoticeDataM.IsShowed = false;
            showedFrame = null;
            isInside = false;
            pages.update(this);

            /*StompMessageSerializer serializer = new StompMessageSerializer();
            JObject obj = new JObject();
            obj.Add("name", UserData.username);
            obj.Add("id", UserData.id);
            obj.Add("teamCode", UserData.teamCode);
            var test = new StompMessage(StompFrame.SEND, obj.ToString());
            test["content-type"] = "application/json";
            test["destination"] = "/app/sendUpdateReadUser";
            UserData.socket.Send(serializer.Serialize(test));*/
        }
        private void reset()
        {
            Point mousePoint = Mouse.GetPosition(UserData.board);
            double resultW = showedFrame.ActualWidth - (UserData.board.ActualWidth - mousePoint.X) + (1.3 * (UserData.board.ActualWidth / 1366) + 16); // XLocation IS OVERED?
            double resultH = (mousePoint.Y + showedFrame.ActualHeight) - UserData.board.ActualHeight + 46;

            if (resultW > 0) showedFrame.Margin = new Thickness(mousePoint.X - (showedFrame.ActualWidth), mousePoint.Y, 0, 0);
            else showedFrame.Margin = new Thickness(mousePoint.X, mousePoint.Y, 0, 0);

            if (resultH > 0)
            {
                showedFrame.Margin = new Thickness(showedFrame.Margin.Left, mousePoint.Y - showedFrame.ActualHeight, 0, 0);
                showedFrame.Height = 300;
            }
            else showedFrame.Margin = new Thickness(showedFrame.Margin.Left, mousePoint.Y, 0, 0);
        }
    }
}
