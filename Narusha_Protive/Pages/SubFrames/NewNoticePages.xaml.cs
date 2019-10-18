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
using Narusha_Protive.CustomControls;
using Narusha_Protive.DataManagers;

namespace Narusha_Protive.Pages.SubFrames
{
    /// <summary>
    /// NewNoticePages.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewNoticePages : Page
    {
        //새로 만들어질 경우
        public NewNoticePages()
        {
            InitializeComponent();
            
            SCROLL.PreviewMouseWheel += (sender, obj) =>
            {
                if (NewNoticeDataM.IsShowed) obj.Handled = true;
                else obj.Handled = false;
            }; // 선택이 된경우 스크롤을 방지하기 위함 ( 이후 추가
            this.Loaded += (sender, args) =>
            {
                Dispatcher.Invoke(() => { Load.Visibility = Visibility.Hidden; });
            };
            this.SizeChanged += (sender, obj) =>
            {
                //
                Notices.Children.Clear();

                Load.Visibility = Visibility.Visible;
                if (TeamData.notice != null)
                {
                    foreach (Notice notice in TeamData.notice)
                    {
                        if (notice.users != null && notice.users.Contains(UserData.id)) continue;
                        {
                            NewNoticeUI ui = new NewNoticeUI();
                            ui.setNotice(notice);
                            ui.setDelegate(this);
                            ui.Height = MainGrid.ActualHeight / 2;
                            ui.NoticeLabel.Content = notice.message;
                            ui.DateLabel.Content = DataFormatter.toDate(notice.when);
                            Notices.Children.Add(ui);
                            
                        }
                    }
                    

                }
                Load.Visibility = Visibility.Hidden;

            };

        }
        public void addNotices()
        {
            foreach(Notice n in TeamData.AddNotices)
            {
                

                Dispatcher.Invoke(() =>
                {
                    NewNoticeUI ui = new NewNoticeUI();
                    ui.setNotice(n);
                    ui.setDelegate(this);
                    ui.Height = MainGrid.ActualHeight / 2;
                    ui.NoticeLabel.Content = n.message;
                    ui.DateLabel.Content = DataFormatter.toDate(n.when);
                    Notices.Children.Insert(0, ui);
                });
            }
            TeamData.AddNotices = null;
        }
        public void update(NewNoticeUI uis)
        {
            Notices.Children.Remove(uis);
        }
    }
}
