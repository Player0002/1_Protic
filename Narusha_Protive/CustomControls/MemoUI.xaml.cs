using Narusha_Protive.Pages.Popup;
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

namespace Narusha_Protive.CustomControls
{
    /// <summary>
    /// MemoUI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MemoUI : UserControl
    {
        Frame frame = null;
        public bool isIn = false;
        public bool isClicked = false;
        string original = "";
        FormattedText originalFormatted = null;
        private void check()
        {
            new Task(() =>
            {
                //
                Dispatcher.Invoke(() =>
                {
                    original = MemoLabel.Content.ToString();
                    originalFormatted = new FormattedText(MemoLabel.Content.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(MemoLabel.FontFamily.ToString()), MemoLabel.FontSize + 2, Brushes.Black);
                    //
                    //
                    // 
                    if (originalFormatted.WidthIncludingTrailingWhitespace > MemoLabel.ActualWidth)
                    {
                        string s = MemoLabel.Content.ToString();
                        int idx = 1;

                        while ((new FormattedText(s.Substring(0, idx++), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(MemoLabel.FontFamily.ToString()), MemoLabel.FontSize + 2, Brushes.Black)).WidthIncludingTrailingWhitespace < MemoLabel.ActualWidth) ;
                        s = s.Substring(0, idx > 3 ? idx - 3 : idx);
                        MemoLabel.Content = s + "...";
                    }

                });
            }).Start();
        }
        public MemoUI()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                check();
            };
            BackGround.MouseDown += (sender, args) =>
            {
                if(UserData.last == frame || UserData.last == null)
                {
                    isClicked = !isClicked;
                    if (isClicked)
                    {
                        UserData.cnt = 1;
                        UserData.last = frame;
                        UserData.showd = frame;
                        UserData.clickedUI = this;
                        frame.IsHitTestVisible = true;
                    }
                    else
                    {
                        UserData.cnt = 0;
                        UserData.last = null;
                        UserData.showd = null;
                        UserData.clickedUI = null;
                        frame.IsHitTestVisible = false;
                        move(sender, args);
                    }
                }
                
            };
            BackGround.MouseMove += move;
            BackGround.MouseEnter += (sender, args) =>
            {
                if (!isIn && !isClicked && UserData.cnt == 0)
                {
                    UserData.isEnter = true;
                    isIn = true;
                    frame = new Frame(); UserData.showd = frame;

                    frame.Visibility = Visibility.Hidden;
                    Point point = Mouse.GetPosition(UserData.board);
                    
                    frame.Source = new Uri("Pages/Popup/MemoMessage.xaml", UriKind.Relative);
                    new Task(() =>
                    {   
                        System.Threading.Thread.Sleep(20);
                        Dispatcher.Invoke(() =>
                        {
                            if (frame != null)
                            {
                                double max = UserData.board.ActualWidth / 5;
                                double maxh = UserData.board.ActualHeight / 8;
                                // 
                                frame.Width = originalFormatted == null ? 0 : originalFormatted.Width > max ? max : originalFormatted.Width + 10;
                                frame.Height = 40;
                                // 
                                ((MemoMessage)frame.Content).setMessage(this.original);
                                //  
                                frame.Margin = new Thickness(point.X, point.Y, 0, 0);
                                //frame.Margin = new Thickness(0, 0, 0, 0);
                                frame.HorizontalAlignment = HorizontalAlignment.Left;
                                frame.VerticalAlignment = VerticalAlignment.Top;
                                frame.IsHitTestVisible = false;
                                /*Canvas canvas = new Canvas();
                                canvas.Background = new SolidColorBrush(Colors.Yellow);
                                canvas.Width = 100;
                                canvas.Height = 100;
                                canvas.Margin = new Thickness(Mouse.GetPosition(UserData.board).X, Mouse.GetPosition(UserData.board).Y, 0, 0);
                                canvas.HorizontalAlignment = HorizontalAlignment.Left;
                                canvas.VerticalAlignment = VerticalAlignment.Top;*/
                                UserData.board.MainGeneral.Children.Add(frame);
                                //  
                            }
                        });
                        System.Threading.Thread.Sleep(20);
                        Dispatcher.Invoke(() =>
                        {
                            if (frame != null)
                            {
                                reset();
                                frame.Visibility = Visibility.Visible;
                            }
                        });
                    }).Start();
                }
                
            };
            BackGround.MouseLeave += leaveEvent;
        }
        private void move(object sender, EventArgs args)
        {
            if (frame != null && !isClicked)
            {
                reset();
            }
        }
        private void reset()
        {
            
            Point point = Mouse.GetPosition(UserData.board);
            double result = frame.ActualWidth - (UserData.board.ActualWidth - point.X) + (1.3 * (UserData.board.ActualWidth / 1366) + 16); // XLocation Over test
            double resulth = (point.Y + frame.ActualHeight) - UserData.board.ActualHeight + 46;
            
            
            if (result > 0) frame.Margin = new Thickness(point.X - (frame.ActualWidth), point.Y, 0, 0);
            else frame.Margin = new Thickness(point.X, point.Y, 0, 0);
            if (resulth > 0)
            {
                frame.Margin = new Thickness(frame.Margin.Left, point.Y - frame.ActualHeight, 0, 0);
                frame.Height = UserData.board.ActualHeight / 4;
            }
            else frame.Margin = new Thickness(frame.Margin.Left, point.Y, 0, 0);
        }
        private void leaveEvent(Object sender, EventArgs args)
        {
            if (isIn && !isClicked)
            {
             //   
                UserData.showd = null;
                isIn = false;
                if (frame != null)
                    UserData.board.MainGeneral.Children.Remove(frame);
                frame = null;
                UserData.isEnter = false;
            }
        }
    }
}
