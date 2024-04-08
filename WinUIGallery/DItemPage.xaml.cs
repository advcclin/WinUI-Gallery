using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using WinUIGallery.Data;
using WinUIGallery.Helper;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using WinUIGallery.DesktopWap.Controls;
using System.Diagnostics;

namespace WinUIGallery
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class DItemPage : Page
    {

        public DashboardDataItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private DashboardDataItem _item;

        public DItemPage()
        {
            this.InitializeComponent();
            Loaded += (s, e) => SetInitialVisuals();
        }

        public void SetInitialVisuals()
        {
            var navigationRootPage = NavigationRootPage.GetForElement(this);
            if (navigationRootPage != null)
            {
                navigationRootPage.NavigationViewLoaded = OnNavigationViewLoaded;
                
                this.Focus(FocusState.Programmatic);
            }

            if (UIHelper.IsScreenshotMode)
            {
                var controlExamples = (this.contentFrame.Content as UIElement)?.GetDescendantsOfType<ControlExample>();

                if (controlExamples != null)
                {
                    foreach (var controlExample in controlExamples)
                    {
                        VisualStateManager.GoToState(controlExample, "ScreenshotMode", false);
                    }
                }
            }
        }
        private void OnNavigationViewLoaded()
        {
            NavigationRootPage.GetForElement(this).EnsureNavigationSelection(this.Item.UniqueId);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationRootPageArgs args = (NavigationRootPageArgs)e.Parameter;
            var uniqueId = (string)args.Parameter;
            var group = await DashboardDataSource.GetGroupFromItemAsync(uniqueId);
            var item = group?.Items.FirstOrDefault(x => x.UniqueId.Equals(uniqueId));

            if (item != null)
            {
                Item = item;

                // Load control page into frame.
                Type pageType = Type.GetType("WinUIGallery.ControlPages." + item.UniqueId + "Page");

                if (pageType != null)
                {
                    var pageName = string.IsNullOrEmpty(group.Folder) ? pageType.Name : $"{group.Folder}/{pageType.Name}";
                    System.Diagnostics.Debug.WriteLine(string.Format("[DItemPage] Navigate to {0}", pageType.ToString()));
                    this.contentFrame.Navigate(pageType);
                }
                args.NavigationRootPage.EnsureNavigationSelection(item?.UniqueId);
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var navigationRootPage = NavigationRootPage.GetForElement(this);
            if (navigationRootPage != null)
            {
                navigationRootPage.NavigationViewLoaded = null;
            }

            // We use reflection to call the OnNavigatedFrom function the user leaves this page
            // See this PR for more information: https://github.com/microsoft/WinUI-Gallery/pull/145
            Frame contentFrameAsFrame = contentFrame as Frame;
            Page innerPage = contentFrameAsFrame.Content as Page;
            if (innerPage != null)
            {
                MethodInfo dynMethod = innerPage.GetType().GetMethod("OnNavigatedFrom",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                dynMethod.Invoke(innerPage, new object[] { e });
            }

            base.OnNavigatedFrom(e);
        }

        public static DItemPage GetForElement(object obj)
        {
            UIElement element = (UIElement)obj;
            Window window = WindowHelper.GetWindowForElement(element);
            if (window != null)
            {
                return (DItemPage)window.Content;
            }
            return null;
        }

    }
}
