namespace GXPEngine.Core
{
    /// <summary>
    /// Defines the <see cref="Vector2" />
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Defines the x
        /// </summary>
        public float x;

        /// <summary>
        /// Defines the y
        /// </summary>
        public float y;

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return "[Vector2 " + x + ", " + y + "]";
        }
    }
}
