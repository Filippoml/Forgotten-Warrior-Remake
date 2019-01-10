namespace GXPEngine.Core
{
    /// <summary>
    /// Defines the <see cref="Collider" />
    /// </summary>
    public class Collider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Collider"/> class.
        /// </summary>
        public Collider()
        {
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The HitTest
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public virtual bool HitTest(Collider other)
        {
            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The HitTestPoint
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public virtual bool HitTestPoint(float x, float y)
        {
            return false;
        }
    }
}
