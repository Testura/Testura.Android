namespace Testura.Android.Device.Ui.Nodes.Data
{
    public class NodeBounds
    {
        public NodeBounds(int width, int height)
        {
            Height = height;
            Width = width;
        }

        /// <summary>
        /// Gets the height bound
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width bound
        /// </summary>
        public int Width { get; }
    }
}
