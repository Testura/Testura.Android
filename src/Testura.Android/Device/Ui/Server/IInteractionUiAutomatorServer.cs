using Testura.Android.Util;

namespace Testura.Android.Device.Ui.Server
{
    public interface IInteractionUiAutomatorServer
    {
        /// <summary>
        /// Send a tap request to the ui automator server on the android device.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        void Tap(int x, int y);

        /// <summary>
        /// Send a swipe request to the ui automator server on the android device.
        /// </summary>
        /// <param name="fromX">Swipe from this x coordinate</param>
        /// <param name="fromY">Swipe from this y coordinate</param>
        /// <param name="toX">Swipe to this x coordinate</param>
        /// <param name="toY">Swipe to this y coordinate</param>
        /// <param name="duration">Swipe duration in miliseconds</param>
        void Swipe(int fromX, int fromY, int toX, int toY, int duration);

        /// <summary>
        /// Send a key event request to the ui automator server on the android device.
        /// </summary>
        /// <param name="keyEvent">Key event to send to the device</param>
        void InputKeyEvent(KeyEvents keyEvent);

        /// <summary>
        /// Send a input text request to the ui automator server on the android device.
        /// </summary>
        /// <param name="text">Text to send</param>
        void InputText(string text);
    }
}
