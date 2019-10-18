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
    /// DownloadUI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DownloadUI : UserControl
    {
        string path;
        public void setFileName(string name)
        {
            FileFullName.Content = name;
        }
        public void setStatus(double value)
        {
            Arcs.EndAngle = 360 * (value / 100.0);
        }
        public void setFileSizeAndDate(string data) {
            FileSizeAndDate.Content = data;
        }
        public void setFilePath(string path)
        {
            this.path = path;
        }
        public void setIdentity(string identity)
        {
            if (DownloadData.getInstance().accessFiles.Contains(identity.Substring(1))) Images.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/" + identity.Substring(1) + ".png"));
            else Images.Source = new BitmapImage(new Uri("pack://application:,,,/Narusha_Protive;component/Resources/FileIcos/file.png"));
        }
        public DownloadUI()
        {
            InitializeComponent();
            OpenFolder.PreviewMouseDown += (sender, args) =>
            {
                System.Diagnostics.Process.Start(path);
            };
        }
    }
}
