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
    /// CircleCounter.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CircleCounter : UserControl
    {
        public Test test = new Test { Count = 0, Backgrounds = new SolidColorBrush(Color.FromArgb(255, 233, 143, 255)) };
        public DownloadData data = DownloadData.getInstance();
        public CircleCounter()
        {
            InitializeComponent();
            DataContext = test;
           
            data.fileAdd += () => { test.incress(); Counter.Content = test.Count; };
            data.fileCompleted += () => { test.decress(); Counter.Content = test.Count; };
        }
    }
    public class Test
    {
        private int count;
        private SolidColorBrush background;
        public SolidColorBrush Backgrounds
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }
        public void change()
        {
            Backgrounds = new SolidColorBrush(Color.FromArgb(255, 12, 25, 26));
        }
        public void incress()
        {
            Count++;
        }
        public void decress()
        {
            Count--;
        }
    }
}
