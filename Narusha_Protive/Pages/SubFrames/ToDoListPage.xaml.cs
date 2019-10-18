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
    /// ToDoListPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ToDoListPage : Page
    {
        public ToDoListPage()
        {
            InitializeComponent();
            this.SizeChanged += (sender, events) =>
            {
                update(true);
            };
        }
        private void update(bool isReset)
        {
            Load.Visibility = Visibility.Visible;

            if (isReset) Lists.Children.Clear();
            if (TeamData.toDoList != null)
            {
                foreach (ToDoList list in TeamData.toDoList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        
                        ToDoUI ui = new ToDoUI();
                        ui.Height = MainGrid.ActualHeight / 6;
                        ui.Message.Content = list.message;
                        ui.Date.Content = DataFormatter.toDate(list.when);
                        ui.IsCompleted.Visibility = list.completed ? Visibility.Visible : Visibility.Hidden;
                        Lists.Children.Add(ui);
                        
                    });
                }
            }
            Load.Visibility = Visibility.Hidden;

        }
    }
}
