using Testura.Android.Util;

namespace Testura.Android.Device.Ui.Server
{
    /// <summary>
    /// Defines methods to send interaction requests to the ui automator server.
    /// </summary>
    public interface IInteractionUiAutomatorServer
    {
        /// <summary>
        /// Send a tap request to the ui automator server on the android device.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>True if we successfully tapped, otherwise false.</returns>
        bool Tap(int x, int y);

        /// <summary>
        /// Send a swipe request to the ui automator server on the android device.
        /// </summary>
        /// <param name="fromX">Swipe from this x coordinate</param>
        /// <param name="fromY">Swipe from this y coordinate</param>
        /// <param name="toX">Swipe to this x coordinate</param>
        /// <param name="toY">Swipe to this y coordinate</param>
        /// <param name="duration">Swipe duration in miliseconds</param>
        /// <returns>True if we successfully swiped, otherwise false.</returns>
        bool Swipe(int fromX, int fromY, int toX, int toY, int duration);

        /// <summary>
        /// Send a key event request to the ui automator server on the android device.
        /// </summary>
        /// <param name="keyEvent">Key event to send to the device</param>
        /// <returns>True if we successfully input key event, otherwise false.</returns>
        bool InputKeyEvent(KeyEvents keyEvent);

        /// <summary>
        /// Send a input text request to the ui automator server on the android device.
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <returns>True if we successfully input text, otherwise false.</returns>
        bool InputText(string text);
    }
}
