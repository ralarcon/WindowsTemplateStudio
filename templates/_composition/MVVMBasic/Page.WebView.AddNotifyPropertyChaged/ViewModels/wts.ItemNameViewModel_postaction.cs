﻿        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            //{[{
            OnPropertyChanged(nameof(BrowserBackCommand));
            OnPropertyChanged(nameof(BrowserForwardCommand));
            //}]}
        }
