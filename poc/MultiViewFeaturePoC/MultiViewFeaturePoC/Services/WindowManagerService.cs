﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Services
{
    public delegate void ViewClosedHandler(ViewLifetimeControl viewControl, EventArgs e);

    public class WindowManagerService
    {
        // TODO WTS: See more details about showing multiple views for an app
        // https://docs.microsoft.com/windows/uwp/design/layout/show-multiple-views
        private static WindowManagerService _current;
        public static WindowManagerService Current => _current ?? (_current = new WindowManagerService());

        // Contains all the opened secondary views.
        public readonly ObservableCollection<ViewLifetimeControl> SecondaryViews = new ObservableCollection<ViewLifetimeControl>();

        public int MainViewId { get; private set; }

        public CoreDispatcher MainDispatcher { get; private set; }

        public void Initialize()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
            MainDispatcher = Window.Current.Dispatcher;
        }

        // TODO WTS: Displays a view as a standalone
        // You can use the resulting ViewLifeTileControl to interact with the new window.
        public async Task<ViewLifetimeControl> TryShowAsStandaloneAsync(string windowTitle, Type pageType)
        {
            ViewLifetimeControl viewControl = await CreateViewLifetimeControlAsync(windowTitle, pageType);
            SecondaryViews.Add(viewControl);
            viewControl.StartViewInUse();
            var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewControl.Id, ViewSizePreference.Default, ApplicationView.GetForCurrentView().Id, ViewSizePreference.Default);
            viewControl.StopViewInUse();
            return viewControl;
        }


        // Displays a view as a standalone view in the desired view mode
        public async Task<ViewLifetimeControl> TryShowAsViewModeAsync(string windowTitle, Type pageType, ApplicationViewMode viewMode = ApplicationViewMode.Default)
        {
            ViewLifetimeControl viewControl = await CreateViewLifetimeControlAsync(windowTitle, pageType);
            SecondaryViews.Add(viewControl);
            viewControl.StartViewInUse();
            var viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(viewControl.Id, viewMode);
            viewControl.StopViewInUse();
            return viewControl;
        }


        private async Task<ViewLifetimeControl> CreateViewLifetimeControlAsync(string windowTitle, Type pageType)
        {
            ViewLifetimeControl viewControl = null;

            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewControl = ViewLifetimeControl.CreateForCurrentView();
                viewControl.Title = windowTitle;
                viewControl.StartViewInUse();
                var frame = new Frame();
                frame.Navigate(pageType, viewControl);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = viewControl.Title;
            });

            return viewControl;
        }

        public bool IsWindowOpen(string windowTitle) => SecondaryViews.Any(v => v.Title == windowTitle);
    }
}
