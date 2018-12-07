using System;
using GXPEngine.Core;
using GXPEngine.Managers;
using System.Collections.Generic;

namespace GXPEngine
{
	/// <summary>
	/// The Game class represents the Game window.
	/// Only a single instance of this class is allowed.
	/// </summary>
	public class Game : GameObject
	{
		internal static Game main = null;

		private GLContext _glContext;

		private UpdateManager _updateManager;
		private CollisionManager _collisionManager;
		private List<GameObject> _gameObjectsContained;

		/// <summary>
		/// Step delegate defines the signature of a method used for step callbacks, see OnBeforeStep, OnAfterStep.
		/// </summary>
		public delegate void StepDelegate ();
		/// <summary>
		/// Occurs before the engine starts the new update loop. This allows you to define general manager classes that can update itself on/after each frame.
		/// </summary>
		public event StepDelegate OnBeforeStep;
		/// <summary>
		/// Occurs after the engine has finished its last update loop. This allows you to define general manager classes that can update itself on/after each frame.
		/// </summary>
		public event StepDelegate OnAfterStep;

		public delegate void RenderDelegate (GLContext glContext);
		public event RenderDelegate OnAfterRender;

		/// <summary>
		/// Sprites will be rendered if and only if they overlap with this rectangle. 
		/// Default value: (0,0,game.width,game.height). 
		/// You only need to change this when rendering to subwindows (e.g. split screen).
		/// </summary>
		/// <value>The render range.</value>
		public Rectangle RenderRange {
			get {
				return _renderRange;
			}
			set {
				_renderRange = value;
			}
		}

		public readonly bool PixelArt;

