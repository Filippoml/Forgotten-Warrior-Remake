namespace GXPEngine
{
    using System;
    using GXPEngine.Core;

    /// <summary>
    /// The Transformable class contains all positional data of GameObjects.
    /// </summary>
    public class Transformable
    {
        /// <summary>
        /// Defines the _matrix
        /// </summary>
        protected float[] _matrix = new float[16] {
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f };

        /// <summary>
        /// Defines the _rotation
        /// </summary>
        protected float _rotation = 0.0f;

        /// <summary>
        /// Defines the _scaleX
        /// </summary>
        protected float _scaleX = 1.0f;

        /// <summary>
        /// Defines the _scaleY
        /// </summary>
        protected float _scaleY = 1.0f;

        //------------------------------------------------------------------------------------------------------------------------
        //														Transform()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="Transformable"/> class.
        /// </summary>
        public Transformable()
        {
        }

        /// <summary>
        /// Gets the matrix
        /// Returns the gameobject's 4x4 matrix.
        /// </summary>
        public float[] matrix
        {
            get
            {
                float[] matrix = (float[])_matrix.Clone();
                matrix[0] *= _scaleX;
                matrix[1] *= _scaleX;
                matrix[4] *= _scaleY;
                matrix[5] *= _scaleY;
                return matrix;
            }
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        public float x
        {
            get { return _matrix[12]; }
            set { _matrix[12] = value; }
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        public float y
        {
            get { return _matrix[13]; }
            set { _matrix[13] = value; }
        }

        /// <summary>
        /// Sets the X and Y position.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        public void SetXY(float x, float y)
        {
            _matrix[12] = x;
            _matrix[13] = y;
        }

        /// <summary>
        /// Transforms the point from the game's global space to this object's local space.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public virtual Vector2 InverseTransformPoint(float x, float y)
        {
            Vector2 ret = new Vector2();
            x -= _matrix[12];
            y -= _matrix[13];
            if (_scaleX != 0) ret.x = ((x * _matrix[0] + y * _matrix[1]) / _scaleX); else ret.x = 0;
            if (_scaleY != 0) ret.y = ((x * _matrix[4] + y * _matrix[5]) / _scaleY); else ret.y = 0;
            return ret;
        }

        /// <summary>
        /// Returns the distance to another Transformable
        /// </summary>
        /// <param name="other">The other<see cref="Transformable"/></param>
        /// <returns>The <see cref="float"/></returns>
        public float DistanceTo(Transformable other)
        {
            float dx = other.x - x;
            float dy = other.y - y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Transforms the point from this object's local space to the game's global space.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public virtual Vector2 TransformPoint(float x, float y)
        {
            Vector2 ret = new Vector2();
            ret.x = (_matrix[0] * x * _scaleX + _matrix[4] * y * _scaleY + _matrix[12]);
            ret.y = (_matrix[1] * x * _scaleX + _matrix[5] * y * _scaleY + _matrix[13]);
            return ret;
        }

        /// <summary>
        /// Gets or sets the object's rotation in degrees.
        /// </summary>
        public float rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                float r = _rotation * Mathf.PI / 180.0f;
                float cs = Mathf.Cos(r);
                float sn = Mathf.Sin(r);
                _matrix[0] = cs;
                _matrix[1] = sn;
                _matrix[4] = -sn;
                _matrix[5] = cs;
            }
        }

        /// <summary>
        /// Turn the specified object with a certain angle in degrees.
        /// </summary>
        /// <param name="angle">The angle<see cref="float"/></param>
        public void Turn(float angle)
        {
            rotation = _rotation + angle;
        }

        /// <summary>
        /// Move the object, based on its current rotation.
        /// </summary>
        /// <param name="stepX">The stepX<see cref="float"/></param>
        /// <param name="stepY">The stepY<see cref="float"/></param>
        public void Move(float stepX, float stepY)
        {
            float r = _rotation * Mathf.PI / 180.0f;
            float cs = Mathf.Cos(r);
            float sn = Mathf.Sin(r);
            _matrix[12] = (_matrix[12] + cs * stepX - sn * stepY);
            _matrix[13] = (_matrix[13] + sn * stepX + cs * stepY);
        }

        /// <summary>
        /// Move the object, in world space. (Object rotation is ignored)
        /// </summary>
        /// <param name="stepX">The stepX<see cref="float"/></param>
        /// <param name="stepY">The stepY<see cref="float"/></param>
        public void Translate(float stepX, float stepY)
        {
            _matrix[12] += stepX;
            _matrix[13] += stepY;
        }

        /// <summary>
        /// Sets the object's scaling
        /// </summary>
        /// <param name="scaleX">The scaleX<see cref="float"/></param>
        /// <param name="scaleY">The scaleY<see cref="float"/></param>
        public void SetScaleXY(float scaleX, float scaleY)
        {
            _scaleX = scaleX;
            _scaleY = scaleY;
        }

        /// <summary>
        /// Sets the object's scaling
        /// </summary>
        /// <param name="scale">The scale<see cref="float"/></param>
        public void SetScaleXY(float scale)
        {
            _scaleX = scale;
            _scaleY = scale;
        }

        /// <summary>
        /// Gets or sets the scaleX
        /// Sets the object's x-axis scale
        /// </summary>
        public float scaleX
        {
            get { return _scaleX; }
            set { _scaleX = value; }
        }

        /// <summary>
        /// Gets or sets the scaleY
        /// Sets the object's y-axis scale
        /// </summary>
        public float scaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; }
        }

        /// <summary>
        /// Gets or sets the scale
        /// Sets the object's x-axis and y-axis scale
		/// Note: This getter/setter is included for convenience only
		/// Reading this value will return scaleX, not scaleY!!
        /// </summary>
        public float scale
        {
            get
            {
                return _scaleX;
            }
            set
            {
                _scaleX = value;
                _scaleY = value;
            }
        }

        /// <summary>
        /// Returns the inverse matrix transformation, if it exists.
		/// (Use this e.g. for cameras used by sub windows)
        /// </summary>
        /// <returns>The <see cref="Transformable"/></returns>
        public Transformable Inverse()
        {
            Transformable inv = new Transformable();
            if (scaleX == 0 || scaleY == 0)
                throw new Exception("Cannot invert a transform with scale 0");
            float cs = _matrix[0];
            float sn = _matrix[1];
            inv._matrix[0] = cs / scaleX;
            inv._matrix[1] = -sn / scaleY;
            inv._matrix[4] = sn / scaleX;
            inv._matrix[5] = cs / scaleY;
            inv.x = (-x * cs - y * sn) / scaleX;
            inv.y = (x * sn - y * cs) / scaleY;
            return inv;
        }
    }
}
