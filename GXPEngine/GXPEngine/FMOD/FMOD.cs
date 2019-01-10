namespace GXPEngine
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="FMOD" />
    /// </summary>
    public class FMOD
    {
        /// <summary>
        /// Defines the FMOD_DEFAULT
        /// </summary>
        public const int FMOD_DEFAULT = 0x00000000;

        /// <summary>
        /// Defines the FMOD_LOOP_OFF
        /// </summary>
        public const int FMOD_LOOP_OFF = 0x00000001;

        /// <summary>
        /// Defines the FMOD_LOOP_NORMAL
        /// </summary>
        public const int FMOD_LOOP_NORMAL = 0x00000002;

        /// <summary>
        /// Defines the FMOD_LOOP_BIDI
        /// </summary>
        public const int FMOD_LOOP_BIDI = 0x00000004;

        /// <summary>
        /// Defines the FMOD_2D
        /// </summary>
        public const int FMOD_2D = 0x00000008;

        /// <summary>
        /// Defines the FMOD_3D
        /// </summary>
        public const int FMOD_3D = 0x00000010;

        /// <summary>
        /// Defines the FMOD_HARDWARE
        /// </summary>
        public const int FMOD_HARDWARE = 0x00000020;

        /// <summary>
        /// Defines the FMOD_SOFTWARE
        /// </summary>
        public const int FMOD_SOFTWARE = 0x00000040;

        /// <summary>
        /// Defines the FMOD_CREATESTREAM
        /// </summary>
        public const int FMOD_CREATESTREAM = 0x00000080;

        /// <summary>
        /// Defines the FMOD_CREATESAMPLE
        /// </summary>
        public const int FMOD_CREATESAMPLE = 0x00000100;

        /// <summary>
        /// Defines the FMOD_CREATECOMPRESSEDSAMPLE
        /// </summary>
        public const int FMOD_CREATECOMPRESSEDSAMPLE = 0x00000200;

        /// <summary>
        /// Defines the FMOD_OPENUSER
        /// </summary>
        public const int FMOD_OPENUSER = 0x00000400;

        /// <summary>
        /// Defines the FMOD_OPENMEMORY
        /// </summary>
        public const int FMOD_OPENMEMORY = 0x00000800;

        /// <summary>
        /// Defines the FMOD_OPENMEMORY_POINT
        /// </summary>
        public const int FMOD_OPENMEMORY_POINT = 0x10000000;

        /// <summary>
        /// Defines the FMOD_OPENRAW
        /// </summary>
        public const int FMOD_OPENRAW = 0x00001000;

        /// <summary>
        /// Defines the FMOD_OPENONLY
        /// </summary>
        public const int FMOD_OPENONLY = 0x00002000;

        /// <summary>
        /// Defines the FMOD_ACCURATETIME
        /// </summary>
        public const int FMOD_ACCURATETIME = 0x00004000;

        /// <summary>
        /// Defines the FMOD_MPEGSEARCH
        /// </summary>
        public const int FMOD_MPEGSEARCH = 0x00008000;

        /// <summary>
        /// Defines the FMOD_NONBLOCKING
        /// </summary>
        public const int FMOD_NONBLOCKING = 0x00010000;

        /// <summary>
        /// Defines the FMOD_UNIQUE
        /// </summary>
        public const int FMOD_UNIQUE = 0x00020000;

        /// <summary>
        /// Defines the FMOD_3D_HEADRELATIVE
        /// </summary>
        public const int FMOD_3D_HEADRELATIVE = 0x00040000;

        /// <summary>
        /// Defines the FMOD_3D_WORLDRELATIVE
        /// </summary>
        public const int FMOD_3D_WORLDRELATIVE = 0x00080000;

        /// <summary>
        /// Defines the FMOD_3D_INVERSEROLLOFF
        /// </summary>
        public const int FMOD_3D_INVERSEROLLOFF = 0x00100000;

        /// <summary>
        /// Defines the FMOD_3D_LINEARROLLOFF
        /// </summary>
        public const int FMOD_3D_LINEARROLLOFF = 0x00200000;

        /// <summary>
        /// Defines the FMOD_3D_LINEARSQUAREROLLOFF
        /// </summary>
        public const int FMOD_3D_LINEARSQUAREROLLOFF = 0x00400000;

        /// <summary>
        /// Defines the FMOD_3D_CUSTOMROLLOFF
        /// </summary>
        public const int FMOD_3D_CUSTOMROLLOFF = 0x04000000;

        /// <summary>
        /// Defines the FMOD_3D_IGNOREGEOMETRY
        /// </summary>
        public const int FMOD_3D_IGNOREGEOMETRY = 0x40000000;

        /// <summary>
        /// Defines the FMOD_UNICODE
        /// </summary>
        public const int FMOD_UNICODE = 0x01000000;

        /// <summary>
        /// Defines the FMOD_IGNORETAGS
        /// </summary>
        public const int FMOD_IGNORETAGS = 0x02000000;

        /// <summary>
        /// Defines the FMOD_LOWMEM
        /// </summary>
        public const int FMOD_LOWMEM = 0x08000000;

        /// <summary>
        /// Defines the FMOD_LOADSECONDARYRAM
        /// </summary>
        public const int FMOD_LOADSECONDARYRAM = 0x20000000;

        /// <summary>
        /// Defines the FMOD_VIRTUAL_PLAYFROMSTART
        /// </summary>
        public const uint FMOD_VIRTUAL_PLAYFROMSTART = 0x80000000;

        /// <summary>
        /// Defines the FMOD_CHANNEL_FREE
        /// </summary>
        public const int FMOD_CHANNEL_FREE = -1;

        /// <summary>
        /// Defines the FMOD_CHANNEL_REUSE
        /// </summary>
        public const int FMOD_CHANNEL_REUSE = -2;

        // System
        /// <summary>
        /// The System_Create
        /// </summary>
        /// <param name="system">The system<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_System_Create")]
        public static extern void System_Create(out int system);

        /// <summary>
        /// The System_Init
        /// </summary>
        /// <param name="system">The system<see cref="int"/></param>
        /// <param name="maxChannels">The maxChannels<see cref="int"/></param>
        /// <param name="flags">The flags<see cref="uint"/></param>
        /// <param name="extraDriverData">The extraDriverData<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_System_Init")]
        public static extern void System_Init(int system, int maxChannels, uint flags, int extraDriverData);

        /// <summary>
        /// The System_CreateSound
        /// </summary>
        /// <param name="system">The system<see cref="int"/></param>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="mode">The mode<see cref="uint"/></param>
        /// <param name="uk">The uk<see cref="int"/></param>
        /// <param name="sound">The sound<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_System_CreateSound")]
        public static extern void System_CreateSound(int system, string filename, uint mode, int uk, out int sound);

        /// <summary>
        /// The System_CreateStream
        /// </summary>
        /// <param name="system">The system<see cref="int"/></param>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="mode">The mode<see cref="uint"/></param>
        /// <param name="uk">The uk<see cref="int"/></param>
        /// <param name="sound">The sound<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_System_CreateStream")]
        public static extern void System_CreateStream(int system, string filename, uint mode, int uk, out int sound);

        /// <summary>
        /// The System_PlaySound
        /// </summary>
        /// <param name="system">The system<see cref="int"/></param>
        /// <param name="channelpref">The channelpref<see cref="int"/></param>
        /// <param name="sound">The sound<see cref="int"/></param>
        /// <param name="paused">The paused<see cref="bool"/></param>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_System_PlaySound")]
        public static extern int System_PlaySound(int system, int channelpref, int sound, bool paused, ref int channel);

        /// <summary>
        /// The System_Update
        /// </summary>
        /// <param name="system">The system<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_System_Update")]
        public static extern void System_Update(int system);

        // Channel
        // Frequency
        /// <summary>
        /// The Channel_GetFrequency
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="frequency">The frequency<see cref="float"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_GetFrequency")]
        public static extern void Channel_GetFrequency(int channel, out float frequency);

        /// <summary>
        /// The Channel_SetFrequency
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="frequency">The frequency<see cref="float"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_SetFrequency")]
        public static extern void Channel_SetFrequency(int channel, float frequency);

        // Stop
        /// <summary>
        /// The Channel_Stop
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_Stop")]
        public static extern void Channel_Stop(int channel);

        // Mute
        /// <summary>
        /// The Channel_GetMute
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="mute">The mute<see cref="bool"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_GetMute")]
        public static extern void Channel_GetMute(int channel, out bool mute);

        /// <summary>
        /// The Channel_SetMute
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="mute">The mute<see cref="bool"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_SetMute")]
        public static extern void Channel_SetMute(int channel, bool mute);

        // Pan
        /// <summary>
        /// The Channel_GetPan
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="pan">The pan<see cref="float"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_GetPan")]
        public static extern void Channel_GetPan(int channel, out float pan);

        /// <summary>
        /// The Channel_SetPan
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="pan">The pan<see cref="float"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_SetPan")]
        public static extern void Channel_SetPan(int channel, float pan);

        // Paused
        /// <summary>
        /// The Channel_GetPaused
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="paused">The paused<see cref="bool"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_GetPaused")]
        public static extern void Channel_GetPaused(int channel, out bool paused);

        /// <summary>
        /// The Channel_SetPaused
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="paused">The paused<see cref="bool"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_SetPaused")]
        public static extern void Channel_SetPaused(int channel, bool paused);

        // Playing
        /// <summary>
        /// The Channel_IsPlaying
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="playing">The playing<see cref="bool"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_IsPlaying")]
        public static extern void Channel_IsPlaying(int channel, out bool playing);

        // Spectrum // can also be done in System for total output, not tested yet
        /// <summary>
        /// The Channel_GetSpectrum
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="spectrumarray">The spectrumarray<see cref="float []"/></param>
        /// <param name="numvalues">The numvalues<see cref="int"/></param>
        /// <param name="channeloffset">The channeloffset<see cref="int"/></param>
        /// <param name="windowtype">The windowtype<see cref="int"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_GetSpectrum")]
        public static extern void Channel_GetSpectrum(int channel, float[] spectrumarray, int numvalues, int channeloffset, int windowtype);

        // Volume
        /// <summary>
        /// The Channel_GetVolume
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="volume">The volume<see cref="float"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_GetVolume")]
        public static extern void Channel_GetVolume(int channel, out float volume);

        /// <summary>
        /// The Channel_SetVolume
        /// </summary>
        /// <param name="channel">The channel<see cref="int"/></param>
        /// <param name="volume">The volume<see cref="float"/></param>
        [DllImport("lib/fmodex.dll", EntryPoint = "FMOD_Channel_SetVolume")]
        public static extern void Channel_SetVolume(int channel, float volume);
    }
}
