namespace GXPEngine
{
    /// <summary>
    /// This class represents a sound channel on the soundcard.
    /// </summary>
    public class SoundChannel
    {
        /// <summary>
        /// Defines the _id
        /// </summary>
        private int _id = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundChannel"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        public SoundChannel(int id)
        {
            _id = id;
        }

        /// <summary>
        /// Gets or sets the channel frequency.
        /// </summary>
        public float Frequency
        {
            get
            {
                float frequency;
                FMOD.Channel_GetFrequency(_id, out frequency);
                return frequency;
            }
            set
            {
                FMOD.Channel_SetFrequency(_id, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GXPEngine.SoundChannel"/> is mute.
        /// </summary>
        public bool Mute
        {
            get
            {
                bool mute;
                FMOD.Channel_GetMute(_id, out mute);
                return mute;
            }
            set
            {
                FMOD.Channel_SetMute(_id, value);
            }
        }

        /// <summary>
        /// Gets or sets the pan. Value should be in range -1..0..1, for left..center..right
        /// </summary>
        public float Pan
        {
            get
            {
                float pan;
                FMOD.Channel_GetPan(_id, out pan);
                return pan;
            }
            set
            {
                FMOD.Channel_SetPan(_id, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GXPEngine.Channel"/> is paused.
        /// </summary>
        public bool IsPaused
        {
            get
            {
                bool paused;
                FMOD.Channel_GetPaused(_id, out paused);
                return paused;
            }
            set
            {
                FMOD.Channel_SetPaused(_id, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GXPEngine.Channel"/> is playing. (readonly)
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                bool playing;
                FMOD.Channel_IsPlaying(_id, out playing);
                return playing;
            }
        }

        /// <summary>
        /// Stop the channel.
        /// </summary>
        public void Stop()
        {
            FMOD.Channel_Stop(_id);
            _id = 0;
        }

        /// <summary>
        /// Gets or sets the volume. Should be in range 0...1
        /// </summary>
        public float Volume
        {
            get
            {
                float volume;
                FMOD.Channel_GetVolume(_id, out volume);
                return volume;
            }
            set
            {
                FMOD.Channel_SetVolume(_id, value);
            }
        }
    }
}
