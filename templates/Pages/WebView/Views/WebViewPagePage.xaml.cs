﻿using System;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    public sealed partial class WebViewPagePage : Page
    {
        public WebViewPagePage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }
    }
}
