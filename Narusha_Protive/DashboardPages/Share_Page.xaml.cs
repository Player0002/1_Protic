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

namespace Narusha_Protive.DashboardPages
{
    /// <summary>
    /// Share_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Share_Page : Page
    {
        public Share_Page()
        {
            InitializeComponent();
            VisiableGrid.SizeChanged += (sender, events) =>
            {
                double ActualWidths = ActualWidth - 60;
                double ActualHeights = ActualHeight - 60 - 100;
                VisiableGrid.Children.Clear();
                VisiableGrid.ColumnDefinitions.Clear();
                VisiableGrid.RowDefinitions.Clear();
                int cnt = (int)Math.Floor(ActualWidths / 266.25); // Visiable Area Length ( Widths )
                int hcnt = (int)Math.Floor(ActualHeights / 171);
                double another = ActualWidths - cnt * 266.25; // Side Area check ( Cause we will make this to center )
                double tanother = ActualHeights - (hcnt * 171);
                int current = -3;
                int rcurrent = -3;
                if(another > 0)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    col.Width = new GridLength(another/2);
                    VisiableGrid.ColumnDefinitions.Add(col);
                    current++;
                }
                if (tanother > 0)
                {
                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(tanother / 2);
                    VisiableGrid.RowDefinitions.Add(row);
                    rcurrent++;
                }
                /*Adding Grid*/
                for (int j = 0; j < hcnt ; j++)
                {
                    RowDefinition row = new RowDefinition();
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
                for (int i = 0; i < hcnt; i++)
                {
                    rcurrent += 3;

                    for (int j = 0; j < cnt; j++)
                    {
                        current += 3;
                        Canvas c = new Canvas();
                        c.Width = 236;
                        c.Height = 150;
                        Canvas c2 = new Canvas();
                        c2.Width = 30.25 / 2;
                        c2.Height = 150;
                        Canvas c3 = new Canvas();
                        c3.Width = 30.25 / 2;
                        c3.Height = 150;
                        c.Background = new SolidColorBrush(Colors.Beige);
                        c2.Background = new SolidColorBrush(Colors.Red);
                        c3.Background = new SolidColorBrush(Colors.Black);
                        Grid.SetColumn(c, current + 1);
                        Grid.SetColumn(c2, current + 2);
                        Grid.SetColumn(c3, current);
                        Grid.SetRow(c, rcurrent + 1);
                        Grid.SetRow(c3, rcurrent + 1);
                        Grid.SetRow(c2, rcurrent + 1);
                        VisiableGrid.Children.Add(c);
                        VisiableGrid.Children.Add(c2);
                        VisiableGrid.Children.Add(c3);
                    }
                    current = another > 0 ? -2 : -3;
                }
                VisiableGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });                    
            };
        }
    }
}
