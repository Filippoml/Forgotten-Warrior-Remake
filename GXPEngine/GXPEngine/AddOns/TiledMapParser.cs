namespace TiledMapParser
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Call the method MapParser.ReadMap, with as argument a Tiled file exported as xml (file extension: .tmx),
	/// to get an object of type Map.
	/// This object, together with its nested objects, contains most of the information contained in the Tiled file.
	/// 
	/// The nesting of objects mimics the structure of the Tiled file exactly. 
	/// (For instance, a Map can contain multiple (tile) Layers, ObjectgroupLayers, ImageLayers, which 
	/// all can have a PropertyList, etc.)
	/// 
	/// You should extend this class yourself if you want to read more information from the Tiled file 
	/// (such as tile rotations, geometry objects, ...). See
	///   http://docs.mapeditor.org/en/stable/reference/tmx-map-format/
	/// for details.
    /// </summary>
    public class MapParser
    {
        /// <summary>
        /// Defines the serial
        /// </summary>
        internal static XmlSerializer serial = new XmlSerializer(typeof(Map));

        /// <summary>
        /// The ReadMap
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <returns>The <see cref="Map"/></returns>
        public static Map ReadMap(string filename)
        {
            TextReader reader = new StreamReader(filename);
            Map myMap = serial.Deserialize(reader) as Map;
            reader.Close();

            return myMap;
        }

        /// <summary>
        /// The WriteMap
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        /// <param name="map">The map<see cref="Map"/></param>
        public static void WriteMap(string filename, Map map)
        {
            TextWriter writer = new StreamWriter(filename);
            serial.Serialize(writer, map);
            writer.Close();
        }
    }

    /// <summary>
    /// Defines the <see cref="Map" />
    /// </summary>
    [XmlRootAttribute("map")]
    public class Map
    {
        /// <summary>
        /// Defines the Width
        /// </summary>
        [XmlAttribute("width")]
        public int Width;

        /// <summary>
        /// Defines the Height
        /// </summary>
        [XmlAttribute("height")]
        public int Height;

        /// <summary>
        /// Defines the Version
        /// </summary>
        [XmlAttribute("version")]
        public string Version;

        /// <summary>
        /// Defines the Orientation
        /// </summary>
        [XmlAttribute("orientation")]
        public string Orientation;

        /// <summary>
        /// Defines the RenderOrder
        /// </summary>
        [XmlAttribute("renderorder")]
        public string RenderOrder;

        /// <summary>
        /// Defines the TileWidth
        /// </summary>
        [XmlAttribute("tilewidth")]
        public int TileWidth;

        /// <summary>
        /// Defines the TileHeight
        /// </summary>
        [XmlAttribute("tileheight")]
        public int TileHeight;

        /// <summary>
        /// Defines the NextObjectId
        /// </summary>
        [XmlAttribute("nextobjectid")]
        public int NextObjectId;

        /// <summary>
        /// Defines the TileSets
        /// </summary>
        [XmlElement("tileset")]
        public TileSet[] TileSets;

        /// <summary>
        /// Defines the Layers
        /// </summary>
        [XmlElement("layer")]
        public Layer[] Layers;

        /// <summary>
        /// Defines the ObjectGroups
        /// </summary>
        [XmlElement("objectgroup")]
        public ObjectGroup[] ObjectGroups;

        /// <summary>
        /// Defines the ImageLayers
        /// </summary>
        [XmlElement("imagelayer")]
        public ImageLayer[] ImageLayers;

        /// <summary>
        /// Defines the InnerXML
        /// </summary>
        [XmlText]
        public string InnerXML;// This should be empty

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            string output = "Map of width " + Width + " and height " + Height + ".\n";

            output += "TILE LAYERS:\n";
            foreach (Layer l in Layers)
                output += l.ToString();

            output += "IMAGE LAYERS:\n";
            foreach (ImageLayer l in ImageLayers)
                output += l.ToString();

            output += "TILE SETS:\n";
            foreach (TileSet t in TileSets)
                output += t.ToString();

            output += "OBJECT GROUPS:\n";
            foreach (ObjectGroup g in ObjectGroups)
                output += g.ToString();

            return output;
        }

        /// <summary>
        /// A helper function that returns the tile set that belongs to the tile ID read from the layer data:
        /// </summary>
        /// <param name="tileID">The tileID<see cref="int"/></param>
        /// <returns>The <see cref="TileSet"/></returns>
        public TileSet GetTileSet(int tileID)
        {
            if (tileID < 0)
                return null;
            int index = 0;
            while (TileSets[index].FirstGId + TileSets[index].TileCount <= tileID)
            {
                index++;
                if (index >= TileSets.Length)
                    return null;
            }
            return TileSets[index];
        }
    }

    /// <summary>
    /// Defines the <see cref="TileSet" />
    /// </summary>
    [XmlRootAttribute("tileset")]
    public class TileSet
    {
        /// <summary>
        /// Defines the TileWidth
        /// </summary>
        [XmlAttribute("tilewidth")]
        public int TileWidth;

        /// <summary>
        /// Defines the TileHeight
        /// </summary>
        [XmlAttribute("tileheight")]
        public int TileHeight;

        /// <summary>
        /// Defines the TileCount
        /// </summary>
        [XmlAttribute("tilecount")]
        public int TileCount;

        /// <summary>
        /// Defines the Columns
        /// </summary>
        [XmlAttribute("columns")]
        public int Columns;

        /// <summary>
        /// Gets the Rows
        /// </summary>
        public int Rows
        {
            get
            {
                if (TileCount % Columns == 0)
                    return TileCount / Columns;
                else
                    return (TileCount / Columns) + 1;
            }
        }

        /// <summary>
        /// This is the number of the first tile. Usually 1 (so 0 means empty/no tile).
		//// When multiple tilesets are used, this is the total number of previous tiles + 1.
        /// </summary>
        [XmlAttribute("firstgid")]
        public int FirstGId;

        /// <summary>
        /// Defines the Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// Defines the Image
        /// </summary>
        [XmlElement("image")]
        public Image Image;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return "Tile set: Name: " + Name + " Image: " + Image + " Tile dimensions: " + TileWidth + "x" + TileHeight + " Grid dimensions: " + Columns + "x" + (int)Math.Ceiling(1f * TileCount / Columns) + "\n";
        }
    }

    /// <summary>
    /// Defines the <see cref="PropertyContainer" />
    /// </summary>
    public class PropertyContainer
    {
        /// <summary>
        /// Defines the propertyList
        /// </summary>
        [XmlElement("properties")]
        public PropertyList propertyList;

        /// <summary>
        /// The HasProperty
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="type">The type<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool HasProperty(string key, string type)
        {
            if (propertyList == null)
                return false;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == type)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// The GetStringProperty
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetStringProperty(string key, string defaultValue = "")
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key)
                    return p.Value;
            }
            return defaultValue;
        }

        /// <summary>
        /// The GetFloatProperty
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="float"/></param>
        /// <returns>The <see cref="float"/></returns>
        public float GetFloatProperty(string key, float defaultValue = 1)
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "float")
                    return float.Parse(p.Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            }
            return defaultValue;
        }

        /// <summary>
        /// The GetIntProperty
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int GetIntProperty(string key, int defaultValue = 1)
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "int")
                    return int.Parse(p.Value);
            }
            return defaultValue;
        }

        /// <summary>
        /// The GetBoolProperty
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="bool"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool GetBoolProperty(string key, bool defaultValue = false)
        {
            if (propertyList == null)
                return defaultValue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "bool")
                    return bool.Parse(p.Value);
            }
            return defaultValue;
        }

        /// <summary>
        /// The GetColorProperty
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="defaultvalue">The defaultvalue<see cref="uint"/></param>
        /// <returns>The <see cref="uint"/></returns>
        public uint GetColorProperty(string key, uint defaultvalue = 0xffffffff)
        {
            if (propertyList == null)
                return defaultvalue;
            foreach (Property p in propertyList.properties)
            {
                if (p.Name == key && p.Type == "color")
                {
                    return TiledUtils.GetColor(p.Value);
                }
            }
            return defaultvalue;
        }
    }

    /// <summary>
    /// Defines the <see cref="ImageLayer" />
    /// </summary>
    [XmlRootAttribute("imagelayer")]
    public class ImageLayer : PropertyContainer
    {
        /// <summary>
        /// Defines the Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// Defines the Image
        /// </summary>
        [XmlElement("image")]
        public Image Image;

        /// <summary>
        /// Defines the offsetX
        /// </summary>
        [XmlAttribute("offsetx")]
        public int offsetX = 0;

        /// <summary>
        /// Defines the offsetY
        /// </summary>
        [XmlAttribute("offsety")]
        public int offsetY = 0;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return "Image layer: " + Name + " Image: " + Image + "\n";
        }
    }

    /// <summary>
    /// Defines the <see cref="Image" />
    /// </summary>
    [XmlRootAttribute("image")]
    public class Image
    {
        /// <summary>
        /// Defines the Width
        /// </summary>
        [XmlAttribute("width")]     // width in pixels
        public int Width;

        /// <summary>
        /// Defines the Height
        /// </summary>
        [XmlAttribute("height")]    // height in pixels
        public int Height;

        /// <summary>
        /// Defines the FileName
        /// </summary>
        [XmlAttribute("source")]    // AnimSprite file name
        public string FileName;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return FileName + " (dim: " + Width + "x" + Height + ")";
        }
    }

    /// <summary>
    /// Defines the <see cref="Layer" />
    /// </summary>
    [XmlRootAttribute("layer")]
    public class Layer : PropertyContainer
    {
        /// <summary>
        /// Defines the Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// Defines the Width
        /// </summary>
        [XmlAttribute("width")]
        public int Width;

        /// <summary>
        /// Defines the Height
        /// </summary>
        [XmlAttribute("height")]
        public int Height;

        /// <summary>
        /// Defines the Data
        /// </summary>
        [XmlElement("data")]
        public Data Data;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            string output = " Layer name: " + Name;
            output += "Properties:\n" + propertyList.ToString();
            output += "Data:" + Data.ToString();
            return output;
        }
    }

    /// <summary>
    /// Defines the <see cref="Data" />
    /// </summary>
    [XmlRootAttribute("data")]
    public class Data
    {
        /// <summary>
        /// Defines the Encoding
        /// </summary>
        [XmlAttribute("encoding")]
        public string Encoding;

        /// <summary>
        /// Defines the innerXML
        /// </summary>
        [XmlText]
        public string innerXML;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return innerXML;
        }
    }

    /// <summary>
    /// Defines the <see cref="PropertyList" />
    /// </summary>
    [XmlRootAttribute("properties")]
    public class PropertyList
    {
        /// <summary>
        /// Defines the properties
        /// </summary>
        [XmlElement("property")]
        public Property[] properties;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            string output = "";
            foreach (Property p in properties)
                output += p.ToString();
            return output;
        }
    }

    /// <summary>
    /// Defines the <see cref="Property" />
    /// </summary>
    [XmlRootAttribute("property")]
    public class Property
    {
        /// <summary>
        /// Defines the Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// Defines the Type
        /// </summary>
        [XmlAttribute("type")]
        public string Type = "string";

        /// <summary>
        /// Defines the Value
        /// </summary>
        [XmlAttribute("value")]
        public string Value;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return "Property: Name: " + Name + " Type: " + Type + " Value: " + Value + "\n";
        }
    }

    /// <summary>
    /// Defines the <see cref="ObjectGroup" />
    /// </summary>
    [XmlRootAttribute("objectgroup")]
    public class ObjectGroup : PropertyContainer
    {
        /// <summary>
        /// Defines the Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// Defines the Objects
        /// </summary>
        [XmlElement("object")]
        public TiledObject[] Objects;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            string output = "Object group: Name: " + Name + " Objects:\n";
            foreach (TiledObject obj in Objects)
                output += obj.ToString();

            return output;
        }
    }

    /// <summary>
    /// Defines the <see cref="Text" />
    /// </summary>
    [XmlRootAttribute("text")]
    public class Text
    {
        /// <summary>
        /// Defines the font
        /// </summary>
        [XmlAttribute("fontfamily")]
        public string font;

        /// <summary>
        /// Defines the wrap
        /// </summary>
        [XmlAttribute("wrap")]
        public int wrap = 0;

        /// <summary>
        /// Defines the bold
        /// </summary>
        [XmlAttribute("bold")]
        public int bold = 0;

        /// <summary>
        /// Defines the italic
        /// </summary>
        [XmlAttribute("italic")]
        public int italic = 0;

        /// <summary>
        /// Defines the fontSize
        /// </summary>
        [XmlAttribute("pixelsize")]
        public int fontSize = 16;

        /// <summary>
        /// Defines the text
        /// </summary>
        [XmlText]
        public string text;

        /// <summary>
        /// Defines the horizontalAlign
        /// </summary>
        [XmlAttribute("halign")]
        public string horizontalAlign = "left";

        /// <summary>
        /// Defines the verticalAlign
        /// </summary>
        [XmlAttribute("valign")]
        public string verticalAlign = "top";

        /// <summary>
        /// Defines the color
        /// </summary>
        [XmlAttribute("color")]
        public string color = "#FF000000";// Tiled default

        /// <summary>
        /// Gets the Color
        /// </summary>
        public uint Color
        {
            get
            {
                return TiledUtils.GetColor(color);
            }
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return text;
        }
    }

    /// <summary>
    /// Defines the <see cref="TiledObject" />
    /// </summary>
    [XmlRootAttribute("object")]
    public class TiledObject : PropertyContainer
    {
        /// <summary>
        /// Defines the ID
        /// </summary>
        [XmlAttribute("id")]
        public int ID;

        /// <summary>
        /// Defines the GID
        /// </summary>
        [XmlAttribute("gid")]
        public int GID = -1;

        /// <summary>
        /// Defines the Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// Defines the Type
        /// </summary>
        [XmlAttribute("type")]
        public string Type;

        /// <summary>
        /// Defines the Width
        /// </summary>
        [XmlAttribute("width")]     // width in pixels
        public float Width;

        /// <summary>
        /// Defines the Height
        /// </summary>
        [XmlAttribute("height")]    // height in pixels
        public float Height;

        /// <summary>
        /// Defines the X
        /// </summary>
        [XmlAttribute("x")]
        public float X;

        /// <summary>
        /// Defines the Y
        /// </summary>
        [XmlAttribute("y")]
        public float Y;

        /// <summary>
        /// Defines the textField
        /// </summary>
        [XmlElement("text")]
        public Text textField;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        override public string ToString()
        {
            return "Object: " + Name + " ID: " + ID + " Type: " + Type + " coordinates: (" + X + "," + Y + ") dimensions: (" + Width + "," + Height + ")\n";
        }
    }

    /// <summary>
    /// Defines the <see cref="TiledUtils" />
    /// </summary>
    public class TiledUtils
    {
        /// <summary>
        /// This translates a Tiled color string to a uint that can be used as a GXPEngine Sprite color.
        /// </summary>
        /// <param name="htmlColor">The htmlColor<see cref="string"/></param>
        /// <returns>The <see cref="uint"/></returns>
        public static uint GetColor(string htmlColor)
        {
            if (htmlColor.Length == 9)
            {
                return (uint)(
                    (Convert.ToInt32(htmlColor.Substring(3, 2), 16) << 24) +        // R
                    (Convert.ToInt32(htmlColor.Substring(5, 2), 16) << 16) +        // G
                    (Convert.ToInt32(htmlColor.Substring(7, 2), 16) << 8) +     // B
                    (Convert.ToInt32(htmlColor.Substring(1, 2), 16)));          // Alpha
            }
            else if (htmlColor.Length == 7)
            {
                return (uint)(
                    (Convert.ToInt32(htmlColor.Substring(1, 2), 16) << 24) +        // R
                    (Convert.ToInt32(htmlColor.Substring(3, 2), 16) << 16) +        // G
                    (Convert.ToInt32(htmlColor.Substring(5, 2), 16) << 8) +     // B
                    0xFF);                                                          // Alpha
            }
            else
            {
                throw new Exception("Cannot recognize color string: " + htmlColor);
            }
        }
    }
}
