using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util;

namespace Testura.Android.Device.Services
{
    public interface IInteractionService
    {
        /// <summary>
        /// Performe a swipe motion on the screen.
        /// </summary>
        /// <param name="fromX">Start x position on screen</param>
        /// <param name="fromY">Start y position on screen</param>
        /// <param name="toX">Final x position on screen</param>
        /// <param name="toY">Final y position on screen</param>
        /// <param name="duration">Duration of the swipe in miliseconds</param>
        void Swipe(int fromX, int fromY, int toX, int toY, int duration);

        /// <summary>
        /// Swipe from one edge of the screen to another.
        /// </summary>
        /// <param name="swipeDirection">Direction to swipe</param>
        /// <param name="duration">Duration of the swipe in muliseconds</param>
        void Swipe(SwipeDirections swipeDirection, int duration);

        /// <summary>
        /// Click in the center of a node.
        /// </summary>
        /// <param name="node">Node to click on</param>
        void Click(Node node);

        /// <summary>
        /// Click on an x and y position of the screen.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        void Click(int x, int y);

        /// <summary>
        /// Send keys to a input (text) field.
        /// </summary>
        /// <param name="text">Input to field</param>
        void SendKeys(string text);
    }
}
