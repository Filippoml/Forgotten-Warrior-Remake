namespace GXPEngine
{
    using System;

    /// <summary>
    /// The Utils class contains a number of useful functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Defines the random
        /// </summary>
        static private Random random = new Random();

        /// <summary>
        /// Gets the frameRate
        /// Returns the current frame rate in frames per second.
        /// Deprecated use game.fps instead!
        /// </summary>
        public static int frameRate
        {
            get
            {
                return Game.main.currentFps;
            }
        }

        /// <summary>
        /// Gets a random value between the specified min (inclusive) and max (exclusive).
		/// If you want to receive an integer value, use two integers as parameters to this function.
        /// </summary>
        /// <param name="min">The min<see cref="int"/></param>
        /// <param name="max">The max<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int Random(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// The Random
        /// </summary>
        /// <param name="min">The min<see cref="float"/></param>
        /// <param name="max">The max<see cref="float"/></param>
        /// <returns>The <see cref="float"/></returns>
        public static float Random(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Shows output on the console window.
		/// Basically, a shortcut for Console.WriteLine() that allows for multiple parameters.
        /// </summary>
        /// <param name="list">The list<see cref="object[]"/></param>
        public static void print(params object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null) Console.Write(list[i].ToString() + " "); else Console.Write("null ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Returns 'true' if the two specified rectangles overlap or 'false' otherwise.
        /// </summary>
        /// <param name="x1">The x1<see cref="float"/></param>
        /// <param name="y1">The y1<see cref="float"/></param>
        /// <param name="width1">The width1<see cref="float"/></param>
        /// <param name="height1">The height1<see cref="float"/></param>
        /// <param name="x2">The x2<see cref="float"/></param>
        /// <param name="y2">The y2<see cref="float"/></param>
        /// <param name="width2">The width2<see cref="float"/></param>
        /// <param name="height2">The height2<see cref="float"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool RectsOverlap(float x1, float y1, float width1, float height1,
                                        float x2, float y2, float width2, float height2)
        {
            if (x1 > x2 + width2) return false;
            if (y1 > y2 + height2) return false;
            if (x2 > x1 + width1) return false;
            if (y2 > y1 + height1) return false;
            return true;
        }
    }
}
