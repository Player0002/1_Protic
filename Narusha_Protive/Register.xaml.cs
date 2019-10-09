using Narusha_Protive.DataManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeamProgramWithDatabase
{
    public class ComboBoxItem
    {
        public string name { get; set; }
    }
    public partial class Register : Window
    {
        private List<ComboBoxItem> list = new List<ComboBoxItem>
            {
                new ComboBoxItem() {name = "naver.com"},
                new ComboBoxItem() {name = "gmail.com"},
                new ComboBoxItem() {name = "daum.net"},
                new ComboBoxItem() {name = "hanmail.net"},
                new ComboBoxItem() {name = "dgsw.hs.kr"},
                new ComboBoxItem() {name = "직접입력"},
            };
        private bool isCanUseUsername = false;
        private bool isCanUseEmail = false;
        private bool isCanUsePassword = false;
        private bool isCanuseTeamName = false;
        private Timers timer = null;
        public Register()
        {
            InitializeComponent();
            //Hide 이메일 인증 버튼
            EmailButton.Visibility = Visibility.Hidden;
            EmailVerifyCanvas.Visibility = Visibility.Hidden;

            EmailCombobox.SelectionChanged += ChangeCombox;
            
            //EmailCombobox Lists
            EmailCombobox.ItemsSource = list;

            EmailBox.LostKeyboardFocus += EmailBoxTestChanged;
            EmailTextBox.TextChanged += (sender, args) => { EmailBoxTestChanged(sender, null); };

            

            PasswordBox.PasswordChanged += PasswordBoxChanged;
            PasswordRepeatBox.PasswordChanged += PasswordRepeatBoxChanged;

            TeamCodeBox.TextChanged += TeamCodeBoxChanged;

            IsNewTeam.Content = "";

            userNameBox.LostKeyboardFocus += (sender, args) => { UserNameBox_TextChanged(sender, null); } ;

            //INitizalcation
            isCanUseEmail = false;
            EmailButton.IsHitTestVisible = true;
            VerfiyButton.IsHitTestVisible = true;
            EmailBox.IsReadOnly = false;
            EmailTextBox.IsReadOnly = false;
            EmailCombobox.IsReadOnly = false;
            EmailCombobox.IsHitTestVisible = true;
            VerifyBox.IsReadOnly = false;
        }
        
        private void TeamCodeBoxChanged(object sender, TextChangedEventArgs e)
        {
            string text = TeamCodeBox.Text;
            TeamCodeVerify vir = DataManager.CheckStr(text);
            Console.WriteLine("DATA - " + vir.ToString());
            switch (vir)
            {
                case TeamCodeVerify.None:
                    IsNewTeam.Content = "새로운 팀을 생성하시는군요!    팀명은 입력하신 정보로 기록됩니다.";
                    isCanuseTeamName = true;
                    IsNewTeam.Foreground = Brushes.Green;
                    break;
                case TeamCodeVerify.TeamCodeContains:
                    IsNewTeam.Content = "팀을 발견했습니다!    팀명은 \" " + DataManager.GetTeamName(text) +" \" 입니다!";
                    isCanuseTeamName = true;
                    IsNewTeam.Foreground = Brushes.Green;
                    break;
                case TeamCodeVerify.TeamNameContains:
                    IsNewTeam.Content = "이미 존재하는 팀입니다!    다른 이름을 입력하거나 팀코드를 입력해주세요.";
                    isCanuseTeamName = false;
                    IsNewTeam.Foreground = Brushes.Red;
                    break;
                case TeamCodeVerify.Empty:
                    IsNewTeam.Content = "이곳을 공백으로 남겨둘순 없습니다.";
                    isCanuseTeamName = false;
                    IsNewTeam.Foreground = Brushes.Red;
                    break;
            }
        }

        private void PasswordBoxChanged(object sender, RoutedEventArgs args)
        {
            bool isTruePassword = Regex.IsMatch(PasswordBox.Password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,35}$");
            if (isTruePassword) PasswordWarring.Foreground = Brushes.Green;
            else PasswordWarring.Foreground = Brushes.Red;
        }

        private void PasswordRepeatBoxChanged(object sender, RoutedEventArgs args)
        {
            if (PasswordBox.Password.Equals(PasswordRepeatBox.Password))
            {
                PasswordCanUseSetter(true);
                return;
            }
            PasswordCanUseSetter(false);
        }
        private void PasswordCanUseSetter(bool canuse)
        {
            if (canuse)
            {
                PasswordRepeatWarring.Foreground = Brushes.Green;
                PasswordRepeatWarring.Content = "비밀번호가 일치합니다!";
                isCanUsePassword = true;
            }
            else
            {
                PasswordRepeatWarring.Foreground = Brushes.Red;
                PasswordRepeatWarring.Content = "비밀번호를 확인해주세요.";
                isCanUsePassword = false;
            }
        }

        private void EmailBoxTestChanged(object sender, KeyboardEventArgs args)
        {
            string text = EmailBox.Text;
            string subEmail = EmailTextBox.Text;
            string email = text + "@" + EmailTextBox.Text;

            bool isCanUse = false;
            bool OnlyKE = true;

            if (text.Length <= 0 || subEmail.Length <= 0)
            {
                EmailCanUseSetter(false);
                return;
            }
            isCanUse = DataManager.isCanUseEmail(email);

            OnlyKE = Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");

            if(isCanUse && OnlyKE)
            {
                EmailCanUseSetter(true);
            }
            else
            {
                EmailCanUseSetter(false);
            }
            return;
        }
        private void EmailCanUseSetter(bool isCanUse)
        {
            if (isCanUse)
            {
                EmailWarrings.Foreground = Brushes.Gray;
                CanUseThisEmail.Content = "이 이메일은 사용할 수 있습니다.";
                CanUseThisEmail.Foreground = Brushes.Green;
                EmailButton.Visibility = Visibility.Visible;
                isCanUseEmail = true;
            }
            else
            {
                EmailWarrings.Foreground = Brushes.Red;
                CanUseThisEmail.Content = "이 이메일은 사용할 수 없거나 이미 등록되었습니다.";
                CanUseThisEmail.Foreground = Brushes.Red;
                EmailButton.Visibility = Visibility.Hidden;
                isCanUseEmail = false;
            }
        }

        private void ChangeCombox(object sender, SelectionChangedEventArgs args)
        {
            int a = EmailCombobox.SelectedIndex;
            if (a > -1 && a < 5) EmailTextBox.Text = list[EmailCombobox.SelectedIndex].name;
            else EmailTextBox.Text = "";
            EmailBoxTestChanged(sender, null);
        }
        //TextChanged="UserNameBox_TextChanged"
        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            string text = userNameBox.Text.ToLower();
            bool isCanuse = false;
            bool ContainsEmpty = false;
            bool OnlyKE = true;
            if (text.Length <= 0)
            {
                CanUseUsernameSetter(false);
                return;
            }

            isCanuse = DataManager.isCanUseUsername(text);

            if (text.Contains(" ")) ContainsEmpty = false;
            else ContainsEmpty = true;

            char[] arr = text.ToCharArray();

            foreach(char a in arr)
            {
                var category = char.GetUnicodeCategory(a);
                if (!(category == System.Globalization.UnicodeCategory.OtherLetter || category == System.Globalization.UnicodeCategory.LowercaseLetter || category == System.Globalization.UnicodeCategory.UppercaseLetter)) OnlyKE = false;
            }

            if (isCanuse && ContainsEmpty && OnlyKE && text.Length <= 20 && text.Length > 0)
            {
                CanUseUsernameSetter(true);
            }
            else
            {
                CanUseUsernameSetter(false);
            }
            return;
        }
        private void CanUseUsernameSetter(bool canuse)
        {
            if (canuse)
            {
                UserNameWarrings.Foreground = Brushes.Gray;
                CanUseUsername.Content = "이 사용자명은 사용할 수 있습니다.";
                CanUseUsername.Foreground = Brushes.Green;
                isCanUseUsername = true;
            }
            else
            {
                UserNameWarrings.Foreground = Brushes.Red;
                CanUseUsername.Content = "이 사용자명은 사용할 수 없습니다.";
                CanUseUsername.Foreground = Brushes.Red;
                isCanUseUsername = false;
            }
        }
        private void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            string text = EmailBox.Text;
            string subEmail = EmailTextBox.Text;
            string email = text + "@" + EmailTextBox.Text;
            new Task(() =>
            {
                DataManager.VerifyCode(email);
            }).Start();
            MessageBox.Show("이메일이 전송되었습니다.\n이메일 전송은 최대 30초까지 소요될 수 있습니다.");

            if (timer == null)
            {
                timer = new Timers();
                timer.timerevent += TImerEventSender;
                timer.Strat(301);
            }
            else
            {
                timer.SetTime(301);
            }
        
            EmailVerifyCanvas.Visibility = Visibility.Visible;
            Console.WriteLine("Email sccessfully sent");
        }
        private void TImerEventSender(int i)
        {
            this.Dispatcher.Invoke(() => { TimeCount.Content = "남은시간 : " + inttostr(i / 60) + ":" + inttostr(i % 60); TimeCount.Foreground = Brushes.Green; });
            if (i <= 0)
            {
                this.Dispatcher.Invoke(() => { TimeCount.Content = "시간이 만료되었습니다."; });
                this.Dispatcher.Invoke(() => { TimeCount.Foreground = Brushes.Red;});
                this.Dispatcher.Invoke(() =>
                {
                    string text = EmailBox.Text;
                    string subEmail = EmailTextBox.Text;
                    string email = text + "@" + EmailTextBox.Text;
                    DataManager.EndVerifyCode(email);
                });
            }
        }
        private string inttostr(int i)
        {
            if (i < 10) return "0" + i;
            else return i.ToString();
        }

        private void VerfiyButton_Click(object sender, RoutedEventArgs e)
        {
            string text = EmailBox.Text;
            string subEmail = EmailTextBox.Text;
            string email = text + "@" + EmailTextBox.Text;
            string code = VerifyBox.Text;

            bool check = DataManager.CheckVerifyCode(email, code);

            if (check)
            {
                Console.WriteLine(1);
                timer.Stop();

                Console.WriteLine(2);
                timer.timerevent -= TImerEventSender;

                Console.WriteLine(3);
                timer = null;
                
                Console.WriteLine(4);
                TimeCount.Content = "인증이 완료되었습니다.";
                TimeCount.Foreground = Brushes.Green;

                Console.WriteLine(5);
                isCanUseEmail = true;
                EmailButton.IsHitTestVisible = false;
                VerfiyButton.IsHitTestVisible = false;
                EmailBox.IsReadOnly = true;
                EmailTextBox.IsReadOnly = true;
                EmailCombobox.IsReadOnly = true;
                EmailCombobox.IsHitTestVisible = false;
                VerifyBox.IsReadOnly = true;

                Console.WriteLine(6);
            }
            else
            {
                MessageBox.Show("코드를 확인해주세요.");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isCanUseEmail) EmailCanUseSetter(false);
            if (!isCanUsePassword) PasswordCanUseSetter(false);
            if (!isCanUseUsername) CanUseUsernameSetter(false);
            if (PasswordBox.Password == "") PasswordWarring.Foreground = Brushes.Red;
            if (!isCanuseTeamName)
            {
                IsNewTeam.Foreground = Brushes.Red;
                IsNewTeam.Content = "이곳을 공백으로 남겨둘순 없습니다.";
            }
            if (isCanUseUsername && isCanUsePassword && isCanUseEmail && PasswordBox.Password != "" && isCanuseTeamName)
            {
                string text = TeamCodeBox.Text;
                TeamCodeVerify vir = DataManager.CheckStr(text);

                StringBuilder builder = new StringBuilder();
                switch (vir)
                {
                    case TeamCodeVerify.None:
                        builder.Append("새로운 팀 창설 | 팀명 : " + text);
                        break;
                    case TeamCodeVerify.TeamCodeContains:
                        builder.Append("기존의 팀 가입 | 팀명 : " + DataManager.GetTeamName(text));
                        break;
                }
                string results = builder.ToString();

                MessageBoxResult result = MessageBox.Show("다음 정보를 확인해주세요.\n사용자명 : " +
                    userNameBox.Text + "\n이메일 : " + EmailBox.Text + "@" + EmailTextBox.Text +"\n" +
                    results, "정보가 맞습니다" , MessageBoxButton.OKCancel);

                if(result == MessageBoxResult.OK)
                {
                    DataManager.RegisterUser(userNameBox.Text, EmailBox.Text + "@" + EmailTextBox.Text, PasswordBox.Password, text, DataManager.CheckStr(text));
                    this.Close();
                }
                else
                {
                    MessageBox.Show("알겠습니다. 다시한번 정보를 확인해주세요.");
                }
            }
        }
    }
    public class Timers
    {
        private int time;
        Task timer;
        public delegate void TimerDelegate(int time);
        public event TimerDelegate timerevent;
        public CancellationTokenSource cts = new CancellationTokenSource();
        public Timers()
        {
            
        }
        public void Strat(int time)
        {
            this.time = time;
            timer = new Task(() =>
            {
                while (time >= 0)
                {
                    if (timerevent == null) return;
                    time--;
                    timerevent(time);
                    Thread.Sleep(1000);
                }
            }, cts.Token);
            timer.Start();
        }
        public void SetTime(int time){

            if (timer.IsCompleted)
            {
                this.time = time;
                /*timer = new Task(() =>
                {
                    while (time >= 0)
                    {
                        if (timerevent == null) return;
                        time--;
                        timerevent(time);
                        Thread.Sleep(1000);
                    }
                }, cts.Token);
                timer.Start();*/
                return;
            }
            this.time = time;
        }
        public void Stop()
        {
            cts.Cancel();
        }
        public void Dispose()
        {
            timerevent = null;
            timer.Wait();
            timer.Dispose();
        }
    }
}
