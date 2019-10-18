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

namespace Narusha_Protive.CustomControls
{
    /// <summary>
    /// SharePageController.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SharePageController : UserControl
    {
        int maxpage = 3;
        int currentpage = 1;

        public delegate void pageChange();
        public event pageChange next;
        public event pageChange undo;
        public SharePageController()
        {
            InitializeComponent(); update();

            Next.PreviewMouseDown += (sender, events) =>
            {
                if(currentpage + 1 <= maxpage)
                {
                    currentpage++;
                    next();
                }
            };
            Undo.PreviewMouseDown += (sender, events) =>
            {
                if(currentpage - 1 != 0)
                {
                    currentpage--;
                    undo();
                }
            };
            next += () =>
            {
                update();
            };
            undo += () =>
            {
                update();
            };
        }
        private void update()
        {
            Str.Content = currentpage + " / " + maxpage;
            if (maxpage == 0)
            {
                Undo.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/undoCant.png"));
                Next.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/nextCant.png"));
                Undo.ForceCursor = false;
                Next.ForceCursor = false;
                Next.Cursor = Cursors.Arrow;
                Undo.Cursor = Cursors.Arrow;
            }
            else if (currentpage == maxpage)
            {
                if(currentpage == 2)
                    Undo.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/undo.png"));
                Next.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/nextCant.png"));
                Next.ForceCursor = false;
                Next.Cursor = Cursors.Arrow;
                Undo.Cursor = maxpage == 1 ? Cursors.Arrow : Cursors.Hand;
            }
            else if (currentpage <= 1)
            {
                if(maxpage != 1)
                    Next.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/next.png"));
                Undo.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/undoCant.png"));
                Undo.ForceCursor = false;
                Undo.Cursor = Cursors.Arrow;
                Next.Cursor = Cursors.Hand;
            }
            else
            {
                Undo.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/undo.png"));
                Next.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/Share_icons/next.png"));
                Undo.ForceCursor = true;
                Next.ForceCursor = true;
                Next.Cursor = Cursors.Hand;
                Undo.Cursor = Cursors.Hand;
            }
        }
        public void setPage(int page)
        {
            currentpage = page;
            update();
        }
        public void setMaxPage(int page)
        {
            if (page == 0) return;
            maxpage = page; update();

        }
    }
}
