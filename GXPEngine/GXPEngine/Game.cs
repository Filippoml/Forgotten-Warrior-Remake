namespace GXPEngine
{
    using System;
    using System.Collections.Generic;
    using GXPEngine.Core;
    using GXPEngine.Managers;

    /// <summary>
    /// The Game class represents the Game window.
	/// Only a single instance of this class is allowed.
    /// </summary>
    public class Game : GameObject
    {
        /// <summary>
        /// Defines the main
        /// </summary>
        internal static Game main = null;

        /// <summary>
        /// Defines the _glContext
        /// </summary>
        private GLContext _glContext;

        /// <summary>
        /// Defines the _updateManager
        /// </summary>
        private UpdateManager _updateManager;

        /// <summary>
        /// Defines the _collisionManager
        /// </summary>
        private CollisionManager _collisionManager;

        /// <summary>
        /// Defines the _gameObjectsContained
        /// </summary>
        private List<GameObject> _gameObjectsContained;

        /// <summary>
		/// Step delegate defines the signature of a method used for step callbacks, see OnBeforeStep, OnAfterStep.
		/// </summary>
        public delegate void StepDelegate();

        /// <summary>
        /// Occurs before the engine starts the new update loop. This allows you to define general manager classes that can update itself on/after each frame.
        /// </summary>
        public event StepDelegate OnBeforeStep;

        /// <summary>
        /// Occurs after the engine has finished its last update loop. This allows you to define general manager classes that can update itself on/after each frame.
        /// </summary>
        public event StepDelegate OnAfterStep;

        /// <summary>
        /// The RenderDelegate
        /// </summary>
        /// <param name="glContext">The glContext<see cref="GLContext"/></param>
        public delegate void RenderDelegate(GLContext glContext);

        /// <summary>
        /// Defines the OnAfterRender
        /// </summary>
        public event RenderDelegate OnAfterRender;

        /// <summary>
        /// Gets or sets the RenderRange
        /// Sprites will be rendered if and only if they overlap with this rectangle. 
		/// Default value: (0,0,game.width,game.height). 
		/// You only need to change this when rendering to subwindows (e.g. split screen).
        /// </summary>
        public Rectangle RenderRange
        {
            get
            {
                return _renderRange;
            }
            set
            {
                _renderRange = value;
            }
        }

        /// <summary>
        /// Defines the PixelArt
        /// </summary>
        public readonly bool PixelArt;

        /// <summary>
        /// Defines the _renderRange
        /// </summary>
        private Rectangle _renderRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="pWidth">The pWidth<see cref="int"/></param>
        /// <param name="pHeight">The pHeight<see cref="int"/></param>
        /// <param name="pFullScreen">The pFullScreen<see cref="bool"/></param>
        /// <param name="pVSync">The pVSync<see cref="bool"/></param>
        /// <param name="pRealWidth">The pRealWidth<see cref="int"/></param>
        /// <param name="pRealHeight">The pRealHeight<see cref="int"/></param>
        /// <param name="pPixelArt">The pPixelArt<see cref="bool"/></param>
        public Game(int pWidth, int pHeight, bool pFullScreen, bool pVSync = true, int pRealWidth = -1, int pRealHeight = -1, bool pPixelArt = false) : base()
        {
            if (pRealWidth <= 0)
            {
                pRealWidth = pWidth;
            }
            if (pRealHeight <= 0)
            {
                pRealHeight = pHeight;
            }
            PixelArt = pPixelArt;

            if (PixelArt)
            {
                // offset should be smaller than 1/(2 * "pixelsize"), but not zero:
                x = 0.01f;
                y = 0.01f;
            }

            if (main != null)
            {
                throw new Exception("Only a single instance of Game is allowed");
            }
            else
            {

                main = this;
                _updateManager = new UpdateManager();
                _collisionManager = new CollisionManager();
                _glContext = new GLContext(this);
                _glContext.CreateWindow(pWidth, pHeight, pFullScreen, pVSync, pRealWidth, pRealHeight);
                _gameObjectsContained = new List<GameObject>();

                _renderRange = new Rectangle(0, 0, pWidth, pHeight);

                //register ourselves for updates
                Add(this);

            }
        }

        /// <summary>
        /// Sets the rendering output view port. All rendering will be done within the given rectangle.
		/// The default setting is {0, 0, game.width, game.height}.
        /// </summary>
        /// <param name="x">The x<see cref="int"/></param>
        /// <param name="y">The y<see cref="int"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        public void SetViewport(int x, int y, int width, int height)
        {
            // Translate from GXPEngine coordinates (origin top left) to OpenGL coordinates (origin bottom left):
            //Console.WriteLine ("Setting viewport to {0},{1},{2},{3}",x,y,width,height);
            _glContext.SetScissor(x, game.height - height - y, width, height);
        }

        /// <summary>
        /// Shows or hides the mouse cursor.
        /// </summary>
        /// <param name="enable">The enable<see cref="bool"/></param>
        public void ShowMouse(bool enable)
        {
            _glContext.ShowCursor(enable);
        }

        /// <summary>
        /// Start the game loop. Call this once at the start of your game.
        /// </summary>
        public void Start()
        {
            _glContext.Run();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Step()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Step
        /// </summary>
        internal void Step()
        {
            Sound.Step();

            if (OnBeforeStep != null)
                OnBeforeStep();
            _updateManager.Step();
            _collisionManager.Step();
            if (OnAfterStep != null)
                OnAfterStep();
        }

        /// <summary>
        /// Defines the recurse
        /// </summary>
        internal bool recurse = true;

        /// <summary>
        /// The Render
        /// </summary>
        /// <param name="glContext">The glContext<see cref="GLContext"/></param>
        public override void Render(GLContext glContext)
        {
            base.Render(glContext);
            if (OnAfterRender != null && recurse)
            {
                recurse = false;
                OnAfterRender(glContext);
                recurse = true;
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
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Add()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Add
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        internal void Add(GameObject gameObject)
        {
            if (!_gameObjectsContained.Contains(gameObject))
            {
                _updateManager.Add(gameObject);
                _collisionManager.Add(gameObject);
                _gameObjectsContained.Add(gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Remove()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Remove
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        internal void Remove(GameObject gameObject)
        {
            if (_gameObjectsContained.Contains(gameObject))
            {
                _updateManager.Remove(gameObject);
                _collisionManager.Remove(gameObject);
                _gameObjectsContained.Remove(gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Contains()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Contains
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="Boolean"/></returns>
        public Boolean Contains(GameObject gameObject)
        {
            return _gameObjectsContained.Contains(gameObject);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetGameObjectCollisions()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetGameObjectCollisions
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="GameObject[]"/></returns>
        internal GameObject[] GetGameObjectCollisions(GameObject gameObject)
        {
            return _collisionManager.GetCurrentCollisions(gameObject);
        }

        /// <summary>
        /// Gets the width
        /// Returns the width of the window.
        /// </summary>
        public int width
        {
            get { return _glContext.width; }
        }

        /// <summary>
        /// Gets the height
        /// Returns the height of the window.
        /// </summary>
        public int height
        {
            get { return _glContext.height; }
        }

        /// <summary>
        /// Destroys the game, and closes the game window.
        /// </summary>
        override public void Destroy()
        {
            base.Destroy();
            _glContext.Close();
        }

        /// <summary>
        /// Gets the currentFps
        /// </summary>
        public int currentFps
        {
            get
            {
                return _glContext.currentFps;
            }
        }

        /// <summary>
        /// Gets or sets the targetFps
        /// </summary>
        public int targetFps
        {
            get
            {
                return _glContext.targetFps;
            }
            set
            {
                _glContext.targetFps = value;
            }
        }

        /// <summary>
        /// The CountSubtreeSize
        /// </summary>
        /// <param name="subtreeRoot">The subtreeRoot<see cref="GameObject"/></param>
        /// <returns>The <see cref="int"/></returns>
        internal int CountSubtreeSize(GameObject subtreeRoot)
        {
            int counter = 1; // for the root
            foreach (GameObject child in subtreeRoot.GetChildren())
            {
                counter += CountSubtreeSize(child);
            }
            return counter;
        }

        /// <summary>
        /// The GetDiagnostics
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string GetDiagnostics()
        {
            string output = "";
            output += "Number of game objects contained: " + _gameObjectsContained.Count + '\n';
            output += "Number of objects in hierarchy: " + CountSubtreeSize(this) + '\n';
            output += "OnBeforeStep delegates: " + (OnBeforeStep == null ? 0 : OnBeforeStep.GetInvocationList().Length) + '\n';
            output += "OnAfterStep delegates: " + (OnAfterStep == null ? 0 : OnAfterStep.GetInvocationList().Length) + '\n';
            output += "OnAfterRender delegates: " + (OnAfterRender == null ? 0 : OnAfterRender.GetInvocationList().Length) + '\n';
            output += Texture2D.GetDiagnostics();
            output += _collisionManager.GetDiagnostics();
            output += _updateManager.GetDiagnostics();
            return output;
        }
    }
}
