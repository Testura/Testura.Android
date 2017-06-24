namespace Testura.Android.Util
{
    /// <summary>
    /// Represent a coordinate on the screen.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate"/> class.
        /// </summary>
        /// <param name="x">X part of a coordinate on the screen.</param>
        /// <param name="y">Y part of a coordinate on the screen.</param>
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the x part of the coordinate
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the y part of a coordinate
        /// </summary>
        public int Y { get; }
    }
}
