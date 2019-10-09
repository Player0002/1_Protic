using Narusha_Protive.CustomControls;
using Narusha_Protive.Pages.Popup;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Narusha_Protive
{
    /// <summary>
    /// Dashboard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Dashboard : Window
    {
        DispatcherTimer timer_Date = new DispatcherTimer();
        private Grid selectedGrid = null;
        private bool userinfoVisibility = false;
        private bool isLoading = false;
        public Dashboard()
        {
            InitializeComponent();
            UserData.board = this;
            LoadingFrame.Visibility = Visibility.Visible;
            isLoading = true;
            new Task(() =>
            {
                while (true)
                {
                    if (!isLoading)
                    {
                        RemoveTask();
                        break;
                    }
                }
            }).Start();
            new Task(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    userName.Content = UserData.username;
                    timer_Date.Interval = TimeSpan.FromSeconds(0.5);
                    timer_Date.Tick += DateEvent;
                    timer_Date.Start();
                    selectedGrid = Menu_1;
                    registerButton(Menu_1);
                    registerButton(Menu_2);
                    registerButton(Menu_3);
                    registerButton(Menu_4);
                    registerUser(UserInfo);
                }); System.Threading.Thread.Sleep(1000);

                isLoading = false;
            }).Start();

            MainGeneral.MouseDown += (sender, args) =>
            {
                if (UserData.last != null)
                {
                    UserData.showd.IsHitTestVisible = false;
                    UserData.cnt = 0;
                    UserData.clickedUI.isClicked = false;
                    UserData.clickedUI.isIn = false;
                    UserData.board.MainGeneral.Children.Remove(UserData.showd); // remove frame;
                    UserData.last = null;
                    UserData.showd = null;
                    UserData.isEnter = false;
                    UserData.clickedUI = null;// set Frame null;
                    UserData.isOutsideClick = true;
                }
                if (NewNoticeDataM.showedUI != null)
                {
                    NewNoticeDataM.showedUI.remove();
                    UserData.isOutsideClick = true;
                }
            };

        }
        private void RemoveTask()
        {
            new Task(() =>
            {
                for (int i = 100; i > 0; i--)
                {
                    Dispatcher.Invoke(() => { LoadingFrame.Opacity = i / 100.0; });
                    System.Threading.Thread.Sleep(10);
                }
                Dispatcher.Invoke(() => { General.Children.Remove(LoadingFrame); });
            }).Start();
        }
        private void registerUser(Grid grid)
        {
            grid.MouseEnter += (sender, args) =>
            {
               // ((Canvas)(grid.Children.OfType<Border>().FirstOrDefault().Child)).Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            };
            grid.MouseLeave += (sender, args) =>
            {
               // ((Canvas)(grid.Children.OfType<Border>().FirstOrDefault().Child)).Background = new SolidColorBrush(Color.FromArgb(255, 249, 249, 249));
            };
            grid.MouseDown += (sender, args) =>
            {
                userinfoVisibility = !userinfoVisibility;
                if(userinfoVisibility) Popup.Visibility = Visibility.Visible;
                double onPercent = Popup.ActualHeight / 100;
                if (userinfoVisibility)
                {
                    Popup.Margin = new Thickness(0, -Popup.ActualHeight, 0, Popup.ActualHeight);
                    new Task(() =>
                    {

                        for (int i = 1; i < 101; i++)
                        {
                            Dispatcher.Invoke(() => { Popup.Margin = new Thickness(0, Popup.Margin.Top + onPercent, 0, Popup.Margin.Bottom - onPercent); });
                            System.Threading.Thread.Sleep(1);
                        }
                    }).Start() ;
                }
                else
                {
                    Popup.Margin = new Thickness(0, 0, 0, 0);
                    new Task(() =>
                    {
                        for (int i = 1; i < 101; i++)
                        {
                            Dispatcher.Invoke(() => { Popup.Margin = new Thickness(0, Popup.Margin.Top - onPercent, 0, Popup.Margin.Bottom + onPercent); });
                            System.Threading.Thread.Sleep(1);
                        }
                        Dispatcher.Invoke(() => { Popup.Visibility = Visibility.Hidden; });
                    }).Start();
                }
            };
        }
        private void registerButton(Grid grid)
        {
            grid.MouseEnter += (sender, args) =>
            {
               // if(grid != selectedGrid) grid.Children.OfType<Canvas>().FirstOrDefault().Background = new SolidColorBrush(Color.FromArgb(255, 111, 235, 254));
            };
            grid.MouseLeave += (sender, args) =>
            {
                //if (grid != selectedGrid) grid.Children.OfType<Canvas>().FirstOrDefault().Background = new SolidColorBrush(Color.FromArgb(255, 140, 240, 255));
            };
            grid.MouseDown += (sender, args) =>
            {
                selectedGrid.Children.OfType<Image>().FirstOrDefault().Visibility = Visibility.Hidden;
                selectedGrid = grid;
                selectedGrid.Children.OfType<Image>().FirstOrDefault().Visibility = Visibility.Visible;

                CurrentSource.Source = GetPage();
            };

        }
        private Uri GetPage()
        {
            if (selectedGrid == Menu_1) return new Uri("DashboardPages/Home_Page.xaml", UriKind.Relative);
            if (selectedGrid == Menu_2) return new Uri("DashboardPages/Share_Page.xaml", UriKind.Relative);
            if (selectedGrid == Menu_3) return new Uri("DashboardPages/Notice_Page.xaml", UriKind.Relative);
            if (selectedGrid == Menu_4) return new Uri("DashboardPages/List_Page.xaml", UriKind.Relative);
            return null;
        }
        private void DateEvent(object sender, EventArgs args)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.Append(DateTime.Now.ToString("yyyy / MM / dd"));
            builder.Append("\n");
            builder.Append(DateTime.Now.ToString("HH : mm"));
            Date.Text = builder.ToString();
        }
    }
}
