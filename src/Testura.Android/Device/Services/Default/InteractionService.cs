using System;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Services.Default
{
    /// <summary>
    /// Provides functionality to interact with the screen with an android device.
    /// </summary>
    public class InteractionService : Service, IInteractionService
    {
        private readonly IInteractionUiAutomatorServer _interactionServer;
        private NodeBounds _screenBounds;

        public InteractionService(IInteractionUiAutomatorServer interactionServer)
        {
            if (interactionServer == null)
            {
                throw new ArgumentNullException(nameof(interactionServer));
            }

            _interactionServer = interactionServer;
        }

        /// <summary>
        /// Perform a swipe motion on the screen.
        /// </summary>
        /// <param name="fromX">Start x position on screen</param>
        /// <param name="fromY">Start y position on screen</param>
        /// <param name="toX">Final x position on screen</param>
        /// <param name="toY">Final y position on screen</param>
        /// <param name="duration">Duration of the swipe in milliseconds</param>
        public void Swipe(int fromX, int fromY, int toX, int toY, int duration)
        {
            _interactionServer.Swipe(fromX, fromY, toX, toY, duration);
            Device.Ui.ClearCache();
        }

        /// <summary>
        /// Swipe from one edge of the screen to another.
        /// </summary>
        /// <param name="swipeDirection">Direction to swipe</param>
        /// <param name="duration">Duration of the swipe in milliseconds</param>
        public void Swipe(SwipeDirections swipeDirection, int duration)
        {
            if (_screenBounds == null)
            {
                SetScreenHeightAndWidth();
            }

            var middleX = _screenBounds.Width / 2;
            var middleY = _screenBounds.Height / 2;

            switch (swipeDirection)
            {
                case SwipeDirections.Left:
                    Swipe(middleX, middleY, 0, middleY, duration);
                    break;
                case SwipeDirections.Up:
                    Swipe(middleX, middleY, middleX, 0, duration);
                    break;
                case SwipeDirections.Right:
                    Swipe(middleX, middleY, _screenBounds.Width, middleY, duration);
                    break;
                case SwipeDirections.Down:
                    Swipe(middleX, middleY, middleX, _screenBounds.Height, duration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
        }

        /// <summary>
        /// Tap in the center of a node.
        /// </summary>
        /// <param name="node">Node to click on</param>
        public void Tap(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var center = node.GetNodeCenter();
            Tap(center.X, center.Y);
        }

        /// <summary>
        /// Tap on an x and y position of the screen.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        public void Tap(int x, int y)
        {
            _interactionServer.Tap(x, y);
            Device.Ui.ClearCache();
        }

        /// <summary>
        /// Input text into the node
        /// </summary>
        /// <param name="text">The text to input into the node</param>
        /// <param name="useSlowInput">If true we use adb input, otherwise set text through server.</param>
        public void InputText(string text, bool useSlowInput = false)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (useSlowInput)
            {
                Device.Adb.Shell($"input text {text.Replace(" ", "%s")}");
            }
            else
            {
                _interactionServer.InputText(text);
            }

            Device.Ui.ClearCache();
        }

        /// <summary>
        /// Send a key event to the device.
        /// </summary>
        /// <param name="keyEvent">Key event to send to the device</param>
        public void InputKeyEvent(KeyEvents keyEvent)
        {
            Device.Adb.Shell($"input keyevent {(int)keyEvent}");
            Device.Ui.ClearCache();
        }

        private void SetScreenHeightAndWidth()
        {
            DeviceLogger.Log("Getting width and height");
            var widthAndHeight = Device.Adb.Shell("wm size");
            if (string.IsNullOrEmpty(widthAndHeight))
            {
                throw new AdbException("Could not get screen width and height");
            }

            var split = widthAndHeight.Replace(" ", string.Empty).Split(':', 'x');
            DeviceLogger.Log($"Width: {split[split.Length - 2]}, Height: {split[split.Length - 1]}");
            _screenBounds = new NodeBounds(int.Parse(split[split.Length - 2]), int.Parse(split[split.Length - 1]));
        }
    }
}
