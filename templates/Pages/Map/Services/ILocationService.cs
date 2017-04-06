using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ItemNamespace.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// Raised when the current position is updated.
        /// </summary>
        event EventHandler<Geoposition> PositionChanged;

        /// <summary>
        /// Gets the last known recorded position.
        /// </summary>
        Geoposition CurrentPosition { get; }

        /// <summary>
        /// Initializes the location service with a default accuracy (100 meters) and movement threshold.
        /// </summary>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// Initializes the location service with the specified accuracy and default movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters);

        /// <summary>
        /// Initializes the location service with the specified accuracy and movement threshold.
        /// </summary>
        /// <param name="desiredAccuracyInMeters">The desired accuracy at which the service provides location updates.</param>
        /// <param name="movementThreshold">The distance of movement, in meters, that is required for the service to raise the PositionChanged event.</param>
        /// <returns>True if the initialization was successful and the service can be used.</returns>
        Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold);

        /// <summary>
        /// Starts the service listening for location updates.
        /// Do not call until InitializeAsync has been called and returned true.
        /// </summary>
        /// <returns>An object that is used to manage the asynchronous operation.</returns>
        Task StartListeningAsync();

        /// <summary>
        /// Stops the service listening for location updates.
        /// </summary>
        void StopListening();
    }
}