using Narusha_Protive.CustomControls;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Narusha_Protive.DashboardPages
{
    /// <summary>
    /// Share_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Share_Page : Page
    {
        public static Share_Page instance = null;
        int page = 0;
        SharePageController control = new SharePageController();
        DispatcherTimer timer = new DispatcherTimer();

        public static Share_Page getInstance()
        {
            if (instance == null) instance = new Share_Page();
            return instance;
        }
        public Share_Page()
        {
            InitializeComponent(); control.setMaxPage(2);
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (sender, args) =>
            {
                if (TeamData.Updated)
                {
                    TeamData.Updated = false;
                    update();
                }
            };
            timer.Start();
            VisiableGrid.SizeChanged += (sender, events) =>
            {
                this.page = 0;
                control.setPage(page + 1);
                update();
                control.setPage(page + 1);
            };
            control.next += () =>
            {
                page++;
                update();
            };
            control.undo += () =>
            {
                page--;
                update();
            };
        }
        public void update()
        {
          
            double ActualWidths = ActualWidth - 60;
            double ActualHeights = ActualHeight - 60 - 60;
            VisiableGrid.Children.Clear();
            VisiableGrid.ColumnDefinitions.Clear();
            VisiableGrid.RowDefinitions.Clear();
            int cnt = (int)Math.Floor(ActualWidths / 266.25); // Visiable Area Length ( Widths )
            int hcnt = (int)Math.Floor(ActualHeights / 171);
            int shouldShowDatas = TeamData.files == null ? 0 : TeamData.files.Length;
            int count = page == 0 ? 0 : page * cnt * hcnt;
            double another = ActualWidths - cnt * 266.25; // Side Area check ( Cause we will make this to center )
            double tanother = ActualHeights - (hcnt * 171);
            int current = -3;
            int rcurrent = -3;
            control.setMaxPage(shouldShowDatas > cnt * hcnt ? (int)Math.Ceiling(shouldShowDatas / (double)(cnt * hcnt)) : 1);
            RowDefinition row = new RowDefinition();
            if (another > 0)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(another / 2);
                VisiableGrid.ColumnDefinitions.Add(col);
                current++;
            }
            if (tanother > 0)
            {
                row = new RowDefinition();
                row.Height = new GridLength(tanother / 2);
                VisiableGrid.RowDefinitions.Add(row);
                rcurrent++;
            }
            /*Adding Grid*/
            for (int j = 0; j < hcnt; j++)
            {
                row = new RowDefinition();
                row.Height = new GridLength(10.5);
                VisiableGrid.RowDefinitions.Add(row);
                row = new RowDefinition();
                row.Height = new GridLength(150);
                VisiableGrid.RowDefinitions.Add(row);
                row = new RowDefinition();
                row.Height = new GridLength(10.5);
                VisiableGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < cnt; i++)
            {
                ColumnDefinition col1 = new ColumnDefinition();
                col1.Width = new GridLength(30.25 / 2);
                ColumnDefinition col2 = new ColumnDefinition();
                col2.Width = new GridLength(236);
                ColumnDefinition col3 = new ColumnDefinition();
                col3.Width = new GridLength(30.25 / 2);
                VisiableGrid.ColumnDefinitions.Add(col1);
                VisiableGrid.ColumnDefinitions.Add(col2);
                VisiableGrid.ColumnDefinitions.Add(col3);
            }
            /* Setter */
            Random rnd = new Random();

            for (int i = 0; i < ((shouldShowDatas - (cnt * hcnt * page)) / cnt +1 > hcnt ? hcnt : (shouldShowDatas - (cnt * hcnt * page)) / cnt + 1); i++)
            {
                rcurrent += 3;
                var identitys = DownloadData.getInstance().accessFiles;
                for (int j = 0; j < ((shouldShowDatas - (cnt * hcnt * page)) - (cnt * i) > cnt ? cnt : (shouldShowDatas - (cnt * hcnt * page)) - (cnt * i)); j++)
                {
                    var str = identitys.ToArray()[rnd.Next(identitys.Count())];
                    current += 3;
                    ShareUI ui = new ShareUI();
                    ui.Width = 236;
                    ui.Height = 150;
                    ui.setName(TeamData.files[count++]);
                    Grid.SetColumn(ui, current + 1);
                    Grid.SetRow(ui, rcurrent + 1);
                    VisiableGrid.Children.Add(ui);
                }
                current = another > 0 ? -2 : -3;
            }
            row = new RowDefinition();
            row.Height = new GridLength(100);
            VisiableGrid.RowDefinitions.Add(row);

            control.Height = 60;
            Grid.SetRow(control, 3 * hcnt + 3);
            Grid.SetColumnSpan(control, 3 * cnt + (another > 0 ? 1 : 0) + 1);
            control.HorizontalAlignment = HorizontalAlignment.Center;
            control.VerticalAlignment = VerticalAlignment.Top;
            VisiableGrid.Children.Add(control);
        }
    }
}

//Grid.SetColumn(c2, current + 2);
//Grid.SetColumn(c3, current);

//Grid.SetRow(c3, rcurrent + 1);
//Grid.SetRow(c2, rcurrent + 1);