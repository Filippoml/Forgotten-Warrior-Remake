namespace GXPEngine.Classes
{
    using System;

    /// <summary>
    /// Defines the <see cref="ElapsedGameTime" />
    /// </summary>
    internal class ElapsedGameTime
    {
        /// <summary>
        /// Defines the TotalSeconds
        /// </summary>
        internal int TotalSeconds;

        /// <summary>
        /// Defines the NowSecondsTime
        /// </summary>
        internal TimeSpan NowSecondsTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedGameTime"/> class.
        /// </summary>
        public ElapsedGameTime()
        {
            NowSecondsTime = DateTime.Now.TimeOfDay;
        }

        /// <summary>
        /// The getTotalSeconds
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public int getTotalSeconds()
        {
            TotalSeconds = (DateTime.Now.TimeOfDay - NowSecondsTime).Milliseconds;
            NowSecondsTime = DateTime.Now.TimeOfDay;
            return TotalSeconds;
        }
    }
}
