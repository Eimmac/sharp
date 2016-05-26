using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using zlib;

namespace TheGame.TiledMax
{
    [Serializable]
    [XmlRoot("map")]
    public class TmxMap
    {
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }
        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }
        [XmlAttribute("backgroundcolor")]
        public string BackgroundColor { get; set; }
        [XmlElement("tileset")]
        public List<TileSet> TileSets { get; set; }
        [XmlElement("layer")]
        public List<Layer> Layers { get; set; }

        [XmlElement("objectgroup")]
        public List<ObjectGroup> ObjectGroups { get; set; }

        public static TmxMap LoadTmxMap(Game game, string mapName)
        {
            TmxMap map;
            //Deserialize tmx
            var mapFilePath = game.Content.RootDirectory + "/Maps/" + mapName;
            if (!mapFilePath.EndsWith(".tmx"))
                mapFilePath += ".tmx";

            using (var sr = new StreamReader(mapFilePath))
            {
                var serializer = new XmlSerializer(typeof(TmxMap));
                map = (TmxMap)serializer.Deserialize(sr);
            }

            foreach (var tileSet in map.TileSets)
            {
                tileSet.Image.Texture = game.Content.Load<Texture2D>(tileSet.Image.Source.Replace("../", ""));
            }

            return map;
        }

        public Tile GetTile(int tileId)
        {
            var tileSet = TileSets.Single(ts => ts.FirstGID - 1 < tileId && ts.TileCount + ts.FirstGID - 1 >= tileId);
            tileId -= tileSet.FirstGID;
            var rectangle = new Rectangle(tileSet.TileWidth * (tileId % tileSet.Columns), tileSet.TileHeight * (tileId / tileSet.Columns), tileSet.TileWidth, tileSet.TileHeight);
            return new Tile
            {
                Rectangle = rectangle,
                Texture = tileSet.Image.Texture
            };
        }

        private Color? _backgroundColor;
        public Color GetBackgroundColor()
        {
            if (_backgroundColor.HasValue) return _backgroundColor.Value;

            _backgroundColor = BackgroundColor.ToColor();
            return _backgroundColor.Value;
        }
    }

    public class ObjectGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("object")]
        public List<TmxObject> Objects { get; set; }
    }

    public class TmxObject
    {
        [XmlAttribute("gid")]
        public int GID { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("x")]
        public float X { get; set; }
        [XmlAttribute("y")]
        public float Y { get; set; }
        [XmlAttribute("width")]
        public float Width { get; set; }
        [XmlAttribute("height")]
        public float Height { get; set; }
        [XmlElement("polyline")]
        public List<PolyLine> PolyLines { get; set; }
        [XmlArray("properties"), XmlArrayItem(typeof(Property), ElementName = "property")]
        public List<Property> Properties { get; set; }
    }

    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }

        public float GetFloatValue()
        {
            return float.Parse(Value);
        }
    }

    public class PolyLine
    {
        [XmlAttribute("points")]
        public string Points { get; set; }

        private List<Vector2> _points;

        [XmlIgnore]
        public List<Vector2> VectorPoints
        {
            get
            {
                if (_points == null)
                {
                    _points = new List<Vector2>();
                    if (!string.IsNullOrWhiteSpace(Points))
                    {
                        var strpts = Points.Split(' ');
                        foreach (var spt in strpts)
                        {
                            var sxy = spt.Split(',');

                            _points.Add(new Vector2(float.Parse(sxy[0], new CultureInfo("en-US")), float.Parse(sxy[1], new CultureInfo("en-US"))));
                        }
                    }
                }
                return _points;
            }
        }
    }

    public class Layer
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlElement("data")]
        public Data Data { get; set; }

        private int[,] _tiles;
        public int[,] Tiles
        {
            get
            {
                if (_tiles != null) return _tiles;

                var data = Data.Decompress();
                _tiles = new int[Width, Height];
                using (var br = new BinaryReader(new MemoryStream(data)))
                {
                    for (var y = 0; y < Height; y++)
                    {
                        for (var x = 0; x < Width; x++)
                        {
                            var tileId = br.ReadInt32();
                            _tiles[x, y] = tileId;
                        }
                    }
                }
                return _tiles;
            }
        }
    }

    public class Data
    {
        [XmlAttribute("encoding")]
        public string Encoding { get; set; }
        [XmlAttribute("compression")]
        public string Compression { get; set; }
        [XmlText]
        public string CompressedData { get; set; }

        public byte[] Decompress()
        {
            if (Encoding != "base64" && Compression != "zlib")
                throw new NotSupportedException("Encoding suported only with base64 and zlib");

            var compressedData = Convert.FromBase64String(CompressedData);
            using (var msOutput = new MemoryStream())
            {
                using (var msInput = new MemoryStream(compressedData))
                {
                    using (var zlib = new ZOutputStream(msOutput))
                    {
                        var buffer = new byte[2000];
                        int len;
                        while ((len = msInput.Read(buffer, 0, 2000)) > 0)
                        {
                            zlib.Write(buffer, 0, len);
                        }
                        zlib.Flush();
                    }
                }
                return msOutput.ToArray();
            }
        }
    }

    public class Tile
    {
        public Texture2D Texture { get; set; }
        public Rectangle Rectangle { get; set; }
    }

    public class TileSet
    {
        [XmlAttribute("firstgid")]
        public int FirstGID { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }
        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }
        private int _tileCount = -1;
        [XmlAttribute("tilecount")]
        public int TileCount
        {
            get
            {
                if (_tileCount == -1)
                {
                    _tileCount = Image.Width / TileWidth * Image.Height / TileHeight;
                }
                return _tileCount;
            }
            set
            {
                _tileCount = value;
            }
        }
        private int _columns = -1;
        [XmlAttribute("columns")]
        public int Columns
        {
            get
            {
                if (_columns == -1)
                {
                    _columns = Image.Width / TileWidth;
                }
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }
        [XmlElement("image")]
        public Image Image { get; set; }
    }

    public class Image
    {
        [XmlAttribute("source")]
        public string Source { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlIgnore]
        public Texture2D Texture { get; set; }
    }
}