		private Rectangle _renderRange;
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Game()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.Game"/> class.
		/// This class represents a game window, containing an openGL view.
		/// </summary>
		/// <param name='width'>
		/// Width of the window in pixels.
		/// </param>
		/// <param name='height'>
		/// Height of the window in pixels.
		/// </param>
		/// <param name='fullScreen'>
		/// If set to <c>true</c> the application will run in fullscreen mode.
		/// </param>
		public Game (int pWidth, int pHeight, bool pFullScreen, bool pVSync = true, int pRealWidth=-1, int pRealHeight=-1, bool pPixelArt=false) : base()
		{
			if (pRealWidth <= 0) {
				pRealWidth = pWidth;
			}
			if (pRealHeight <= 0) {
				pRealHeight = pHeight;
			}
			PixelArt = pPixelArt;

			if (PixelArt) {
				// offset should be smaller than 1/(2 * "pixelsize"), but not zero:
				x = 0.01f;
				y = 0.01f;
			}
			
			if (main != null) {
				throw new Exception ("Only a single instance of Game is allowed");
			} else {

				main = this;
				_updateManager = new UpdateManager ();
				_collisionManager = new CollisionManager ();
				_glContext = new GLContext (this);
				_glContext.CreateWindow (pWidth, pHeight, pFullScreen, pVSync, pRealWidth, pRealHeight);
				_gameObjectsContained = new List<GameObject>();

				_renderRange = new Rectangle (0, 0, pWidth, pHeight);

				//register ourselves for updates
				Add (this);

			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														SetViewPort()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the rendering output view port. All rendering will be done within the given rectangle.
		/// The default setting is {0, 0, game.width, game.height}.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate.
		/// </param>
		/// <param name='y'>
		/// The y coordinate.
		/// </param>
		/// <param name='width'>
		/// The new width of the viewport.
		/// </param>
		/// <param name='height'>
		/// The new height of the viewport.
		/// </param>
		public void SetViewport(int x, int y, int width, int height) {
			// Translate from GXPEngine coordinates (origin top left) to OpenGL coordinates (origin bottom left):
			//Console.WriteLine ("Setting viewport to {0},{1},{2},{3}",x,y,width,height);
			_glContext.SetScissor(x, game.height - height - y, width, height);
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														ShowMouse()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Shows or hides the mouse cursor.
		/// </summary>
		/// <param name='enable'>
		/// Set this to 'true' to enable the cursor.
		/// Else, set this to 'false'.
		/// </param>
		public void ShowMouse (bool enable)
		{
			_glContext.ShowCursor(enable);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Start()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Start the game loop. Call this once at the start of your game.
		/// </summary>
		public void Start() {
			_glContext.Run();
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Step()
		//------------------------------------------------------------------------------------------------------------------------
		internal void Step ()
		{
			Sound.Step ();

			if (OnBeforeStep != null)
				OnBeforeStep ();
			_updateManager.Step ();
			_collisionManager.Step ();
			if (OnAfterStep != null)
				OnAfterStep ();
		}

		bool recurse=true;

		public override void Render(GLContext glContext) {
			base.Render (glContext);
			if (OnAfterRender != null && recurse) {
				recurse = false;
				OnAfterRender (glContext);
				recurse = true;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														RenderSelf()
		//------------------------------------------------------------------------------------------------------------------------
		override protected void RenderSelf(GLContext glContext) {
			//empty
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														Add()
		//------------------------------------------------------------------------------------------------------------------------
		internal void Add (GameObject gameObject)
		{
			if (!_gameObjectsContained.Contains (gameObject)) {
				_updateManager.Add (gameObject);
				_collisionManager.Add (gameObject);
				_gameObjectsContained.Add (gameObject);
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Remove()
		//------------------------------------------------------------------------------------------------------------------------
		internal void Remove (GameObject gameObject)
		{
			if (_gameObjectsContained.Contains (gameObject)) {
				_updateManager.Remove (gameObject);
				_collisionManager.Remove (gameObject);
				_gameObjectsContained.Remove (gameObject);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														Contains()
		//------------------------------------------------------------------------------------------------------------------------
		public Boolean Contains(GameObject gameObject) {
			return _gameObjectsContained.Contains(gameObject);
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														GetGameObjectCollisions()
		//------------------------------------------------------------------------------------------------------------------------
		internal GameObject[] GetGameObjectCollisions (GameObject gameObject)
		{
			return _collisionManager.GetCurrentCollisions(gameObject);
		}


		//------------------------------------------------------------------------------------------------------------------------
		//														width
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the width of the window.
		/// </summary>
		public int width {
			get { return _glContext.width; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														height
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the height of the window.
		/// </summary>
		public int height {
			get { return _glContext.height; }
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														Destroy()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Destroys the game, and closes the game window.
		/// </summary>
		override public void Destroy ()
		{
			base.Destroy();
			_glContext.Close();
		}

		public int currentFps {
			get {
				return _glContext.currentFps;
			}
		}

		public int targetFps {
			get { 
				return _glContext.targetFps; 
			}
			set {
				_glContext.targetFps = value;
			}
		}	

		int CountSubtreeSize(GameObject subtreeRoot) {
			int counter=1; // for the root
			foreach (GameObject child in subtreeRoot.GetChildren()) {
				counter += CountSubtreeSize (child);
			}
			return counter;
		}

		public string GetDiagnostics() {
			string output = "";
			output += "Number of game objects contained: "+_gameObjectsContained.Count+'\n';
			output += "Number of objects in hierarchy: " + CountSubtreeSize (this)+'\n';
			output += "OnBeforeStep delegates: "+(OnBeforeStep==null?0:OnBeforeStep.GetInvocationList().Length)+'\n';
			output += "OnAfterStep delegates: "+(OnAfterStep==null?0:OnAfterStep.GetInvocationList().Length)+'\n';
			output += "OnAfterRender delegates: "+(OnAfterRender==null?0:OnAfterRender.GetInvocationList().Length)+'\n';
			output += Texture2D.GetDiagnostics ();
			output += _collisionManager.GetDiagnostics (); 
			output += _updateManager.GetDiagnostics (); 
			return output;
		}
	}
}

