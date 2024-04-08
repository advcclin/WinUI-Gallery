using System;
using WinUIGallery.Data;
using WinUIGallery.Helper;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uri = System.Uri;

namespace WinUIGallery.DesktopWap.Controls
{
    public sealed partial class DPageHeader : UserControl
    {
       
        public string PageName { get; set; }

        public DashboardDataItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private DashboardDataItem _item;

        public DPageHeader()
        {
            this.InitializeComponent();
        }

    }
}
