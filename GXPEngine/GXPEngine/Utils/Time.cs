namespace GXPEngine
{
    /// <summary>
    /// Contains various time related functions.
    /// </summary>
    public class Time
    {
        /// <summary>
        /// Defines the previousTime
        /// </summary>
        private static int previousTime;

        /// <summary>
        /// Initializes static members of the <see cref="Time"/> class.
        /// </summary>
        static Time()
        {
        }

        /// <summary>
        /// Gets the now
        /// Returns the current system time in milliseconds
        /// </summary>
        public static int now
        {
            get { return System.Environment.TickCount; }
        }

        /// <summary>
        /// Gets the time
        /// Returns this time in milliseconds since the program started
        /// </summary>
        public static int time
        {
            get { return (int)(OpenGL.GL.glfwGetTime() * 1000); }
        }

        /// <summary>
        /// Returns the time in milliseconds that has passed since the previous frame
        /// </summary>
        private static int previousFrameTime;

        /// <summary>
        /// Gets the deltaTime
        /// </summary>
        public static int deltaTime
        {
            get
            {
                return previousFrameTime;
            }
        }

        /// <summary>
        /// The newFrame
        /// </summary>
        internal static void newFrame()
        {
            previousFrameTime = time - previousTime;
            previousTime = time;
        }
    }
}
