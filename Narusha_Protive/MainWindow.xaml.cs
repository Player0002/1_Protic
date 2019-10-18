using Narusha_Protive.DataManagers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using TeamProgramWithDatabase;
using System.Threading;


namespace Narusha_Protive
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Http_Getcs httpsGet = new Http_Getcs();
        public MainWindow()
        {
            InitializeComponent();
            JObject kobj = httpsGet.checkAutoLogin();
            if (kobj != null && kobj["name"].ToString() != "null")
            {
                UserData.id = Convert.ToInt64(kobj["id"].ToString());
                UserData.username = kobj["name"].ToString();
                UserData.teamCode = kobj["teamCode"].ToString();
                UserData.email = kobj["email"].ToString();
                UserData.password = kobj["password"].ToString();
                UserData.memos = JsonConvert.DeserializeObject<int[]>(kobj["memo"].ToString());
                UserData.userMemos = JsonConvert.DeserializeObject<string[]>(kobj["usermemo"].ToString());
                httpsGet.InitialzationTeam();

                Dashboard board = new Dashboard();
                board.WindowState = this.WindowState;
                board.Show();
                this.Close();
            }
            Register.MouseDown += (sender, obj) =>
            {
                Register register = new Register();
                register.Show();
            };

            this.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter) login();
            };
            /*Register Button Effect System*/
            Register.MouseEnter += (sender, obj) =>
            {
                //Register.Foreground = new SolidColorBrush(Color.FromArgb(255, 120,120,120));
                
            };

            Register.MouseLeave += (sender, obj) =>
            {
               //Register.Foreground = new SolidColorBrush(Color.FromArgb(255, 85,85,85));
            };

            LoginButton.PreviewMouseDown += (sender, obj) => {
                login();
            };
            
        }
        private void login()
        {
            try
            {
                Http_Getcs getcs = new Http_Getcs();
                JObject objs = objs = getcs.Login(idBox.Text, passwordBox.Password);
                new Thread(() =>
                {
                    if (objs != null && objs["name"].ToString() != "null")
                    {
                        UserData.id = Convert.ToInt64(objs["id"].ToString());
                        UserData.username = objs["name"].ToString();
                        UserData.teamCode = objs["teamCode"].ToString();
                        UserData.email = objs["email"].ToString();
                        UserData.password = objs["password"].ToString();
                        UserData.memos = JsonConvert.DeserializeObject<int[]>(objs["memo"].ToString());
                        UserData.userMemos = JsonConvert.DeserializeObject<string[]>(objs["usermemo"].ToString());
                        httpsGet.InitialzationTeam();

                        Dispatcher.Invoke(() =>
                        {
                            if (IsAutoLogin.IsChekced)
                            {
                                JObject objs2 = httpsGet.registerAutologin(idBox.Text, UserData.password);
                                if (objs2 == null) return;
                            }

                            Dashboard board = new Dashboard();
                            board.WindowState = this.WindowState;
                            board.Show();
                            this.Close();
                        });
                    }
                    else
                    {
                        MessageBox.Show("Check user account");
                    }
                }).Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
