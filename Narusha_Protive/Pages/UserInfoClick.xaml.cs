using Narusha_Protive.DataManagers;
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
    /// UserInfoClick.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfoClick : Page
    {
        Http_Getcs getcs = new Http_Getcs();
        public UserInfoClick()
        {
           InitializeComponent();
            Loaded += (sender, args) =>
            {
                UserName.Content = UserData.username;
                TeamName.Content = TeamData.name;
            };
            registerButton(Settings);
            registerButton(Undefined);
            registerButton(Logout);
             Logout.MouseDown += (sender, args) =>
             {
                 getcs.removeAutoLogin();
                 MainWindow window = new MainWindow();
                 window.WindowState = UserData.board.WindowState;
                 window.Show();
                 UserData.board.Close();

             };
        }
        private void registerButton(Grid grid)
        {
            grid.MouseEnter += (sender, args) =>
            {
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            };
            grid.MouseLeave += (sender, args) =>
            {
                grid.Background = new SolidColorBrush(Colors.White);
            };
        }
    }
}
