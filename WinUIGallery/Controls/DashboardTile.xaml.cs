using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace WinUIGallery.Controls
{
    public sealed partial class DashboardTile : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DashboardTile), new PropertyMetadata(null));

        public string Subtitle1
        {
            get { return (string)GetValue(Subtitle1Property); }
            set { SetValue(Subtitle1Property, value); }
        }

        public static readonly DependencyProperty Subtitle1Property =
            DependencyProperty.Register("Subtitle1", typeof(string), typeof(DashboardTile), new PropertyMetadata(null));

        public string Subtitle2
        {
            get { return (string)GetValue(Subtitle2Property); }
            set { SetValue(Subtitle2Property, value); }
        }

        public static readonly DependencyProperty Subtitle2Property =
            DependencyProperty.Register("Subtitle2", typeof(string), typeof(DashboardTile), new PropertyMetadata(null));

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(DashboardTile), new PropertyMetadata(null));

        public string  BadgeString
        {
            get { return (string)GetValue(BadgeProperty); }
            set { SetValue(BadgeProperty, value); }
        }

        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.Register("Badge", typeof(string), typeof(DashboardTile), new PropertyMetadata(null));

        public string FontIconGlyph
        {
            get { return (string)GetValue(FontIconGlyphProperty); }
            set { SetValue(FontIconGlyphProperty, value); }
        }

        public static readonly DependencyProperty FontIconGlyphProperty =
            DependencyProperty.Register("FontIconGlyph", typeof(string), typeof(DashboardTile), new PropertyMetadata(null));

        public DashboardTile()
        {
            this.InitializeComponent();
        }
    }
}
