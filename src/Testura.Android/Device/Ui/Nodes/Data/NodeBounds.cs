namespace Testura.Android.Device.Ui.Nodes.Data
{
    /// <summary>
    /// Represents the node bounds (width and height) of a node.
    /// </summary>
    public class NodeBounds
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeBounds"/> class.
        /// 
        /// </summary>
        /// <param name="width">Width of the node.</param>
        /// <param name="height">Height of the node.</param>
        public NodeBounds(int width, int height)
        {
            Height = height;
            Width = width;
        }

        /// <summary>
        /// Gets the height bound.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width bound.
        /// </summary>
        public int Width { get; }
    }
}
