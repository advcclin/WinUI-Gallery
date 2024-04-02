//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WinUIGallery.Common;
using WinUIGallery.DesktopWap.DataModel;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app
// is first launched.

namespace WinUIGallery.Data
{
    public class DRoot
    {
        public ObservableCollection<DashboardDataGroup> Groups { get; set; }
    }
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(DRoot))]
    internal partial class DRootContext : JsonSerializerContext
    {
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class DashboardDataItem
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string ApiNamespace { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string IconGlyph { get; set; }
        public string BadgeString { get; set; }
        public string Content { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsPreview { get; set; }
        public ObservableCollection<DashboardDocLink> Docs { get; set; }
        public ObservableCollection<string> RelatedControls { get; set; }

        public bool IncludedInBuild { get; set; }

        public string SourcePath { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class DashboardDocLink
    {
        public DashboardDocLink(string title, string uri)
        {
            this.Title = title;
            this.Uri = uri;
        }
        public string Title { get; set; }
        public string Uri { get; set; }
    }


    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class DashboardDataGroup
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string IconGlyph { get; set; }
        public string ApiNamespace { get; set; }
        public bool IsSpecialSection { get; set; }
        public string Folder { get; set; }
        public ObservableCollection<DashboardDataItem> Items { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    ///
    /// DashboardSource initializes with data read from a static json file included in the
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class DashboardDataSource
    {
        private static readonly object _lock = new();

        #region Singleton

        private static readonly DashboardDataSource _instance;

        public static DashboardDataSource Instance
        {
            get
            {
                return _instance;
            }
        }

        static DashboardDataSource()
        {
            _instance = new DashboardDataSource();
        }

        private DashboardDataSource() { }

        #endregion

        private readonly IList<DashboardDataGroup> _groups = new List<DashboardDataGroup>();
        public IList<DashboardDataGroup> Groups
        {
            get { return this._groups; }
        }

        public async Task<IEnumerable<DashboardDataGroup>> GetGroupsAsync()
        {
            await _instance.GetDashboardDataAsync();

            return _instance.Groups;
        }

        public static async Task<DashboardDataGroup> GetGroupAsync(string uniqueId)
        {
            await _instance.GetDashboardDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _instance.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<DashboardDataItem> GetItemAsync(string uniqueId)
        {
            await _instance.GetDashboardDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _instance.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() > 0) return matches.First();
            return null;
        }

        public static async Task<DashboardDataGroup> GetGroupFromItemAsync(string uniqueId)
        {
            await _instance.GetDashboardDataAsync();
            var matches = _instance.Groups.Where((group) => group.Items.FirstOrDefault(item => item.UniqueId.Equals(uniqueId)) != null);
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetDashboardDataAsync()
        {
            lock (_lock)
            {
                if (this.Groups.Count() != 0)
                {
                    return;
                }
            }
            Debug.Print("DashboardDataGroup - Get data async");

            var jsonText = await FileLoader.LoadText("DataModel/DashboardData.json");
            var dashboardDataGroup = JsonSerializer.Deserialize(jsonText, typeof(DRoot), DRootContext.Default) as DRoot;

            lock (_lock)
            {
                string pageRoot = "WinUIGallery.ControlPages.";

                dashboardDataGroup.Groups.SelectMany(g => g.Items).ToList().ForEach(item =>
                {
#nullable enable
                    string? badgeString = item switch
                    {
                        { IsNew: true } => "New",
                        { IsUpdated: true } => "Updated",
                        { IsPreview: true } => "Preview",
                        _ => ""

                    };
                    string pageString = $"{pageRoot}{item.UniqueId}Page";
                    Type? pageType = Type.GetType(pageString);

                    item.BadgeString = badgeString;
                    item.IncludedInBuild = pageType is not null;
                    item.ImagePath ??= "ms-appx:///Assets/ControlImages/Placeholder.png";
#nullable disable
                });

                foreach (var group in dashboardDataGroup.Groups)
                {
                    if (!Groups.Any(g => g.Title == group.Title))
                    {
                        Debug.Print("DashboardDataGroup - Add Group: " + group.Title);
                        Groups.Add(group);
                    }
                }
            }
        }
    }
}
