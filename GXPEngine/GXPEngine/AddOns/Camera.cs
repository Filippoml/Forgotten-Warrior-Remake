namespace GXPEngine
{
    /// <summary>
    /// A Camera gameobject, that owns a rectangular render window, and determines the focal point, rotation and scale
	/// of what's rendered in that window.
	/// (Don't forget to add this as child somewhere in the hierarchy.)
    /// </summary>
    internal class Camera : GameObject
    {
        /// <summary>
        /// Defines the _renderTarget
        /// </summary>
        internal Window _renderTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="windowX">Left x coordinate of the render window.</param>
        /// <param name="windowY">Top y coordinate of the render window.</param>
        /// <param name="windowWidth">Width of the render window.</param>
        /// <param name="windowHeight">Height of the render window.</param>
        public Camera(int windowX, int windowY, int windowWidth, int windowHeight)
        {
            _renderTarget = new Window(windowX, windowY, windowWidth, windowHeight, this);
            game.OnAfterRender += _renderTarget.RenderWindow;
        }

        /// <summary>
        /// The OnDestroy
        /// </summary>
        protected override void OnDestroy()
        {
            game.OnAfterRender -= _renderTarget.RenderWindow;
        }
    }
}
