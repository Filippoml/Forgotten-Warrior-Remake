namespace GXPEngine
{
    using System;
    using GXPEngine.Core;

    /// <summary>
    /// Animated Sprite. Has all the functionality of a regular sprite, but supports multiple animation frames/subimages.
    /// </summary>
    public class AnimationSprite : Sprite
    {
        /// <summary>
        /// Defines the _frameWidth
        /// </summary>
        protected float _frameWidth;

        /// <summary>
        /// Defines the _frameHeight
        /// </summary>
        protected float _frameHeight;

        /// <summary>
        /// Defines the _cols
        /// </summary>
        protected int _cols;

        /// <summary>
        /// Defines the _frames
        /// </summary>
        protected int _frames;

        /// <summary>
        /// Defines the _currentFrame
        /// </summary>
        protected int _currentFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSprite"/> class.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="cols">The cols<see cref="int"/></param>
        /// <param name="rows">The rows<see cref="int"/></param>
        /// <param name="frames">The frames<see cref="int"/></param>
        /// <param name="keepInCache">The keepInCache<see cref="bool"/></param>
        public AnimationSprite(string filename, int cols, int rows, int frames = -1, bool keepInCache = false) : base(filename, keepInCache)
        {
            name = filename;
            initializeAnimFrames(cols, rows, frames);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSprite"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap<see cref="System.Drawing.Bitmap"/></param>
        /// <param name="cols">The cols<see cref="int"/></param>
        /// <param name="rows">The rows<see cref="int"/></param>
        /// <param name="frames">The frames<see cref="int"/></param>
        public AnimationSprite(System.Drawing.Bitmap bitmap, int cols, int rows, int frames = -1) : base(bitmap)
        {
            name = "BMP " + bitmap.Width + "x" + bitmap.Height;
            initializeAnimFrames(cols, rows, frames);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														initializeAnimFrames()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The initializeAnimFrames
        /// </summary>
        /// <param name="cols">The cols<see cref="int"/></param>
        /// <param name="rows">The rows<see cref="int"/></param>
        /// <param name="frames">The frames<see cref="int"/></param>
        protected void initializeAnimFrames(int cols, int rows, int frames = -1)
        {
            if (frames < 0) frames = rows * cols;
            if (frames > rows * cols) frames = rows * cols;
            if (frames < 1) return;
            _cols = cols;
            _frames = frames;

            _frameWidth = 1.0f / (float)cols;
            _frameHeight = 1.0f / (float)rows;
            _bounds = new Rectangle(0, 0, _texture.width * _frameWidth, _texture.height * _frameHeight);

            _currentFrame = -1;
            SetFrame(0);
        }

        /// <summary>
        /// Gets or sets the sprite's width in pixels.
        /// </summary>
        override public int width
        {
            get
            {
                if (_texture != null) return (int)Math.Abs(_texture.width * _scaleX * _frameWidth);
                return 0;
            }
            set
            {
                if (_texture != null && _texture.width != 0) scaleX = value / ((float)_texture.width * _frameWidth);
            }
        }

        /// <summary>
        /// Gets or sets the sprite's height in pixels.
        /// </summary>
        override public int height
        {
            get
            {
                if (_texture != null) return (int)Math.Abs(_texture.height * _scaleY * _frameHeight);
                return 0;
            }
            set
            {
                if (_texture != null && _texture.height != 0) scaleY = value / ((float)_texture.height * _frameHeight);
            }
        }

        /// <summary>
        /// Sets the current animation frame.
		/// Frame should be in range 0...frameCount-1
        /// </summary>
        /// <param name="frame">The frame<see cref="int"/></param>
        public void SetFrame(int frame)
        {
            if (frame == _currentFrame) return;
            if (frame < 0) frame = 0;
            if (frame >= _frames) frame = _frames - 1;
            _currentFrame = frame;
            setUVs();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														setUVs
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The setUVs
        /// </summary>
        protected override void setUVs()
        {
            if (_cols == 0) return;

            int frameX = _currentFrame % _cols;
            int frameY = _currentFrame / _cols;

            float left = _frameWidth * frameX;
            float right = left + _frameWidth;

            float top = _frameHeight * frameY;
            float bottom = top + _frameHeight;

            if (!game.PixelArt)
            {
                //fix1
                float wp = .5f / _texture.width;
                left += wp;
                right -= wp;
                //end fix1

                //fix2
                float hp = .5f / _texture.height;
                top += hp;
                bottom -= hp;
                //end fix2
            }

            float frameLeft = _mirrorX ? right : left;
            float frameRight = _mirrorX ? left : right;

            float frameTop = _mirrorY ? bottom : top;
            float frameBottom = _mirrorY ? top : bottom;

            _uvs = new float[8] {
                frameLeft, frameTop, frameRight, frameTop,
                frameRight, frameBottom, frameLeft, frameBottom };
        }

        /// <summary>
        /// Goes to the next frame. At the end of the animation, it jumps back to the first frame. (It loops)
        /// </summary>
        public void NextFrame()
        {
            int frame = _currentFrame + 1;
            if (frame >= _frames) frame = 0;
            SetFrame(frame);
        }

        /// <summary>
        /// Gets or sets the currentFrame
        /// Returns the current frame.
        /// </summary>
        public int currentFrame
        {
            get { return _currentFrame; }
            set { SetFrame(value); }
        }

        /// <summary>
        /// Gets the frameCount
        /// Returns the number of frames in this animation.
        /// </summary>
        public int frameCount
        {
            get { return _frames; }
        }
    }

    //legacy, sorry Hans
    /// <summary>
    /// Defines the <see cref="AnimSprite" />
    /// </summary>
    public class AnimSprite : AnimationSprite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimSprite"/> class.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="cols">The cols<see cref="int"/></param>
        /// <param name="rows">The rows<see cref="int"/></param>
        /// <param name="frames">The frames<see cref="int"/></param>
        public AnimSprite(string filename, int cols, int rows, int frames = -1) : base(filename, cols, rows, frames)
        {
        }
    }

;
}
