﻿using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Param_ItemNamespace.Helpers;

namespace Param_ItemNamespace.Services
{
    public class LocationService
    {
        private Geolocator geolocator;

        public event EventHandler<Geoposition> PositionChanged;

        public Geoposition CurrentPosition { get; private set; }

        public Task<bool> InitializeAsync()
        {
            return InitializeAsync(100);
        }

        public Task<bool> InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, (double)desiredAccuracyInMeters / 2);
        }

        public async Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            // to find out more about getting location, go to https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
            if (geolocator != null)
            {
                geolocator.PositionChanged -= Geolocator_PositionChanged;
                geolocator = null;
            }

            var access = await Geolocator.RequestAccessAsync();

            bool result;

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    geolocator = new Geolocator
                    {
                        DesiredAccuracyInMeters = desiredAccuracyInMeters,
                        MovementThreshold = movementThreshold
                    };
                    result = true;
                    break;
                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    result = false;
                    break;
            }

            return result;
        }

        public async Task StartListeningAsync()
        {
            if (geolocator == null)
            {
                throw new InvalidOperationException("ExceptionLocationServiceStartListeningCanNotBeCalled".GetLocalized());
            }

            geolocator.PositionChanged += Geolocator_PositionChanged;

            CurrentPosition = await geolocator.GetGeopositionAsync();
        }

        public void StopListening()
        {
            if (geolocator == null)
            {
                return;
            }

            geolocator.PositionChanged -= Geolocator_PositionChanged;
        }

        private async void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            CurrentPosition = args.Position;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PositionChanged?.Invoke(this, CurrentPosition);
            });
        }
    }
}
