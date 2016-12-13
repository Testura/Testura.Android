using System;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;

namespace Testura.Android.Device.Services.Default
{
    public class InteractionService : Service, IInteractionService
    {
        private NodeBounds _screenBounds;

        /// <summary>
        /// Performe a swipe motion on the screen.
        /// </summary>
        /// <param name="fromX">Start x position on screen</param>
        /// <param name="fromY">Start y position on screen</param>
        /// <param name="toX">Final x position on screen</param>
        /// <param name="toY">Final y position on screen</param>
        /// <param name="duration">Duration of the swipe in miliseconds</param>
        public void Swipe(int fromX, int fromY, int toX, int toY, int duration)
        {
            Device.Adb.Shell($"input swipe {fromX} {fromY} {toX} {toY} {duration}");
        }

        /// <summary>
        /// Swipe from one edge of the screen to another.
        /// </summary>
        /// <param name="swipeDirection">Direction to swipe</param>
        /// <param name="duration">Duration of the swipe in muliseconds</param>
        public void Swipe(SwipeDirections swipeDirection, int duration)
        {
            if (_screenBounds == null)
            {
                SetScreenHeightAndWidth();
            }

            switch (swipeDirection)
            {
                case SwipeDirections.Left:
                    Swipe(_screenBounds.Width, _screenBounds.Height / 2, 0, _screenBounds.Height / 2, duration);
                    break;
                case SwipeDirections.Up:
                    Swipe(_screenBounds.Width / 2, _screenBounds.Height, _screenBounds.Width / 2, 0, duration);
                    break;
                case SwipeDirections.Right:
                    Swipe(0, _screenBounds.Height / 2, _screenBounds.Width, _screenBounds.Height / 2, duration);
                    break;
                case SwipeDirections.Down:
                    Swipe(_screenBounds.Width / 2, 0, _screenBounds.Width / 2, _screenBounds.Height, duration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
        }

        /// <summary>
        /// Click in the center of a node.
        /// </summary>
        /// <param name="node">Node to click on</param>
        public void Click(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var center = node.GetNodeCenter();
            Click(center.X, center.Y);
        }

        /// <summary>
        /// Click on an x and y position of the screen.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        public void Click(int x, int y)
        {
            Device.Adb.Shell($"input tap {x} {y}");
        }

        /// <summary>
        /// Send keys to a input (text) field.
        /// </summary>
        /// <param name="text">Input to field</param>
        public void SendKeys(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Device.Adb.Shell($"input text {text.Replace(" ", "%s")}");
        }

        private void SetScreenHeightAndWidth()
        {
            var node = Device.Ui.FindNode(5, With.Index(0));
            var bounds = node.GetNodeBounds();
            _screenBounds = new NodeBounds(bounds[1].X - bounds[0].X, bounds[1].Y - bounds[0].Y);
        }
    }
}
