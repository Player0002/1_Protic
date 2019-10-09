using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using Narusha_Protive.Pages.SubFrames;

namespace Narusha_Protive.DashboardPages
{
    /// <summary>
    /// Home_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Home_Page : Page
    {
        bool isEnterUploadFile = false;
        bool isEnterNotice = false;
        bool isEnterToDoList = false;
        bool isEnterMemo = false;
        double UploadFiles_scalex = 1;
        double Notice_scalex = 1;
        double ToDoList_scalex = 1;
        double Memo_scalex = 1;
        DispatcherTimer timer = new DispatcherTimer();
        UploadedFilePages pages = new UploadedFilePages();
        NewNoticePages pages2 = new NewNoticePages();
        MemoFramePage pages3 = new MemoFramePage();
        public Home_Page()
        {

            InitializeComponent();
            registerZoomAnimation(UploadFiles);
            registerZoomAnimation(NewNotice);
            registerZoomAnimation(ToDoList);
            registerZoomAnimation(Memo);
            UploadFrame.Source = new Uri("Pages/SubFrames/UploadedFilePages.xaml", UriKind.Relative);
            UploadFrame.NavigationService.Navigate(pages);

            NoticeFrame.Source = new Uri("Pages/SubFrames/NewNoticePages.xaml", UriKind.Relative);
            NoticeFrame.NavigationService.Navigate(pages2);

            MemoFrame.Source = new Uri("Pages/SubFrames/MemoFramePage.xaml", UriKind.Relative);
            MemoFrame.NavigationService.Navigate(pages3);

            TeamName.Content = "TEAM " + TeamData.name;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (sender, args) =>
            {
                if (UserData.isOutsideClick)
                {
                    UserData.isOutsideClick = false;
                    Leave(Memo);
                    Leave(NewNotice);
                }
                if (UserData.fileUpdated)
                {
                    UserData.fileUpdated = false;
                    Console.WriteLine("UPDATECCCCCCCCCCCCCCCCCCCccc");
                    pages = new UploadedFilePages();
                    UploadFrame.Source = new Uri("Pages/SubFrames/UploadedFilePages.xaml", UriKind.Relative);
                    UploadFrame.NavigationService.Navigate(pages);
                }
                if (UserData.noticeUpdated)
                {
                    UserData.noticeUpdated = false;
                    pages2.addNotices();
                }
                if (UserData.memoUpdated)
                {
                    UserData.memoUpdated = false;
                    pages3.AddMemos();
                }
            };
            timer.Start();
        }
        
        private double GetScale(Grid grid)
        {
            if (grid == NewNotice)
                return Notice_scalex;
            if (grid == ToDoList)
                return ToDoList_scalex;
            if (grid == Memo)
                return Memo_scalex;
            return UploadFiles_scalex;
        }
        private void setScale(double value, Grid grid)
        {
            if (grid == NewNotice)
                 Notice_scalex = value;
            else if (grid == ToDoList)
                ToDoList_scalex = value;
            else if (grid == Memo) Memo_scalex = value;
            else UploadFiles_scalex = value;
        }
        private bool getIsEnter(Grid grid)
        {
            if (grid == NewNotice)
                return isEnterNotice;
            if (grid == ToDoList)
                return isEnterToDoList;
            if (grid == Memo)
                return isEnterMemo;
            return isEnterUploadFile;
        }
        private void setIsEnter(bool value, Grid grid)
        {
            if (grid == NewNotice)
                isEnterNotice = value;
            if (grid == ToDoList)
                isEnterToDoList = value;
            if (grid == Memo)
                isEnterMemo = value;
            if(grid == UploadFiles) isEnterUploadFile = value;
        }
        private void registerZoomAnimation(Grid grid)
        {
            grid.MouseEnter += (sender, args) =>
            {
                Console.WriteLine("DATA : " + ((grid == Memo ? UserData.isStop : true) || (grid == Memo ? UserData.isEnter : false)));
                if (grid == Memo ? UserData.cnt == 0 :  grid == NewNotice ? !NewNoticeDataM.IsShowed : true)
                {
                    double amount = GetScale(grid);
                    new Task(() =>
                    {
                        setIsEnter(true, grid);
                        while (amount < 1.1)
                        {
                            if (!getIsEnter(grid)) break;
                            Dispatcher.Invoke(() =>
                            {
                                grid.RenderTransformOrigin = new Point(0.5, 0.5);
                                grid.RenderTransform = new ScaleTransform(amount, amount);
                            });
                            amount += 0.001;
                            setScale(amount, grid);
                            Thread.Sleep(1);
                        }

                    }).Start();
                }
            };

            grid.MouseLeave += (sender, args) =>
            {
                Leave(grid);    
            };
        }
        private void Leave(Grid grid)
        {
            if (grid == Memo ? UserData.cnt == 0 : grid == NewNotice ? !NewNoticeDataM.IsShowed : true)
            {
                double amount = GetScale(grid);
                new Task(() =>
                {
                    setIsEnter(false, grid);
                    while (amount > 1)
                    {
                        if (getIsEnter(grid)) break;
                        Dispatcher.Invoke(() =>
                        {
                            grid.RenderTransformOrigin = new Point(0.5, 0.5);
                            grid.RenderTransform = new ScaleTransform(amount, amount);
                        });
                        amount -= 0.001;
                        setScale(amount, grid);
                        Thread.Sleep(1);
                    }
                }).Start();
            }
        }
    }
}
