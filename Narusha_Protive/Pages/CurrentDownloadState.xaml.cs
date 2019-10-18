using Narusha_Protive.CustomControls;
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
    /// CurrentDownloadState.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CurrentDownloadState : Page
    {
        public CurrentDownloadState()
        {
            InitializeComponent();
            var data = DownloadData.getInstance();
            Load.Visibility = Visibility.Hidden;
            data.update += () =>
            {
                updates();
            };
            this.SizeChanged += (sender, args) =>
            {
                updates();
            };
        }
        private void updates()
        {
            Load.Visibility = Visibility.Visible;
            LoadingT.Content = "Loading..";
            Lists.Children.Clear();
            if(DownloadData.getInstance().fileStatus != null )
                foreach (var d in DownloadData.getInstance().fileStatus)
                {
                    var ui = new DownloadUI();
                    ui.setFileName(d.Key);
                    ui.setFilePath(d.Value.fileLocation);
                    ui.setStatus(d.Value.percentage);
                    ui.setFileSizeAndDate(d.Value.maxFileSize + "MB | " + d.Value.Date);
                    ui.setIdentity(d.Key.Substring(d.Key.LastIndexOf('.')));
                    ui.Loaded += (c, s) =>
                    {
                        ui.Height = 74;
                        ui.Width = 340;
                    };
                    Lists.Children.Add(ui);
                }
            if(Lists.Children.Count < 1)
            {
                LoadingT.Content = "Can't find any files";
                return;
            }
            Load.Visibility = Visibility.Hidden;
        }
    }
}
