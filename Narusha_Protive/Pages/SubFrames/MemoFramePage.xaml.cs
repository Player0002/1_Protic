using Narusha_Protive.CustomControls;
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

namespace Narusha_Protive.Pages.SubFrames
{
    /// <summary>
    /// MemoFramePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MemoFramePage : Page
    {
        Http_Getcs httpget = new Http_Getcs();

        Random rnd = new Random();
        public MemoFramePage()
        {
            InitializeComponent();
            SCROLL.PreviewMouseWheel += (sender, objs) =>
            {
                if (UserData.last != null) objs.Handled = true;
                else objs.Handled = false;
            };
            this.SizeChanged += (sender, objs) =>
            {
                Memous.Children.Clear();
                Load.Visibility = Visibility.Visible;
                new Task(() =>
                {
                    if(UserData.memos != null)
                        for (int i = 0; i < UserData.memos.Length; i++)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                MemoUI ui = new MemoUI();
                                ui.Height = MainGrid.ActualHeight / 8;
                                ui.MemoLabel.Content = httpget.GetMemoText(UserData.memos[i]);
                                Memous.Children.Add(ui);
                            });
                        }
                    Dispatcher.Invoke(() =>
                    {
                        Load.Visibility = Visibility.Hidden;
                    });
                }).Start();
            };
        }
        public void AddMemos()
        {
            MemoUI ui = new MemoUI();
            ui.Height = MainGrid.ActualHeight / 8;
            ui.MemoLabel.Content = httpget.GetMemoText(UserData.updateMemos);
            Memous.Children.Add(ui);
            UserData.updateMemos = -1;
        }

    }
}
