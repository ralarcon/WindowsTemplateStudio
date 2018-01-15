﻿using System;
using System.Collections.ObjectModel;
using Windows.Media.Playback;
using Windows.Media.Core;

namespace Param_ItemNamespace.ViewModels
{
    public class MediaPlayerViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO WTS: Specify your video default and image here
        private const string DefaultSource = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_high.mp4";

        // The poster image is displayed until the video is started
        private const string DefaultPoster = "https://sec.ch9.ms/ch9/db15/43c9fbed-535e-4013-8a4a-a74cc00adb15/C9L12WinTemplateStudio_960.jpg";

        private IMediaPlaybackSource _source;

        public IMediaPlaybackSource Source
        {
            get { return _source; }
            set { Param_Setter(ref _source, value); }
        }

        private string _posterSource;

        public string PosterSource
        {
            get { return _posterSource; }
            set { Param_Setter(ref _posterSource, value); }
        }

        public MediaPlayerViewViewModel()
        {
            Source = MediaSource.CreateFromUri(new Uri(DefaultSource));
            PosterSource = DefaultPoster;
        }
    }
}
