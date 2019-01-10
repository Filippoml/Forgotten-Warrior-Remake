namespace GXPEngine.Core
{
    using System;
    using GXPEngine.OpenGL;

    /// <summary>
    /// Defines the <see cref="WindowSize" />
    /// </summary>
    internal class WindowSize
    {
        /// <summary>
        /// Defines the instance
        /// </summary>
        public static WindowSize instance = new WindowSize();

        /// <summary>
        /// Defines the width, height
        /// </summary>
        public int width, height;
    }

    /// <summary>
    /// Defines the <see cref="GLContext" />
    /// </summary>
    public class GLContext
    {
        /// <summary>
        /// Defines the MAXKEYS
        /// </summary>
        internal const int MAXKEYS = 65535;

        /// <summary>
        /// Defines the MAXBUTTONS
        /// </summary>
        internal const int MAXBUTTONS = 255;

        /// <summary>
        /// Defines the keys
        /// </summary>
        private static bool[] keys = new bool[MAXKEYS + 1];

        /// <summary>
        /// Defines the keydown
        /// </summary>
        private static bool[] keydown = new bool[MAXKEYS + 1];

        /// <summary>
        /// Defines the keyup
        /// </summary>
        private static bool[] keyup = new bool[MAXKEYS + 1];

        /// <summary>
        /// Defines the buttons
        /// </summary>
        private static bool[] buttons = new bool[MAXBUTTONS + 1];

        /// <summary>
        /// Defines the mousehits
        /// </summary>
        private static bool[] mousehits = new bool[MAXBUTTONS + 1];

        /// <summary>
        /// Defines the mouseup
        /// </summary>
        private static bool[] mouseup = new bool[MAXBUTTONS + 1];//mouseup kindly donated by LeonB

        /// <summary>
        /// Defines the mouseX
        /// </summary>
        public static int mouseX = 0;

        /// <summary>
        /// Defines the mouseY
        /// </summary>
        public static int mouseY = 0;

        /// <summary>
        /// Defines the _owner
        /// </summary>
        private Game _owner;

        /// <summary>
        /// Defines the _targetFrameRate
        /// </summary>
        private int _targetFrameRate = 60;

        /// <summary>
        /// Defines the _lastFrameTime
        /// </summary>
        private long _lastFrameTime = 0;

        /// <summary>
        /// Defines the _lastFPSTime
        /// </summary>
        private long _lastFPSTime = 0;

        /// <summary>
        /// Defines the _frameCount
        /// </summary>
        private int _frameCount = 0;

        /// <summary>
        /// Defines the _lastFPS
        /// </summary>
        private int _lastFPS = 0;

        /// <summary>
        /// Defines the _vsyncEnabled
        /// </summary>
        private bool _vsyncEnabled = false;

        /// <summary>
        /// Defines the _realToLogicWidthRatio
        /// </summary>
        private static double _realToLogicWidthRatio;

        /// <summary>
        /// Defines the _realToLogicHeightRatio
        /// </summary>
        private static double _realToLogicHeightRatio;

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderWindow()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="GLContext"/> class.
        /// </summary>
        /// <param name="owner">The owner<see cref="Game"/></param>
        public GLContext(Game owner)
        {
            _owner = owner;
            _lastFPS = _targetFrameRate;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Width
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the width
        /// </summary>
        public int width
        {
            get { return WindowSize.instance.width; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Height
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the height
        /// </summary>
        public int height
        {
            get { return WindowSize.instance.height; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														setupWindow()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The CreateWindow
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        /// <param name="fullScreen">The fullScreen<see cref="bool"/></param>
        /// <param name="vSync">The vSync<see cref="bool"/></param>
        /// <param name="realWidth">The realWidth<see cref="int"/></param>
        /// <param name="realHeight">The realHeight<see cref="int"/></param>
        public void CreateWindow(int width, int height, bool fullScreen, bool vSync, int realWidth, int realHeight)
        {
            // This stores the "logical" width, used by all the game logic:
            WindowSize.instance.width = width;
            WindowSize.instance.height = height;
            _realToLogicWidthRatio = (double)realWidth / width;
            _realToLogicHeightRatio = (double)realHeight / height;
            _vsyncEnabled = vSync;

            GL.glfwInit();

            GL.glfwOpenWindowHint(GL.GLFW_FSAA_SAMPLES, 8);
            GL.glfwOpenWindow(realWidth, realHeight, 8, 8, 8, 8, 24, 0, (fullScreen ? GL.GLFW_FULLSCREEN : GL.GLFW_WINDOWED));
            GL.glfwSetWindowTitle("Game");
            GL.glfwSwapInterval(vSync);

            GL.glfwSetKeyCallback(
                (int _key, int _mode) =>
                {
                    bool press = (_mode == 1);
                    if (press) keydown[_key] = true;
                    else keyup[_key] = true;
                    keys[_key] = press;
                });

            GL.glfwSetMouseButtonCallback(
                (int _button, int _mode) =>
                {
                    bool press = (_mode == 1);
                    if (press) mousehits[_button] = true;
                    else mouseup[_button] = true;
                    buttons[_button] = press;
                });

            GL.glfwSetWindowSizeCallback((int newWidth, int newHeight) =>
            {
                GL.Viewport(0, 0, newWidth, newHeight);
                GL.Enable(GL.MULTISAMPLE);
                GL.Enable(GL.TEXTURE_2D);
                GL.Enable(GL.BLEND);
                GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
                GL.Hint(GL.PERSPECTIVE_CORRECTION, GL.FASTEST);
                //GL.Enable (GL.POLYGON_SMOOTH);
                GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

                // Load the basic projection settings:
                GL.MatrixMode(GL.PROJECTION);
                GL.LoadIdentity();
                // Here's where the conversion from logical width/height to real width/height happens: 
                GL.Ortho(0.0f, newWidth / _realToLogicWidthRatio, newHeight / _realToLogicHeightRatio, 0.0f, 0.0f, 1000.0f);

                lock (WindowSize.instance)
                {
                    WindowSize.instance.width = (int)(newWidth / _realToLogicWidthRatio);
                    WindowSize.instance.height = (int)(newHeight / _realToLogicHeightRatio);
                }
            });
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														ShowCursor()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The ShowCursor
        /// </summary>
        /// <param name="enable">The enable<see cref="bool"/></param>
        public void ShowCursor(bool enable)
        {
            if (enable)
            {
                GL.glfwEnable(GL.GLFW_MOUSE_CURSOR);
            }
            else
            {
                GL.glfwDisable(GL.GLFW_MOUSE_CURSOR);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetScissor()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The SetScissor
        /// </summary>
        /// <param name="x">The x<see cref="int"/></param>
        /// <param name="y">The y<see cref="int"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        public void SetScissor(int x, int y, int width, int height)
        {
            if ((width == WindowSize.instance.width) && (height == WindowSize.instance.height))
            {
                GL.Disable(GL.SCISSOR_TEST);
            }
            else
            {
                GL.Enable(GL.SCISSOR_TEST);
            }

            GL.Scissor(
                (int)(x * _realToLogicWidthRatio),
                (int)(y * _realToLogicHeightRatio),
                (int)(width * _realToLogicWidthRatio),
                (int)(height * _realToLogicHeightRatio)
            );
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Close()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Close
        /// </summary>
        public void Close()
        {
            GL.glfwCloseWindow();
            GL.glfwTerminate();
            System.Environment.Exit(0);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Run()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Run
        /// </summary>
        public void Run()
        {
            //Update();
            GL.glfwSetTime(0.0);
            do
            {
                if (_vsyncEnabled || (Time.time - _lastFrameTime > (1000 / _targetFrameRate)))
                {
                    _lastFrameTime = Time.time;

                    //actual fps count tracker
                    _frameCount++;
                    if (Time.time - _lastFPSTime > 1000)
                    {
                        _lastFPS = (int)(_frameCount / ((Time.time - _lastFPSTime) / 1000.0f));
                        _lastFPSTime = Time.time;
                        _frameCount = 0;
                    }

                    UpdateMouseInput();
                    _owner.Step();

                    ResetHitCounters();
                    Display();

                    Time.newFrame();
                    GL.glfwPollEvents();
                }


            } while (GL.glfwGetWindowParam(GL.GLFW_ACTIVE) == 1);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														display()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Display
        /// </summary>
        private void Display()
        {
            GL.Clear(GL.COLOR_BUFFER_BIT);

            GL.MatrixMode(GL.MODELVIEW);
            GL.LoadIdentity();

            _owner.Render(this);

            GL.glfwSwapBuffers();
            if (GetKey(Key.ESCAPE)) this.Close();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetColor()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The SetColor
        /// </summary>
        /// <param name="r">The r<see cref="byte"/></param>
        /// <param name="g">The g<see cref="byte"/></param>
        /// <param name="b">The b<see cref="byte"/></param>
        /// <param name="a">The a<see cref="byte"/></param>
        public void SetColor(byte r, byte g, byte b, byte a)
        {
            GL.Color4ub(r, g, b, a);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														PushMatrix()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The PushMatrix
        /// </summary>
        /// <param name="matrix">The matrix<see cref="float[]"/></param>
        public void PushMatrix(float[] matrix)
        {
            GL.PushMatrix();
            GL.MultMatrixf(matrix);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														PopMatrix()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The PopMatrix
        /// </summary>
        public void PopMatrix()
        {
            GL.PopMatrix();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														DrawQuad()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The DrawQuad
        /// </summary>
        /// <param name="vertices">The vertices<see cref="float[]"/></param>
        /// <param name="uv">The uv<see cref="float[]"/></param>
        public void DrawQuad(float[] vertices, float[] uv)
        {
            GL.EnableClientState(GL.TEXTURE_COORD_ARRAY);
            GL.EnableClientState(GL.VERTEX_ARRAY);
            GL.TexCoordPointer(2, GL.FLOAT, 0, uv);
            GL.VertexPointer(2, GL.FLOAT, 0, vertices);
            GL.DrawArrays(GL.QUADS, 0, 4);
            GL.DisableClientState(GL.VERTEX_ARRAY);
            GL.DisableClientState(GL.TEXTURE_COORD_ARRAY);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetKey()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetKey
        /// </summary>
        /// <param name="key">The key<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetKey(int key)
        {
            return keys[key];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetKeyDown()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetKeyDown
        /// </summary>
        /// <param name="key">The key<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetKeyDown(int key)
        {
            return keydown[key];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetKeyUp()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetKeyUp
        /// </summary>
        /// <param name="key">The key<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetKeyUp(int key)
        {
            return keyup[key];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetMouseButton()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetMouseButton
        /// </summary>
        /// <param name="button">The button<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetMouseButton(int button)
        {
            return buttons[button];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetMouseButtonDown()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetMouseButtonDown
        /// </summary>
        /// <param name="button">The button<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetMouseButtonDown(int button)
        {
            return mousehits[button];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetMouseButtonUp()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetMouseButtonUp
        /// </summary>
        /// <param name="button">The button<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool GetMouseButtonUp(int button)
        {
            return mouseup[button];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														ResetHitCounters()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The ResetHitCounters
        /// </summary>
        public static void ResetHitCounters()
        {
            Array.Clear(keydown, 0, MAXKEYS);
            Array.Clear(keyup, 0, MAXKEYS);
            Array.Clear(mousehits, 0, MAXBUTTONS);
            Array.Clear(mouseup, 0, MAXBUTTONS);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														UpdateMouseInput()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The UpdateMouseInput
        /// </summary>
        public static void UpdateMouseInput()
        {
            GL.glfwGetMousePos(out mouseX, out mouseY);
            mouseX = (int)(mouseX / _realToLogicWidthRatio);
            mouseY = (int)(mouseY / _realToLogicHeightRatio);
        }

        /// <summary>
        /// Gets the currentFps
        /// </summary>
        public int currentFps
        {
            get { return _lastFPS; }
        }

        /// <summary>
        /// Gets or sets the targetFps
        /// </summary>
        public int targetFps
        {
            get { return _targetFrameRate; }
            set
            {
                if (value < 1)
                {
                    _targetFrameRate = 1;
                }
                else
                {
                    _targetFrameRate = value;
                }
            }
        }
    }
}
