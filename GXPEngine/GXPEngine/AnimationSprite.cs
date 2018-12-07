using System;
using GXPEngine.Core;

namespace GXPEngine
{
	/// <summary>
	/// Animated Sprite. Has all the functionality of a regular sprite, but supports multiple animation frames/subimages.
	/// </summary>
	public class AnimationSprite : Sprite
	{
		protected float _frameWidth;
		protected float _frameHeight;
		
		protected int _cols;
		protected int _frames;
		protected int _currentFrame;
		
		//------------------------------------------------------------------------------------------------------------------------
		//														AnimSprite()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.AnimSprite"/> class.
		/// </summary>
		/// <param name='filename'>
		/// The name of the file to be loaded. Files are cached internally.
		/// Texture sizes should be a power of two: 1, 2, 4, 8, 16, 32, 64 etc.
		/// The width and height don't need to be the same.
		/// If you want to load transparent sprites, use .PNG with transparency.
		/// </param>
		/// <param name='cols'>
		/// Number of columns in the animation.
		/// </param>
		/// <param name='rows'>
		/// Number of rows in the animation.
		/// </param>
		/// <param name='frames'>
		/// Optionally, indicate a number of frames. When left blank, defaults to width*height.
		/// </param>
		public AnimationSprite (string filename, int cols, int rows, int frames=-1, bool keepInCache=false) : base(filename,keepInCache)
		{
			name = filename;
			initializeAnimFrames(cols, rows, frames);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.AnimSprite"/> class.
		/// </summary>
		/// <param name='bitmap'>
		/// The Bitmap object to be used to create the sprite. 
		/// Texture sizes should be a power of two: 1, 2, 4, 8, 16, 32, 64 etc.
		/// The width and height don't need to be the same.
		/// If you want to load transparent sprites, use .PNG with transparency.
		/// </param>
		/// <param name='cols'>
		/// Number of columns in the animation.
		/// </param>
		/// <param name='rows'>
		/// Number of rows in the animation.
		/// </param>
		/// <param name='frames'>
		/// Optionally, indicate a number of frames. When left blank, defaults to width*height.
		/// </param>
		public AnimationSprite (System.Drawing.Bitmap bitmap, int cols, int rows, int frames=-1) : base(bitmap)
		{
			name = "BMP " + bitmap.Width + "x" + bitmap.Height;
			initializeAnimFrames(cols, rows, frames);
		}
			
		//------------------------------------------------------------------------------------------------------------------------
		//														initializeAnimFrames()
		//------------------------------------------------------------------------------------------------------------------------
		protected void initializeAnimFrames(int cols, int rows, int frames=-1) 
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

		//------------------------------------------------------------------------------------------------------------------------
		//														width
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the sprite's width in pixels.
		/// </summary>
		override public int width {
			get { 
				if (_texture != null) return (int)Math.Abs(_texture.width * _scaleX * _frameWidth);
				return 0;
			}
			set {
				if (_texture != null && _texture.width != 0) scaleX = value / ((float)_texture.width * _frameWidth);
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														height
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the sprite's height in pixels.
		/// </summary>
		override public int height {
			get { 
				if (_texture != null) return (int)Math.Abs(_texture.height * _scaleY * _frameHeight);
				return 0;
			}
			set {
				if (_texture != null && _texture.height != 0) scaleY = value / ((float)_texture.height * _frameHeight);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														SetFrame()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the current animation frame.
		/// Frame should be in range 0...frameCount-1
		/// </summary>
		/// <param name='frame'>
		/// Frame.
		/// </param>
		public void SetFrame(int frame) {
			if (frame == _currentFrame) return;
			if (frame < 0) frame = 0;
			if (frame >= _frames) frame = _frames - 1;
			_currentFrame = frame;
			setUVs();
		}
				
		//------------------------------------------------------------------------------------------------------------------------
		//														setUVs
		//------------------------------------------------------------------------------------------------------------------------
		protected override void setUVs() {
			if (_cols == 0) return;

			int frameX = _currentFrame % _cols;
			int frameY = _currentFrame / _cols;

			float left = _frameWidth * frameX;
			float right = left + _frameWidth;

			float top = _frameHeight * frameY;
			float bottom = top + _frameHeight;

			if (!game.PixelArt) {
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

			float frameLeft = _mirrorX?right:left;
			float frameRight = _mirrorX?left:right;

			float frameTop = _mirrorY?bottom:top;
			float frameBottom = _mirrorY?top:bottom;

			_uvs = new float[8] {
				frameLeft, frameTop, frameRight, frameTop,
				frameRight, frameBottom, frameLeft, frameBottom };
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														NextFrame()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Goes to the next frame. At the end of the animation, it jumps back to the first frame. (It loops)
		/// </summary>
		public void NextFrame() {
			int frame = _currentFrame + 1;
			if (frame >= _frames) frame = 0;
			SetFrame(frame);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														currentFrame
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the current frame.
		/// </summary>
		public int currentFrame {
			get { return _currentFrame; }
			set { SetFrame (value); }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														frameCount
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the number of frames in this animation.
		/// </summary>
		public int frameCount {
			get { return _frames; }
		}

	}

	//legacy, sorry Hans
	public class AnimSprite : AnimationSprite {
		public AnimSprite (string filename, int cols, int rows, int frames=-1) : base(filename, cols, rows, frames) {}
	};
}

