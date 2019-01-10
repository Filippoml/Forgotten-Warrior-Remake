namespace GXPEngine.Core
{
    using System;

    /// <summary>
    /// Defines the <see cref="BoxCollider" />
    /// </summary>
    public class BoxCollider : Collider
    {
        /// <summary>
        /// Defines the _owner
        /// </summary>
        private Sprite _owner;

        //------------------------------------------------------------------------------------------------------------------------
        //														BoxCollider()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxCollider"/> class.
        /// </summary>
        /// <param name="owner">The owner<see cref="Sprite"/></param>
        public BoxCollider(Sprite owner)
        {
            _owner = owner;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The HitTest
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool HitTest(Collider other)
        {
            if (other is BoxCollider)
            {
                Vector2[] c = _owner.GetExtents();
                if (c == null) return false;
                Vector2[] d = ((BoxCollider)other)._owner.GetExtents();
                if (d == null) return false;
                if (!areaOverlap(c, d)) return false;
                return areaOverlap(d, c);
            }
            else
            {
                return false;
            }
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
        public override bool HitTestPoint(float x, float y)
        {
            Vector2[] c = _owner.GetExtents();
            if (c == null) return false;
            Vector2 p = new Vector2(x, y);
            return pointOverlapsArea(p, c);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														areaOverlap()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The areaOverlap
        /// </summary>
        /// <param name="c">The c<see cref="Vector2[]"/></param>
        /// <param name="d">The d<see cref="Vector2[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool areaOverlap(Vector2[] c, Vector2[] d)
        {

            float dx = c[1].x - c[0].x;
            float dy = c[1].y - c[0].y;
            float lengthSQ = (dy * dy + dx * dx);

            if (lengthSQ == 0.0f) lengthSQ = 1.0f;

            float t, minT, maxT;

            t = ((d[0].x - c[0].x) * dx + (d[0].y - c[0].y) * dy) / lengthSQ;
            maxT = t; minT = t;

            t = ((d[1].x - c[0].x) * dx + (d[1].y - c[0].y) * dy) / lengthSQ;
            minT = Math.Min(minT, t); maxT = Math.Max(maxT, t);

            t = ((d[2].x - c[0].x) * dx + (d[2].y - c[0].y) * dy) / lengthSQ;
            minT = Math.Min(minT, t); maxT = Math.Max(maxT, t);

            t = ((d[3].x - c[0].x) * dx + (d[3].y - c[0].y) * dy) / lengthSQ;
            minT = Math.Min(minT, t); maxT = Math.Max(maxT, t);

            if ((minT >= 1) || (maxT < 0)) return false;

            dx = c[3].x - c[0].x;
            dy = c[3].y - c[0].y;
            lengthSQ = (dy * dy + dx * dx);

            if (lengthSQ == 0.0f) lengthSQ = 1.0f;

            t = ((d[0].x - c[0].x) * dx + (d[0].y - c[0].y) * dy) / lengthSQ;
            maxT = t; minT = t;

            t = ((d[1].x - c[0].x) * dx + (d[1].y - c[0].y) * dy) / lengthSQ;
            minT = Math.Min(minT, t); maxT = Math.Max(maxT, t);

            t = ((d[2].x - c[0].x) * dx + (d[2].y - c[0].y) * dy) / lengthSQ;
            minT = Math.Min(minT, t); maxT = Math.Max(maxT, t);

            t = ((d[3].x - c[0].x) * dx + (d[3].y - c[0].y) * dy) / lengthSQ;
            minT = Math.Min(minT, t); maxT = Math.Max(maxT, t);

            if ((minT >= 1) || (maxT < 0)) return false;

            return true;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														pointOverlapsArea()
        //------------------------------------------------------------------------------------------------------------------------
        //ie. for hittestpoint and mousedown/up/out/over
        /// <summary>
        /// The pointOverlapsArea
        /// </summary>
        /// <param name="p">The p<see cref="Vector2"/></param>
        /// <param name="c">The c<see cref="Vector2[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private bool pointOverlapsArea(Vector2 p, Vector2[] c)
        {

            float dx = c[1].x - c[0].x;
            float dy = c[1].y - c[0].y;
            float lengthSQ = (dy * dy + dx * dx);

            float t;

            t = ((p.x - c[0].x) * dx + (p.y - c[0].y) * dy) / lengthSQ;

            if ((t > 1) || (t < 0)) return false;

            dx = c[3].x - c[0].x;
            dy = c[3].y - c[0].y;
            lengthSQ = (dy * dy + dx * dx);

            t = ((p.x - c[0].x) * dx + (p.y - c[0].y) * dy) / lengthSQ;

            if ((t > 1) || (t < 0)) return false;

            return true;
        }
    }
}
