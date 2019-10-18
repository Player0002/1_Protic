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

namespace Narusha_Protive.Pages.Popup
{
    /// <summary>
    /// download_info.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class download_info : Page
    {
        public download_info()
        {
            InitializeComponent();
        }
        public void setCount(int i)
        {
            Message.Content = "다운로드 중인 파일이 " + i + "개가 있습니다.";
        }
    }
}
