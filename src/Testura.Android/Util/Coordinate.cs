namespace Testura.Android.Util
{
    public class Coordinate
    {
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
