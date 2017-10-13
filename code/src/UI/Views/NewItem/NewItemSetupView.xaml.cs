﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class NewItemSetupView : Page
    {
        public MainViewModel ViewModel { get; }

        public NewItemSetupView()
        {
            ViewModel = MainViewModel.Current;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
