﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Windows.Input;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class SummaryLicenseViewModel : Observable
    {
        public SummaryLicenseViewModel()
        {
        }

        public SummaryLicenseViewModel(TemplateLicense license)
        {
            if (license == null)
            {
                return;
            }

            Text = license.Text;
            Url = license.Url;
        }

        private ICommand _navigateCommand;

        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand(Navigate));

        private void Navigate()
        {
            if (!string.IsNullOrWhiteSpace(Url) && Uri.IsWellFormedUriString(Url, UriKind.Absolute))
            {
                Process.Start(Url);
            }
        }

        private string _text;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _url;

        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }
    }
}
