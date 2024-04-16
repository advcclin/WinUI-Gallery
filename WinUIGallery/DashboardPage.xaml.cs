//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using WinUIGallery.Data;
using System.Linq;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace WinUIGallery
{
    public sealed partial class DashboardPage : DItemsPageBase
    {
        public DashboardPage()
        {
            this.InitializeComponent();
        }

        public string WinAppSdkDetails => App.WinAppSdkDetails;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationRootPageArgs args = (NavigationRootPageArgs)e.Parameter;
            //var menuItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.NavigationRootPage.NavigationView.MenuItems.ElementAt(3);
            var menuItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.NavigationRootPage.NavigationView.MenuItems.First();
            menuItem.IsSelected = true;

            Items = DashboardDataSource.Instance.Groups.Where(g => !g.IsSpecialSection).SelectMany(g => g.Items).OrderBy(i => i.Title).ToList();
            //Items = ControlInfoDataSource.Instance.Groups.SelectMany(g => g.Items.Where(i => i.BadgeString != null)).OrderBy(i => i.Title).ToList();
            itemsDashboardCVS.Source = FormatData();
        }

        private ObservableCollection<DGroupInfoList> FormatData()
        {
            var query = from item in this.Items
                        group item by item.BadgeString into g
                        orderby g.Key
                        select new DGroupInfoList(g) { Key = g.Key };

            ObservableCollection<DGroupInfoList> groupList = new ObservableCollection<DGroupInfoList>(query);

            foreach (var item in groupList)
            {
                switch (item.Key.ToString())
                {
                    case "New":
                        item.Title = "Recently added samples";
                        break;
                    case "Updated":
                        item.Title = "Recently updated samples";
                        break;
                    case "Preview":
                        item.Title = "Preview samples";
                        break;
                    default:
                        item.Title = "All Modules";
                        break;
                }
            }

            return groupList;
        }

        protected override bool GetIsNarrowLayoutState()
        {
            return LayoutVisualStates.CurrentState == NarrowLayout;
        }
    }

    public class DGroupInfoList : List<object>
    {
        public DGroupInfoList(IEnumerable<object> items) : base(items) { }

        public object Key { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
