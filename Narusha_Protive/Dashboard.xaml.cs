using Narusha_Protive.CustomControls;
using Narusha_Protive.DashboardPages;
using Narusha_Protive.Pages;
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
        private bool showDownloadState = false;
        private bool isLoading = false;
        Frame ndownload_dialog = null;
        download_info info = new download_info();
        CurrentDownloadState stats = new CurrentDownloadState();

        UserInfoClick click = new UserInfoClick();

        Frame userinfo_dialog = null;
        Frame download_dialog = null;
        public Dashboard()
        {
            InitializeComponent();
            UserData.board = this;
            isLoading = true;
            SizeChanged += (sender, args) =>
            {
                if (userinfo_dialog != null)
                    userinfo_dialog.Margin = new Thickness(MainGeneral.ActualWidth - 300, (25 / 326.0) * MainGeneral.ActualHeight, 0, 0);
                if (download_dialog != null)
                    download_dialog.Margin = new Thickness(MainGeneral.ActualWidth - 340, (25 / 326.0) * MainGeneral.ActualHeight, 0, 0);
            };
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
            }).Start(); //Delete Loading..


            new Task(() =>
            {
                timer_Date.Interval = TimeSpan.FromSeconds(0.5);
                timer_Date.Tick += DateEvent;
                selectedGrid = Menu_1;
                Dispatcher.Invoke(() =>
                {
                    //userName.Content = UserData.username;
                    timer_Date.Start();
                    registerButton(Menu_1);
                    registerButton(Menu_2);
                    registerButton(Menu_3);
                    registerButton(Menu_4);
                    registerUser(UserInfo);
                });

                isLoading = false;
            }).Start();
            DownloadedFiles.MouseMove += (sender, args) =>
            {
                download_move();
            };
            DownloadedFiles.MouseLeave += (sender, args) =>
        {
                if (ndownload_dialog != null) MainGeneral.Children.Remove(ndownload_dialog);
                ndownload_dialog = null;
            };
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


            DownloadedFiles.MouseDown += (sender, events) =>
            {
                showDownloadState = !showDownloadState;
                if (showDownloadState)
                {
                    
                    if (userinfoVisibility)
                    {
                        userinfoVisibility = !userinfoVisibility;
                        if (userinfo_dialog != null) MainGeneral.Children.Remove(userinfo_dialog);
                        userinfo_dialog = null;
                    }
                    if (download_dialog != null) MainGeneral.Children.Remove(download_dialog);
                    download_dialog = new Frame();
                    download_dialog.Width = 340;
                    download_dialog.Height = 260;
                    MainGeneral.Children.Add(download_dialog);
                    var point = Mouse.GetPosition(MainGeneral);
                    download_dialog.HorizontalAlignment = HorizontalAlignment.Left;
                    download_dialog.VerticalAlignment = VerticalAlignment.Top;
                    download_dialog.Margin = new Thickness(MainGeneral.ActualWidth - 340, (25 / 326.0) * MainGeneral.ActualHeight, 0, 0);
                    download_dialog.Source = new Uri("Pages/CurrentDownloadState.xaml", UriKind.Relative);
                    download_dialog.NavigationService.Navigate(stats);
                    if (ndownload_dialog != null)
                    {
                        download_move();
                    }
                }
                else
                {
                    if (download_dialog != null) MainGeneral.Children.Remove(download_dialog);
                    download_dialog = null;
                }
            };
        }
        private void download_move()
        {
            if (ndownload_dialog != null) MainGeneral.Children.Remove(ndownload_dialog);
            ndownload_dialog = new Frame();
            ndownload_dialog.Width = ActualWidth / 5;
            ndownload_dialog.Height = (15 / 326.0) * ActualHeight;
            this.MainGeneral.Children.Add(ndownload_dialog);
            var point = Mouse.GetPosition(MainGeneral);
            ndownload_dialog.HorizontalAlignment = HorizontalAlignment.Left;
            ndownload_dialog.VerticalAlignment = VerticalAlignment.Top;
            ndownload_dialog.Margin = new Thickness(point.X - ndownload_dialog.Width, point.Y, 0, 0);
            ndownload_dialog.IsHitTestVisible = false;
            ndownload_dialog.Source = new Uri("Pages/Popup/download_info.xaml", UriKind.Relative);
            ndownload_dialog.NavigationService.Navigate(info);
            info.setCount(Counter.test.Count);
            
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
                //if(userinfoVisibility) if(userinfo_dialog != null) userinfo_dialog.Visibility = Visibility.Visible;
                if (userinfoVisibility)
                {
                    if (showDownloadState)
                    {
                        showDownloadState = !showDownloadState;
                        if (download_dialog != null) MainGeneral.Children.Remove(download_dialog);
                        download_dialog = null;
                    }
                    if (userinfo_dialog != null) MainGeneral.Children.Remove(userinfo_dialog);
                    userinfo_dialog = new Frame();
                    userinfo_dialog.Width = 300;
                    userinfo_dialog.Height = 300;
                    MainGeneral.Children.Add(userinfo_dialog);
                    var point = Mouse.GetPosition(MainGeneral);
                    userinfo_dialog.HorizontalAlignment = HorizontalAlignment.Left;
                    userinfo_dialog.VerticalAlignment = VerticalAlignment.Top;
                    userinfo_dialog.Margin = new Thickness(MainGeneral.ActualWidth-300, (25 / 326.0) * MainGeneral.ActualHeight, 0, 0);
                    userinfo_dialog.Source = new Uri("Pages/UserInfoClick.xaml", UriKind.Relative);
                    userinfo_dialog.NavigationService.Navigate(click);

                    
                    //userinfo_dialog.Margin = new Thickness(0, -userinfo_dialog.ActualHeight, 0, userinfo_dialog.ActualHeight);
                    /*new Task(() =>
                    {

                        for (int i = 1; i < 101; i++)
                        {
                            Dispatcher.Invoke(() => { userinfo_dialog.Margin = new Thickness(0, userinfo_dialog.Margin.Top + onPercent, 0, userinfo_dialog.Margin.Bottom - onPercent); });
                            System.Threading.Thread.Sleep(1);
                        }
                    }).Start() ;*/
                }
                else
                {
                    new Task(() =>
                    {
                        Dispatcher.Invoke(() => {
                            if (userinfo_dialog != null) MainGeneral.Children.Remove(userinfo_dialog);
                            userinfo_dialog = null;
                        });
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

                Uri uri = GetPage();
                CurrentSource.Source = uri;
                if (grid == Menu_2) CurrentSource.Navigate(Share_Page.getInstance());
                else CurrentSource.NavigationService.Navigate(uri);
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
