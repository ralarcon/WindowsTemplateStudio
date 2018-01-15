﻿using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

using Param_ItemNamespace.Helpers;

namespace Param_ItemNamespace.ViewModels
{
    public class SharedDataWebLinkViewModel : SharedDataViewModelBase
    {
        private Uri _uri;

        public Uri Uri
        {
            get => _uri;
            set => Param_Setter(ref _uri, value);
        }

        public SharedDataWebLinkViewModel()
        {
        }

        public override async Task LoadDataAsync(ShareOperation shareOperation)
        {
            await base.LoadDataAsync(shareOperation);

            PageTitle = "ShareTargetFeature_WebLinkTitle".GetLocalized();
            DataFormat = StandardDataFormats.WebLink;
            Uri = await shareOperation.GetWebLinkAsync();
        }
    }
}
