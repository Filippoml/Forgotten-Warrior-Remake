// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace TiledSharp
{
    using System.Xml.Linq;

    /// <summary>
    /// Defines the <see cref="TmxImageLayer" />
    /// </summary>
    public class TmxImageLayer : ITmxElement
    {
        /// <summary>
        /// Gets the Name
        /// </summary>
        public string Name { get; private set; }

        // TODO: Legacy (Tiled Java) attributes (x, y, width, height)
        /// <summary>
        /// Gets the Width
        /// </summary>
        public int? Width { get; private set; }

        /// <summary>
        /// Gets the Height
        /// </summary>
        public int? Height { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Visible
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        /// Gets the Opacity
        /// </summary>
        public double Opacity { get; private set; }

        /// <summary>
        /// Gets the OffsetX
        /// </summary>
        public double OffsetX { get; private set; }

        /// <summary>
        /// Gets the OffsetY
        /// </summary>
        public double OffsetY { get; private set; }

        /// <summary>
        /// Gets the Image
        /// </summary>
        public TmxImage Image { get; private set; }

        /// <summary>
        /// Gets the Properties
        /// </summary>
        public PropertyDict Properties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TmxImageLayer"/> class.
        /// </summary>
        /// <param name="xImageLayer">The xImageLayer<see cref="XElement"/></param>
        /// <param name="tmxDir">The tmxDir<see cref="string"/></param>
        public TmxImageLayer(XElement xImageLayer, string tmxDir = "")
        {
            Name = (string)xImageLayer.Attribute("name");

            Width = (int?)xImageLayer.Attribute("width");
            Height = (int?)xImageLayer.Attribute("height");
            Visible = (bool?)xImageLayer.Attribute("visible") ?? true;
            Opacity = (double?)xImageLayer.Attribute("opacity") ?? 1.0;
            OffsetX = (double?)xImageLayer.Attribute("offsetx") ?? 0.0;
            OffsetY = (double?)xImageLayer.Attribute("offsety") ?? 0.0;

            Image = new TmxImage(xImageLayer.Element("image"), tmxDir);

            Properties = new PropertyDict(xImageLayer.Element("properties"));
        }
    }
}
