namespace GXPEngine.Core
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using GXPEngine.OpenGL;

    /// <summary>
    /// Defines the <see cref="Texture2D" />
    /// </summary>
    public class Texture2D
    {
        /// <summary>
        /// Defines the LoadCache
        /// </summary>
        private static Hashtable LoadCache = new Hashtable();

        /// <summary>
        /// Defines the lastBound
        /// </summary>
        private static Texture2D lastBound = null;

        /// <summary>
        /// Defines the UNDEFINED_GLTEXTURE
        /// </summary>
        internal const int UNDEFINED_GLTEXTURE = 0;

        /// <summary>
        /// Defines the _bitmap
        /// </summary>
        private Bitmap _bitmap;

        /// <summary>
        /// Defines the _glTexture
        /// </summary>
        private int[] _glTexture;

        /// <summary>
        /// Defines the _filename
        /// </summary>
        private string _filename = "";

        /// <summary>
        /// Defines the count
        /// </summary>
        private int count = 0;

        /// <summary>
        /// Defines the stayInCache
        /// </summary>
        private bool stayInCache = false;

        //------------------------------------------------------------------------------------------------------------------------
        //														Texture2D()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        public Texture2D(int width, int height)
        {
            if (width == 0) if (height == 0) return;
            SetBitmap(new Bitmap(width, height));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        public Texture2D(string filename)
        {
            Load(filename);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap<see cref="Bitmap"/></param>
        public Texture2D(Bitmap bitmap)
        {
            SetBitmap(bitmap);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetInstance()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetInstance
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="keepInCache">The keepInCache<see cref="bool"/></param>
        /// <returns>The <see cref="Texture2D"/></returns>
        public static Texture2D GetInstance(string filename, bool keepInCache = false)
        {
            Texture2D tex2d = LoadCache[filename] as Texture2D;
            if (tex2d == null)
            {
                tex2d = new Texture2D(filename);
                LoadCache[filename] = tex2d;
            }
            tex2d.stayInCache |= keepInCache; // setting it once to true keeps it in cache
            tex2d.count++;
            return tex2d;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RemoveInstance()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The RemoveInstance
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        public static void RemoveInstance(string filename)
        {
            if (LoadCache.ContainsKey(filename))
            {
                Texture2D tex2D = LoadCache[filename] as Texture2D;
                tex2D.count--;
                if (tex2D.count == 0 && !tex2D.stayInCache) LoadCache.Remove(filename);
            }
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        public void Dispose()
        {
            if (_filename != "")
            {
                Texture2D.RemoveInstance(_filename);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														bitmap
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the bitmap
        /// </summary>
        public Bitmap bitmap
        {
            get { return _bitmap; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														filename
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the filename
        /// </summary>
        public string filename
        {
            get { return _filename; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														width
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the width
        /// </summary>
        public int width
        {
            get { return _bitmap.Width; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														height
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the height
        /// </summary>
        public int height
        {
            get { return _bitmap.Height; }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Bind()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Bind
        /// </summary>
        public void Bind()
        {
            if (lastBound == this) return;
            lastBound = this;
            GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Unbind()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Unbind
        /// </summary>
        public void Unbind()
        {
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Load()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        private void Load(string filename)
        {
            _filename = filename;
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(filename);
            }
            catch
            {
                throw new Exception("Image " + filename + " cannot be found.");
            }
            SetBitmap(bitmap);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetBitmap()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The SetBitmap
        /// </summary>
        /// <param name="bitmap">The bitmap<see cref="Bitmap"/></param>
        private void SetBitmap(Bitmap bitmap)
        {
            _bitmap = bitmap;
            CreateGLTexture();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														CreateGLTexture()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The CreateGLTexture
        /// </summary>
        private void CreateGLTexture()
        {
            if (_glTexture != null)
                if (_glTexture.Length > 0)
                    if (_glTexture[0] != UNDEFINED_GLTEXTURE)
                        destroyGLTexture();

            _glTexture = new int[1];
            if (_bitmap == null)
                _bitmap = new Bitmap(64, 64);

            GL.GenTextures(1, _glTexture);

            GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
            if (Game.main.PixelArt)
            {
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
            }
            else
            {
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
            }
            GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, GL.GL_CLAMP_TO_EDGE_EXT);
            GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, GL.GL_CLAMP_TO_EDGE_EXT);

            UpdateGLTexture();
            GL.BindTexture(GL.TEXTURE_2D, 0);
            lastBound = null;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														UpdateGLTexture()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The UpdateGLTexture
        /// </summary>
        public void UpdateGLTexture()
        {
            BitmapData data = _bitmap.LockBits(new System.Drawing.Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                                                 ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
            GL.TexImage2D(GL.TEXTURE_2D, 0, GL.RGBA, _bitmap.Width, _bitmap.Height, 0,
                          GL.BGRA, GL.UNSIGNED_BYTE, data.Scan0);

            _bitmap.UnlockBits(data);
            lastBound = null;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														destroyGLTexture()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The destroyGLTexture
        /// </summary>
        private void destroyGLTexture()
        {
            GL.DeleteTextures(1, _glTexture);
            _glTexture[0] = UNDEFINED_GLTEXTURE;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Clone()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Clone
        /// </summary>
        /// <param name="deepCopy">The deepCopy<see cref="bool"/></param>
        /// <returns>The <see cref="Texture2D"/></returns>
        public Texture2D Clone(bool deepCopy = false)
        {
            Bitmap bitmap;
            if (deepCopy)
            {
                bitmap = _bitmap.Clone() as Bitmap;
            }
            else
            {
                bitmap = _bitmap;
            }
            Texture2D newTexture = new Texture2D(0, 0);
            newTexture.SetBitmap(bitmap);
            return newTexture;
        }

        /// <summary>
        /// Sets a value indicating whether wrap
        /// </summary>
        public bool wrap
        {
            set
            {
                GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, value ? GL.GL_REPEAT : GL.GL_CLAMP_TO_EDGE_EXT);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, value ? GL.GL_REPEAT : GL.GL_CLAMP_TO_EDGE_EXT);
                GL.BindTexture(GL.TEXTURE_2D, 0);
                lastBound = null;
            }
        }

        /// <summary>
        /// The GetDiagnostics
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public static string GetDiagnostics()
        {
            string output = "";
            output += "Number of textures in cache: " + LoadCache.Keys.Count + '\n';
            return output;
        }
    }
}
