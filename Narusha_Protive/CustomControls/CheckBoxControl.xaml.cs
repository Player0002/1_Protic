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
    /// CheckBoxControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CheckBoxControl : UserControl
    {
        public bool IsChekced { get
            {
                return checkeds;
            }
        }

        private bool checkeds = false;

        public CheckBoxControl()
        {
            InitializeComponent();
            CheckedImg.Visibility = IsChekced ? Visibility.Visible : Visibility.Hidden;
            /*CheckBoxx.MouseDown += (sender, eventargs)=>{
                check();
            };
            CheckedImg.MouseDown += (sender, eventargs) =>
            {
                check();
            };
            TexttT.MouseDown += (sender, eventargs) =>
            {
                check();
            };*/
            GeneralGrid.MouseDown += (sender, eventargs) =>
            {
                check();
            };
        }
        private void check()
        {
            checkeds = !checkeds;
            CheckedImg.Visibility = IsChekced ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
