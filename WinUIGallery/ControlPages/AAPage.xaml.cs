using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace WinUIGallery.ControlPages
{
    public sealed partial class AAPage : Page
    {
        public AAPage()
        {
            this.InitializeComponent();
            Loaded += AAPage_Loaded;
        }

        private void AAPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
