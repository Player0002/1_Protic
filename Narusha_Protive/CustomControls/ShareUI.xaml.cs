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
    /// ShareUI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ShareUI : UserControl
    {
        public ShareUI()
        {
            InitializeComponent();
        }
        public void setName(string FIleFullName)
        {
            FileName.Content = FIleFullName;
            imageUpdate();
        }
        private void imageUpdate()
        {
            var data = DownloadData.getInstance();
            var identity = FileName.Content.ToString().Substring(FileName.Content.ToString().LastIndexOf('.'));
            if (DownloadData.getInstance().accessFiles.Contains(identity.Substring(1))) FileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/" + identity.Substring(1) + ".png"));
            else FileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/file.png"));
        }
    }
}
