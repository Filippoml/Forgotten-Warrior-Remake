// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace TiledSharp
{
    using System;
    using System.Xml.Linq;

    /// <summary>
    /// Defines the <see cref="TmxGroup" />
    /// </summary>
    public class TmxGroup : ITmxElement
    {
        /// <summary>
        /// Gets the Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the Opacity
        /// </summary>
        public double Opacity { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Visible
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        /// Gets the OffsetX
        /// </summary>
        public double? OffsetX { get; private set; }

        /// <summary>
        /// Gets the OffsetY
        /// </summary>
        public double? OffsetY { get; private set; }

        /// <summary>
        /// Gets the Layers
        /// </summary>
        public TmxList<TmxLayer> Layers { get; private set; }

        /// <summary>
        /// Gets the ObjectGroups
        /// </summary>
        public TmxList<TmxObjectGroup> ObjectGroups { get; private set; }

        /// <summary>
        /// Gets the ImageLayers
        /// </summary>
        public TmxList<TmxImageLayer> ImageLayers { get; private set; }

        /// <summary>
        /// Gets the Groups
        /// </summary>
        public TmxList<TmxGroup> Groups { get; private set; }

        /// <summary>
        /// Gets the Properties
        /// </summary>
        public PropertyDict Properties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TmxGroup"/> class.
        /// </summary>
        /// <param name="xGroup">The xGroup<see cref="XElement"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        /// <param name="tmxDirectory">The tmxDirectory<see cref="string"/></param>
        public TmxGroup(XElement xGroup, int width, int height, string tmxDirectory)
        {
            Name = (string)xGroup.Attribute("name") ?? String.Empty;
            Opacity = (double?)xGroup.Attribute("opacity") ?? 1.0;
            Visible = (bool?)xGroup.Attribute("visible") ?? true;
            OffsetX = (double?)xGroup.Attribute("offsetx") ?? 0.0;
            OffsetY = (double?)xGroup.Attribute("offsety") ?? 0.0;

            Properties = new PropertyDict(xGroup.Element("properties"));

            Layers = new TmxList<TmxLayer>();
            foreach (var e in xGroup.Elements("layer"))
                Layers.Add(new TmxLayer(e, width, height));

            ObjectGroups = new TmxList<TmxObjectGroup>();
            foreach (var e in xGroup.Elements("objectgroup"))
                ObjectGroups.Add(new TmxObjectGroup(e));

            ImageLayers = new TmxList<TmxImageLayer>();
            foreach (var e in xGroup.Elements("imagelayer"))
                ImageLayers.Add(new TmxImageLayer(e, tmxDirectory));

            Groups = new TmxList<TmxGroup>();
            foreach (var e in xGroup.Elements("group"))
                Groups.Add(new TmxGroup(e, width, height, tmxDirectory));
        }
    }
}
