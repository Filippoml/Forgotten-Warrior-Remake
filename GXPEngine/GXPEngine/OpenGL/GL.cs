namespace GXPEngine.OpenGL
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Defines the <see cref="GL" />
    /// </summary>
    public class GL
    {
        //----------------------------------------------------------------------------------------------------------------------
        //														openGL
        //----------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Defines the TEXTURE_2D
        /// </summary>
        public const int TEXTURE_2D = 0x0DE1;

        /// <summary>
        /// Defines the BLEND
        /// </summary>
        public const int BLEND = 0x0BE2;

        /// <summary>
        /// Defines the MODELVIEW
        /// </summary>
        public const int MODELVIEW = 0x1700;

        /// <summary>
        /// Defines the PROJECTION
        /// </summary>
        public const int PROJECTION = 0x1701;

        /// <summary>
        /// Defines the COLOR_BUFFER_BIT
        /// </summary>
        public const int COLOR_BUFFER_BIT = 0x4000;

        /// <summary>
        /// Defines the QUADS
        /// </summary>
        public const int QUADS = 0x0007;

        /// <summary>
        /// Defines the TRIANGLES
        /// </summary>
        public const int TRIANGLES = 0x0004;

        /// <summary>
        /// Defines the LINES
        /// </summary>
        public const int LINES = 0x0001;

        /// <summary>
        /// Defines the TEXTURE_MIN_FILTER
        /// </summary>
        public const int TEXTURE_MIN_FILTER = 0x2801;

        /// <summary>
        /// Defines the TEXTURE_MAG_FILTER
        /// </summary>
        public const int TEXTURE_MAG_FILTER = 0x2800;

        /// <summary>
        /// Defines the LINEAR
        /// </summary>
        public const int LINEAR = 0x2601;

        /// <summary>
        /// Defines the TEXTURE_WRAP_S
        /// </summary>
        public const int TEXTURE_WRAP_S = 0x2802;

        /// <summary>
        /// Defines the TEXTURE_WRAP_T
        /// </summary>
        public const int TEXTURE_WRAP_T = 0x2803;

        /// <summary>
        /// Defines the CLAMP
        /// </summary>
        public const int CLAMP = 0x2900;

        /// <summary>
        /// Defines the GL_CLAMP_TO_EDGE_EXT
        /// </summary>
        public const int GL_CLAMP_TO_EDGE_EXT = 0x812F;

        /// <summary>
        /// Defines the RGBA
        /// </summary>
        public const int RGBA = 0x1908;

        /// <summary>
        /// Defines the BGRA
        /// </summary>
        public const int BGRA = 0x80E1;

        /// <summary>
        /// Defines the UNSIGNED_BYTE
        /// </summary>
        public const int UNSIGNED_BYTE = 0x1401;

        /// <summary>
        /// Defines the PERSPECTIVE_CORRECTION
        /// </summary>
        public const int PERSPECTIVE_CORRECTION = 0x0C50;

        /// <summary>
        /// Defines the FASTEST
        /// </summary>
        public const int FASTEST = 0x1101;

        /// <summary>
        /// Defines the NICEST
        /// </summary>
        public const int NICEST = 0x1102;

        /// <summary>
        /// Defines the NEAREST
        /// </summary>
        public const int NEAREST = 0x2600;

        /// <summary>
        /// Defines the POLYGON_SMOOTH
        /// </summary>
        public const int POLYGON_SMOOTH = 0x0B41;

        /// <summary>
        /// Defines the MULTISAMPLE
        /// </summary>
        public const int MULTISAMPLE = 0x809D;

        /// <summary>
        /// Defines the FLOAT
        /// </summary>
        public const int FLOAT = 0x1406;

        /// <summary>
        /// Defines the UNSIGNED_INT
        /// </summary>
        public const int UNSIGNED_INT = 0x1405;

        /// <summary>
        /// Defines the VERTEX_ARRAY
        /// </summary>
        public const int VERTEX_ARRAY = 0x8074;

        /// <summary>
        /// Defines the INT
        /// </summary>
        public const int INT = 0x1404;

        /// <summary>
        /// Defines the DOUBLE
        /// </summary>
        public const int DOUBLE = 0x140A;

        /// <summary>
        /// Defines the INDEX_ARRAY
        /// </summary>
        public const int INDEX_ARRAY = 0x8077;

        /// <summary>
        /// Defines the TEXTURE_COORD_ARRAY
        /// </summary>
        public const int TEXTURE_COORD_ARRAY = 0x8078;

        /// <summary>
        /// Defines the SCISSOR_TEST
        /// </summary>
        public const int SCISSOR_TEST = 0x0C11;

        /// <summary>
        /// Defines the MAX_TEXTURE_SIZE
        /// </summary>
        public const int MAX_TEXTURE_SIZE = 0x0D33;

        /// <summary>
        /// Defines the ZERO
        /// </summary>
        public const int ZERO = 0x0000;

        /// <summary>
        /// Defines the ONE
        /// </summary>
        public const int ONE = 0x0001;

        /// <summary>
        /// Defines the SRC_COLOR
        /// </summary>
        public const int SRC_COLOR = 0x0300;

        /// <summary>
        /// Defines the ONE_MINUS_SRC_COLOR
        /// </summary>
        public const int ONE_MINUS_SRC_COLOR = 0x0301;

        /// <summary>
        /// Defines the DST_COLOR
        /// </summary>
        public const int DST_COLOR = 0x0306;

        /// <summary>
        /// Defines the ONE_MINUS_DST_COLOR
        /// </summary>
        public const int ONE_MINUS_DST_COLOR = 0x0307;

        /// <summary>
        /// Defines the SRC_ALPHA
        /// </summary>
        public const int SRC_ALPHA = 0x0302;

        /// <summary>
        /// Defines the ONE_MINUS_SRC_ALPHA
        /// </summary>
        public const int ONE_MINUS_SRC_ALPHA = 0x0303;

        /// <summary>
        /// Defines the DST_ALPHA
        /// </summary>
        public const int DST_ALPHA = 0x0304;

        /// <summary>
        /// Defines the ONE_MINUS_DST_ALPHA
        /// </summary>
        public const int ONE_MINUS_DST_ALPHA = 0x0305;

        /// <summary>
        /// Defines the CONSTANT_COLOR
        /// </summary>
        public const int CONSTANT_COLOR = 0x8001;

        /// <summary>
        /// Defines the ONE_MINUS_CONSTANT_COLOR
        /// </summary>
        public const int ONE_MINUS_CONSTANT_COLOR = 0x8002;

        /// <summary>
        /// Defines the CONSTANT_ALPHA
        /// </summary>
        public const int CONSTANT_ALPHA = 0x8003;

        /// <summary>
        /// Defines the ONE_MINUS_CONSTANT_ALPHA
        /// </summary>
        public const int ONE_MINUS_CONSTANT_ALPHA = 0x8004;

        /// <summary>
        /// Defines the SRC_ALPHA_SATURATE
        /// </summary>
        public const int SRC_ALPHA_SATURATE = 0x0308;

        /// <summary>
        /// Defines the MIN
        /// </summary>
        public const int MIN = 0x8007;

        /// <summary>
        /// Defines the MAX
        /// </summary>
        public const int MAX = 0x8008;

        /// <summary>
        /// Defines the FUNC_ADD
        /// </summary>
        public const int FUNC_ADD = 0x8006;

        /// <summary>
        /// Defines the FUNC_SUBTRACT
        /// </summary>
        public const int FUNC_SUBTRACT = 0x800A;

        /// <summary>
        /// Defines the FUNC_REVERSE_SUBTRACT
        /// </summary>
        public const int FUNC_REVERSE_SUBTRACT = 0x800B;

        /// <summary>
        /// Defines the GL_REPEAT
        /// </summary>
        public const int GL_REPEAT = 0x2901;

        /// <summary>
        /// The Enable
        /// </summary>
        /// <param name="cap">The cap<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glEnable")]
        public static extern void Enable(int cap);

        /// <summary>
        /// The Disable
        /// </summary>
        /// <param name="cap">The cap<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glDisable")]
        public static extern void Disable(int cap);

        /// <summary>
        /// The BlendFunc
        /// </summary>
        /// <param name="sourceFactor">The sourceFactor<see cref="int"/></param>
        /// <param name="destFactor">The destFactor<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glBlendFunc")]
        public static extern void BlendFunc(int sourceFactor, int destFactor);

        /// <summary>
        /// The BlendEquation
        /// </summary>
        /// <param name="mode">The mode<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glBlendEquation")]
        public static extern void BlendEquation(int mode);

        /// <summary>
        /// The ClearColor
        /// </summary>
        /// <param name="r">The r<see cref="float"/></param>
        /// <param name="g">The g<see cref="float"/></param>
        /// <param name="b">The b<see cref="float"/></param>
        /// <param name="a">The a<see cref="float"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glClearColor")]
        public static extern void ClearColor(float r, float g, float b, float a);

        /// <summary>
        /// The MatrixMode
        /// </summary>
        /// <param name="mode">The mode<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glMatrixMode")]
        public static extern void MatrixMode(int mode);

        /// <summary>
        /// The LoadIdentity
        /// </summary>
        [DllImport("opengl32.dll", EntryPoint = "glLoadIdentity")]
        public static extern void LoadIdentity();

        /// <summary>
        /// The Ortho
        /// </summary>
        /// <param name="left">The left<see cref="double"/></param>
        /// <param name="right">The right<see cref="double"/></param>
        /// <param name="top">The top<see cref="double"/></param>
        /// <param name="bottom">The bottom<see cref="double"/></param>
        /// <param name="near">The near<see cref="double"/></param>
        /// <param name="far">The far<see cref="double"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glOrtho")]
        public static extern void Ortho(double left, double right, double top, double bottom, double near, double far);

        /// <summary>
        /// The Clear
        /// </summary>
        /// <param name="mask">The mask<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glClear")]
        public static extern void Clear(int mask);

        /// <summary>
        /// The Color4ub
        /// </summary>
        /// <param name="r">The r<see cref="byte"/></param>
        /// <param name="g">The g<see cref="byte"/></param>
        /// <param name="b">The b<see cref="byte"/></param>
        /// <param name="a">The a<see cref="byte"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glColor4ub")]
        public static extern void Color4ub(byte r, byte g, byte b, byte a);

        /// <summary>
        /// The PushMatrix
        /// </summary>
        [DllImport("opengl32.dll", EntryPoint = "glPushMatrix")]
        public static extern void PushMatrix();

        /// <summary>
        /// The MultMatrixf
        /// </summary>
        /// <param name="matrix">The matrix<see cref="float[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glMultMatrixf")]
        public static extern void MultMatrixf(float[] matrix);

        /// <summary>
        /// The PopMatrix
        /// </summary>
        [DllImport("opengl32.dll", EntryPoint = "glPopMatrix")]
        public static extern void PopMatrix();

        /// <summary>
        /// The Begin
        /// </summary>
        /// <param name="mode">The mode<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glBegin")]
        public static extern void Begin(int mode);

        /// <summary>
        /// The TexCoord2f
        /// </summary>
        /// <param name="u">The u<see cref="float"/></param>
        /// <param name="v">The v<see cref="float"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glTexCoord2f")]
        public static extern void TexCoord2f(float u, float v);

        /// <summary>
        /// The Vertex2f
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glVertex2f")]
        public static extern void Vertex2f(float x, float y);

        /// <summary>
        /// The Vertex3f
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <param name="z">The z<see cref="float"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glVertex3f")]
        public static extern void Vertex3f(float x, float y, float z);

        /// <summary>
        /// The End
        /// </summary>
        [DllImport("opengl32.dll", EntryPoint = "glEnd")]
        public static extern void End();

        /// <summary>
        /// The BindTexture
        /// </summary>
        /// <param name="target">The target<see cref="int"/></param>
        /// <param name="texture">The texture<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glBindTexture")]
        public static extern void BindTexture(int target, int texture);

        /// <summary>
        /// The GenTextures
        /// </summary>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="textures">The textures<see cref="int[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glGenTextures")]
        public static extern void GenTextures(int count, int[] textures);

        /// <summary>
        /// The TexParameteri
        /// </summary>
        /// <param name="target">The target<see cref="int"/></param>
        /// <param name="name">The name<see cref="int"/></param>
        /// <param name="value">The value<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glTexParameteri")]
        public static extern void TexParameteri(int target, int name, int value);

        /// <summary>
        /// The TexImage2D
        /// </summary>
        /// <param name="target">The target<see cref="int"/></param>
        /// <param name="level">The level<see cref="int"/></param>
        /// <param name="internalFormat">The internalFormat<see cref="int"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        /// <param name="border">The border<see cref="int"/></param>
        /// <param name="format">The format<see cref="int"/></param>
        /// <param name="type">The type<see cref="int"/></param>
        /// <param name="pixels">The pixels<see cref="IntPtr"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glTexImage2D")]
        public static extern void TexImage2D(int target, int level, int internalFormat, int width, int height,
                                             int border, int format, int type, IntPtr pixels);

        /// <summary>
        /// The DeleteTextures
        /// </summary>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="textures">The textures<see cref="int[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glDeleteTextures")]
        public static extern void DeleteTextures(int count, int[] textures);

        /// <summary>
        /// The Flush
        /// </summary>
        [DllImport("opengl32.dll", EntryPoint = "glFinish")]
        public static extern void Flush();

        /// <summary>
        /// The Finish
        /// </summary>
        [DllImport("opengl32.dll", EntryPoint = "glFlush")]
        public static extern void Finish();

        /// <summary>
        /// The Hint
        /// </summary>
        /// <param name="target">The target<see cref="int"/></param>
        /// <param name="mode">The mode<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glHint")]
        public static extern void Hint(int target, int mode);

        /// <summary>
        /// The Viewport
        /// </summary>
        /// <param name="x">The x<see cref="int"/></param>
        /// <param name="y">The y<see cref="int"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glViewport")]
        public static extern void Viewport(int x, int y, int width, int height);

        /// <summary>
        /// The Scissor
        /// </summary>
        /// <param name="x">The x<see cref="int"/></param>
        /// <param name="y">The y<see cref="int"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glScissor")]
        public static extern void Scissor(int x, int y, int width, int height);

        /// <summary>
        /// The VertexPointer
        /// </summary>
        /// <param name="size">The size<see cref="int"/></param>
        /// <param name="type">The type<see cref="int"/></param>
        /// <param name="stride">The stride<see cref="int"/></param>
        /// <param name="pointer">The pointer<see cref="float[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glVertexPointer")]
        public static extern void VertexPointer(int size, int type, int stride, float[] pointer);

        /// <summary>
        /// The TexCoordPointer
        /// </summary>
        /// <param name="size">The size<see cref="int"/></param>
        /// <param name="type">The type<see cref="int"/></param>
        /// <param name="stride">The stride<see cref="int"/></param>
        /// <param name="pointer">The pointer<see cref="float[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glTexCoordPointer")]
        public static extern void TexCoordPointer(int size, int type, int stride, float[] pointer);

        /// <summary>
        /// The DrawElements
        /// </summary>
        /// <param name="mode">The mode<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="type">The type<see cref="int"/></param>
        /// <param name="indices">The indices<see cref="int[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glDrawElements")]
        public static extern void DrawElements(int mode, int count, int type, int[] indices);

        /// <summary>
        /// The EnableClientState
        /// </summary>
        /// <param name="array">The array<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glEnableClientState")]
        public static extern void EnableClientState(int array);

        /// <summary>
        /// The ArrayElement
        /// </summary>
        /// <param name="element">The element<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glArrayElement")]
        public static extern void ArrayElement(int element);

        /// <summary>
        /// The DrawArrays
        /// </summary>
        /// <param name="mode">The mode<see cref="int"/></param>
        /// <param name="offset">The offset<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glDrawArrays")]
        public static extern void DrawArrays(int mode, int offset, int count);

        /// <summary>
        /// The DisableClientState
        /// </summary>
        /// <param name="state">The state<see cref="int"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glDisableClientState")]
        public static extern void DisableClientState(int state);

        /// <summary>
        /// The GetError
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        [DllImport("opengl32.dll", EntryPoint = "glGetError")]
        public static extern int GetError();

        /// <summary>
        /// The GetIntegerv
        /// </summary>
        /// <param name="name">The name<see cref="int"/></param>
        /// <param name="param">The param<see cref="int[]"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glGetIntegerv")]
        public static extern void GetIntegerv(int name, int[] param);

        /// <summary>
        /// The LineWidth
        /// </summary>
        /// <param name="width">The width<see cref="float"/></param>
        [DllImport("opengl32.dll", EntryPoint = "glLineWidth")]
        public static extern void LineWidth(float width);

        //----------------------------------------------------------------------------------------------------------------------
        //														GLFW
        //----------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Defines the GLFW_OPENED
        /// </summary>
        public const int GLFW_OPENED = 0x00020001;

        /// <summary>
        /// Defines the GLFW_WINDOWED
        /// </summary>
        public const int GLFW_WINDOWED = 0x00010001;

        /// <summary>
        /// Defines the GLFW_FULLSCREEN
        /// </summary>
        public const int GLFW_FULLSCREEN = 0x00010002;

        /// <summary>
        /// Defines the GLFW_ACTIVE
        /// </summary>
        public const int GLFW_ACTIVE = 0x00020001;

        /// <summary>
        /// Defines the GLFW_FSAA_SAMPLES
        /// </summary>
        public const int GLFW_FSAA_SAMPLES = 0x0002100E;

        /// <summary>
        /// Defines the GLFW_MOUSE_CURSOR
        /// </summary>
        public const int GLFW_MOUSE_CURSOR = 0x00030001;

        /// <summary>
        /// The GLFWWindowSizeCallback
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void GLFWWindowSizeCallback(int width, int height);

        /// <summary>
        /// The GLFWKeyCallback
        /// </summary>
        /// <param name="key">The key<see cref="int"/></param>
        /// <param name="action">The action<see cref="int"/></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void GLFWKeyCallback(int key, int action);

        /// <summary>
        /// The GLFWMouseButtonCallback
        /// </summary>
        /// <param name="button">The button<see cref="int"/></param>
        /// <param name="action">The action<see cref="int"/></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void GLFWMouseButtonCallback(int button, int action);

        /// <summary>
        /// The glfwSetTime
        /// </summary>
        /// <param name="time">The time<see cref="double"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSetTime(double time);

        /// <summary>
        /// The glfwGetTime
        /// </summary>
        /// <returns>The <see cref="double"/></returns>
        [DllImport("lib/glfw.dll")]
        public static extern double glfwGetTime();

        /// <summary>
        /// The glfwPollEvents
        /// </summary>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwPollEvents();

        /// <summary>
        /// The glfwGetWindowParam
        /// </summary>
        /// <param name="param">The param<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllImport("lib/glfw.dll")]
        public static extern int glfwGetWindowParam(int param);

        /// <summary>
        /// The glfwInit
        /// </summary>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwInit();

        /// <summary>
        /// The glfwOpenWindow
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        /// <param name="r">The r<see cref="int"/></param>
        /// <param name="g">The g<see cref="int"/></param>
        /// <param name="b">The b<see cref="int"/></param>
        /// <param name="a">The a<see cref="int"/></param>
        /// <param name="depth">The depth<see cref="int"/></param>
        /// <param name="stencil">The stencil<see cref="int"/></param>
        /// <param name="mode">The mode<see cref="int"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwOpenWindow(int width, int height, int r, int g, int b, int a, int depth, int stencil, int mode);

        /// <summary>
        /// The glfwSetWindowTitle
        /// </summary>
        /// <param name="title">The title<see cref="string"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSetWindowTitle(string title);

        /// <summary>
        /// The glfwSwapInterval
        /// </summary>
        /// <param name="mode">The mode<see cref="bool"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSwapInterval(bool mode);

        /// <summary>
        /// The glfwSetWindowSizeCallback
        /// </summary>
        /// <param name="callback">The callback<see cref="GLFWWindowSizeCallback"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSetWindowSizeCallback(GLFWWindowSizeCallback callback);

        /// <summary>
        /// The glfwCloseWindow
        /// </summary>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwCloseWindow();

        /// <summary>
        /// The glfwTerminate
        /// </summary>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwTerminate();

        /// <summary>
        /// The glfwSwapBuffers
        /// </summary>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSwapBuffers();

        /// <summary>
        /// The glfwGetKey
        /// </summary>
        /// <param name="key">The key<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        [DllImport("lib/glfw.dll")]
        public static extern bool glfwGetKey(int key);

        /// <summary>
        /// The glfwSetKeyCallback
        /// </summary>
        /// <param name="callback">The callback<see cref="GLFWKeyCallback"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSetKeyCallback(GLFWKeyCallback callback);

        /// <summary>
        /// The glfwOpenWindowHint
        /// </summary>
        /// <param name="name">The name<see cref="int"/></param>
        /// <param name="value">The value<see cref="int"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwOpenWindowHint(int name, int value);

        /// <summary>
        /// The glfwGetMousePos
        /// </summary>
        /// <param name="x">The x<see cref="int"/></param>
        /// <param name="y">The y<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        [DllImport("lib/glfw.dll")]
        public static extern bool glfwGetMousePos(out int x, out int y);

        /// <summary>
        /// The glfwSetMouseButtonCallback
        /// </summary>
        /// <param name="callback">The callback<see cref="GLFWMouseButtonCallback"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwSetMouseButtonCallback(GLFWMouseButtonCallback callback);

        /// <summary>
        /// The glfwEnable
        /// </summary>
        /// <param name="property">The property<see cref="int"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwEnable(int property);

        /// <summary>
        /// The glfwDisable
        /// </summary>
        /// <param name="property">The property<see cref="int"/></param>
        [DllImport("lib/glfw.dll")]
        public static extern void glfwDisable(int property);
    }
}
