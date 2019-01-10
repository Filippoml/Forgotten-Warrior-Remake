namespace GXPEngine
{
    using System;
    using GXPEngine.OpenGL;

    /// <summary>
    /// Defines different BlendModes, only two present now, but you can add your own.
    /// </summary>
    public class BlendMode
    {
        /// <summary>
        /// Defines the NORMAL
        /// </summary>
        public static readonly BlendMode NORMAL = new BlendMode(
            "Normal", () => { GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA); }
        );

        /// <summary>
        /// Defines the MULTIPLY
        /// </summary>
        public static readonly BlendMode MULTIPLY = new BlendMode(
            "Multiply", () => { GL.BlendFunc(GL.DST_COLOR, GL.ZERO); }
        );

        /// <summary>
        /// The Action
        /// </summary>
        public delegate void Action();

        /// <summary>
        /// This should point to an anonymous function updating the blendfunc
        /// </summary>
        public readonly Action enable;

        /// <summary>
        /// A label for this blendmode
        /// </summary>
        public readonly string label;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendMode"/> class.
        /// </summary>
        /// <param name="pLabel">The pLabel<see cref="string"/></param>
        /// <param name="pEnable">The pEnable<see cref="Action"/></param>
        public BlendMode(string pLabel, Action pEnable)
        {
            if (pEnable == null)
            {
                throw new Exception("Enabled action cannot be null");
            }
            else
            {
                enable = pEnable;
            }

            label = pLabel;
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return label;
        }
    }
}
