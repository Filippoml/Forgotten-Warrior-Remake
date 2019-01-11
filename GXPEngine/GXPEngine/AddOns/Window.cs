namespace GXPEngine
{
    using System.Drawing;
    using GXPEngine.Core;
    using GXPEngine.OpenGL;

    /// <summary>
    /// A class that can be used to create "sub windows" (e.g. mini-map, splitscreen, etc).
	/// This is not a gameobject. Instead, subscribe the RenderWindow method to the main game's 
	/// OnAfterRender event.
    /// </summary>
    internal class Window
    {
        /// <summary>
        /// Gets or sets the windowX
        /// The x coordinate of the window's left side
        /// </summary>
        public int windowX
        {
            get
            {
                return _windowX;
            }
            set
            {
                _windowX = value;
                _dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the windowY
        /// The y coordinate of the window's top
        /// </summary>
        public int windowY
        {
            get
            {
                return _windowY;
            }
            set
            {
                _windowY = value;
                _dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the width
        /// The window's width
        /// </summary>
        public int width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                _dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the height
        /// The window's height
        /// </summary>
        public int height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                _dirty = true;
            }
        }

        /// <summary>
        /// The game object (which should be in the hierarchy!) that determines the focus point, rotation and scale
		/// of the viewport window.
        /// </summary>
        public GameObject camera;

        // private variables:
        /// <summary>
        /// Defines the _windowX, _windowY
        /// </summary>
        internal int _windowX, _windowY;

        /// <summary>
        /// Defines the _width, _height
        /// </summary>
        internal int _width, _height;

        /// <summary>
        /// Defines the _dirty
        /// </summary>
        internal bool _dirty = true;

        /// <summary>
        /// Defines the window
        /// </summary>
        internal Transformable window;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="x">The x<see cref="int"/></param>
        /// <param name="y">The y<see cref="int"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        /// <param name="camera">The camera<see cref="GameObject"/></param>
        public Window(int x, int y, int width, int height, GameObject camera)
        {
            _windowX = x;
            _windowY = y;
            _width = width;
            _height = height;
            this.camera = camera;
            window = new Transformable();
        }

        /// <summary>
        /// To render the scene in this window, subscribe this method to the main game's OnAfterRender event.
        /// </summary>
        /// <param name="glContext">The glContext<see cref="GLContext"/></param>
        public void RenderWindow(GLContext glContext)
        {

            if (_dirty)
            {
                window.x = _windowX + _width / 2;
                window.y = _windowY + _height / 2;
                _dirty = false;
            }
            glContext.PushMatrix(window.matrix);

            int pushes = 1;
            GameObject current = camera;
            Transformable cameraInverse;
            while (true)
            {
                cameraInverse = current.Inverse();
                glContext.PushMatrix(cameraInverse.matrix);
                pushes++;
                if (current.parent == null)
                    break;
                current = current.parent;
            }

            if (current is Game)
            {// otherwise, the camera is not in the scene hierarchy, so render nothing - not even a black background
                Game main = Game.main;
                SetRenderRange();
                main.SetViewport(_windowX, _windowY, _width, _height);
                GL.Clear(GL.COLOR_BUFFER_BIT);
                current.Render(glContext);
                main.SetViewport(0, 0, Game.main.width, Game.main.height);
                main.RenderRange = new GXPEngine.Core.Rectangle(0, 0, main.width, main.height);
                
            }

            for (int i = 0; i < pushes; i++)
            {
                glContext.PopMatrix();
            }
        }

        /// <summary>
        /// The SetRenderRange
        /// </summary>
        internal void SetRenderRange()
        {
            Vector2[] worldSpaceCorners = new Vector2[4];
            worldSpaceCorners[0] = camera.TransformPoint(-_width / 2, -_height / 2);
            worldSpaceCorners[1] = camera.TransformPoint(-_width / 2, _height / 2);
            worldSpaceCorners[2] = camera.TransformPoint(_width / 2, _height / 2);
            worldSpaceCorners[3] = camera.TransformPoint(_width / 2, -_height / 2);

            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                if (worldSpaceCorners[i].x > maxX) maxX = worldSpaceCorners[i].x;
                if (worldSpaceCorners[i].x < minX) minX = worldSpaceCorners[i].x;
                if (worldSpaceCorners[i].y > maxY) maxY = worldSpaceCorners[i].y;
                if (worldSpaceCorners[i].y < minY) minY = worldSpaceCorners[i].y;
            }
            Game.main.RenderRange = new GXPEngine.Core.Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
