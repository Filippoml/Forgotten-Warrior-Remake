namespace GXPEngine
{
    using System;
    using GXPEngine.Core;

    /// <summary>
    /// The Sprite class holds 2D images that can be used as objects in your game.
    /// </summary>
    public class Sprite : GameObject
    {
        /// <summary>
        /// Defines the _texture
        /// </summary>
        protected Texture2D _texture;

        /// <summary>
        /// Defines the _bounds
        /// </summary>
        protected Rectangle _bounds;

        /// <summary>
        /// Defines the _uvs
        /// </summary>
        protected float[] _uvs;

        /// <summary>
        /// Defines the _color
        /// </summary>
        private uint _color = 0xFFFFFF;

        /// <summary>
        /// Defines the _alpha
        /// </summary>
        private float _alpha = 1.0f;

        /// <summary>
        /// Defines the _mirrorX
        /// </summary>
        protected bool _mirrorX = false;

        /// <summary>
        /// Defines the _mirrorY
        /// </summary>
        protected bool _mirrorY = false;

        /// <summary>
        /// Defines the blendMode
        /// </summary>
        public BlendMode blendMode = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap<see cref="System.Drawing.Bitmap"/></param>
        public Sprite(System.Drawing.Bitmap bitmap)
        {
            name = "BMP" + bitmap.Width + "x" + bitmap.Height;
            initializeFromTexture(new Texture2D(bitmap));
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														OnDestroy()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The OnDestroy
        /// </summary>
        protected override void OnDestroy()
        {
            if (_texture != null) _texture.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="keepInCache">The keepInCache<see cref="bool"/></param>
        public Sprite(string filename, bool keepInCache = false)
        {
            name = filename;
            initializeFromTexture(Texture2D.GetInstance(filename, keepInCache));
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														initializeFromTexture()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The initializeFromTexture
        /// </summary>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        protected void initializeFromTexture(Texture2D texture)
        {
            _texture = texture;
            _bounds = new Rectangle(0, 0, _texture.width, _texture.height);
            setUVs();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														setUVs
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The setUVs
        /// </summary>
        protected virtual void setUVs()
        {
            float left = _mirrorX ? 1.0f : 0.0f;
            float right = _mirrorX ? 0.0f : 1.0f;
            float top = _mirrorY ? 1.0f : 0.0f;
            float bottom = _mirrorY ? 0.0f : 1.0f;
            _uvs = new float[8] { left, top, right, top, right, bottom, left, bottom };
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														createCollider
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The createCollider
        /// </summary>
        /// <returns>The <see cref="Collider"/></returns>
        protected override Collider createCollider()
        {
            return new BoxCollider(this);
        }

        /// <summary>
        /// Gets the texture
        /// Returns the texture that is used to create this sprite.
		/// If no texture is used, null will be returned.
		/// Use this to retreive the original width/height or filename of the texture.
        /// </summary>
        public Texture2D texture
        {
            get { return _texture; }
        }

        /// <summary>
        /// Gets or sets the sprite's width in pixels.
        /// </summary>
        virtual public int width
        {
            get
            {
                if (_texture != null) return (int)Math.Abs(_texture.width * _scaleX);
                return 0;
            }
            set
            {
                if (_texture != null && _texture.width != 0) scaleX = value / ((float)_texture.width);
            }
        }

        /// <summary>
        /// Gets or sets the sprite's height in pixels.
        /// </summary>
        virtual public int height
        {
            get
            {
                if (_texture != null) return (int)Math.Abs(_texture.height * _scaleY);
                return 0;
            }
            set
            {
                if (_texture != null && _texture.height != 0) scaleY = value / ((float)_texture.height);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The RenderSelf
        /// </summary>
        /// <param name="glContext">The glContext<see cref="GLContext"/></param>
        override protected void RenderSelf(GLContext glContext)
        {
            if (game != null)
            {
                Vector2[] bounds = GetExtents();
                float maxX = float.MinValue;
                float maxY = float.MinValue;
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                for (int i = 0; i < 4; i++)
                {
                    if (bounds[i].x > maxX) maxX = bounds[i].x;
                    if (bounds[i].x < minX) minX = bounds[i].x;
                    if (bounds[i].y > maxY) maxY = bounds[i].y;
                    if (bounds[i].y < minY) minY = bounds[i].y;
                }
                bool test = (maxX < game.RenderRange.left) || (maxY < game.RenderRange.top) || (minX >= game.RenderRange.right) || (minY >= game.RenderRange.bottom);
                if (test == false)
                {
                    if (blendMode != null) blendMode.enable();
                    _texture.Bind();
                    glContext.SetColor((byte)((_color >> 16) & 0xFF),
                                       (byte)((_color >> 8) & 0xFF),
                                       (byte)(_color & 0xFF),
                                       (byte)(_alpha * 0xFF));
                    glContext.DrawQuad(GetArea(), _uvs);
                    glContext.SetColor(1, 1, 1, 1);
                    _texture.Unbind();
                    if (blendMode != null) BlendMode.NORMAL.enable();
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetArea()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetArea
        /// </summary>
        /// <returns>The <see cref="float[]"/></returns>
        internal float[] GetArea()
        {
            return new float[8] {
                _bounds.left, _bounds.top,
                _bounds.right, _bounds.top,
                _bounds.right, _bounds.bottom,
                _bounds.left, _bounds.bottom
            };
        }

        /// <summary>
        /// Gets the four corners of this object as a set of 4 Vector2s.
        /// </summary>
        /// <returns>The <see cref="Vector2[]"/></returns>
        public Vector2[] GetExtents()
        {
            Vector2[] ret = new Vector2[4];
            ret[0] = TransformPoint(_bounds.left, _bounds.top);
            ret[1] = TransformPoint(_bounds.right, _bounds.top);
            ret[2] = TransformPoint(_bounds.right, _bounds.bottom);
            ret[3] = TransformPoint(_bounds.left, _bounds.bottom);
            return ret;
        }

        /// <summary>
        /// Sets the origin, the pivot point of this Sprite in pixels.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        public void SetOrigin(float x, float y)
        {
            _bounds.x = -x;
            _bounds.y = -y;
        }

        /// <summary>
        /// This function can be used to enable mirroring for the sprite in either x or y direction.
        /// </summary>
        /// <param name="mirrorX">The mirrorX<see cref="bool"/></param>
        /// <param name="mirrorY">The mirrorY<see cref="bool"/></param>
        public void Mirror(bool mirrorX, bool mirrorY)
        {
            _mirrorX = mirrorX;
            _mirrorY = mirrorY;
            setUVs();
        }

        /// <summary>
        /// Gets or sets the color filter for this sprite.
		/// This can be any value between 0x000000 and 0xFFFFFF.
        /// </summary>
        public uint color
        {
            get { return _color; }
            set { _color = value & 0xFFFFFF; }
        }

        /// <summary>
        /// Sets the color of the sprite.
        /// </summary>
        /// <param name="r">The r<see cref="float"/></param>
        /// <param name="g">The g<see cref="float"/></param>
        /// <param name="b">The b<see cref="float"/></param>
        public void SetColor(float r, float g, float b)
        {
            r = Mathf.Clamp(r, 0, 1);
            g = Mathf.Clamp(g, 0, 1);
            b = Mathf.Clamp(b, 0, 1);
            byte rr = (byte)Math.Floor((r * 255));
            byte rg = (byte)Math.Floor((g * 255));
            byte rb = (byte)Math.Floor((b * 255));
            color = (uint)rb + (uint)(rg << 8) + (uint)(rr << 16);
        }

        /// <summary>
        /// Gets or sets the alpha value of the sprite. 
		/// Setting this value allows you to make the sprite (semi-)transparent.
		/// The alpha value should be in the range 0...1, where 0 is fully transparent and 1 is fully opaque.
        /// </summary>
        public float alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }
    }
}
