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

namespace Narusha_Protive.Pages
{
    /// <summary>
    /// LoadingScreen.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingScreen : Page
    {
        public LoadingScreen()
        {
            InitializeComponent();
            Label.Visibility = Visibility.Hidden;
            new Task(() =>
            {
                System.Threading.Thread.Sleep(200);
               /* for (int i = 1; i <= 180; i ++)
                {
                    Dispatcher.Invoke(() => { LogoImg.RenderTransform = new RotateTransform(i * 2); });
                    System.Threading.Thread.Sleep(1);
                }*/
                System.Threading.Thread.Sleep(10);
                Dispatcher.Invoke(() => { Label.Content = ""; });
                Dispatcher.Invoke(() => { Label.Visibility = Visibility.Visible; });
                var array = new StringBuilder("Welcome ").Append(UserData.username).ToString().ToCharArray();
                foreach (var s in array)
                {
                    Dispatcher.Invoke(() => { Label.Content = Label.Content + s.ToString(); });
                    System.Threading.Thread.Sleep(50);
                    Console.WriteLine(s);
                }
                
            }).Start();
        }
    }
}
