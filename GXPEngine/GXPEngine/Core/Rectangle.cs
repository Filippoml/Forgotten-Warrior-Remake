namespace GXPEngine.Core
{
    /// <summary>
    /// Defines the <see cref="Rectangle" />
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Defines the x, y, width, height
        /// </summary>
        public float x, y, width, height;

        //------------------------------------------------------------------------------------------------------------------------
        //														Rectangle()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <param name="width">The width<see cref="float"/></param>
        /// <param name="height">The height<see cref="float"/></param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Properties()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the left
        /// </summary>
        public float left
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the right
        /// </summary>
        public float right
        {
            get { return x + width; }
        }

        /// <summary>
        /// Gets the top
        /// </summary>
        public float top
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the bottom
        /// </summary>
        public float bottom
        {
            get { return y + height; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														ToString()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return (x + "," + y + "," + width + "," + height);
        }
    }
}
