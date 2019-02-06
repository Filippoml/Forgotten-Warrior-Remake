using System;
using System.Collections.Generic;
using System.Drawing;
using TiledMapParser;

namespace GXPEngine.Classes
{
    public class Level : GameObject
    {
        private Player _player;

        private int _scrollIndex, _num_objects;

        private HUD _hud;

        private readonly List<Tile> _tiles;

        private string _filename;

        private Shop _shop;

        public Level()
        {
            _tiles = new List<Tile>();
        }

        public void generateLevel(int index)
        {
            _filename = "Data/level" + index + ".tmx";            

            //Background creation
            Bitmap Bmp = new Bitmap(800, 600);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(Color.FromArgb(135, 206, 235));
            gfx.FillRectangle(brush, 0, 0, 800, 600);

            Sprite _background = new Sprite(Bmp);
            AddChild(_background);
            _background.width = 2000;
            _background.height = 2000;

            Map level = MapParser.ReadMap(_filename);
            
            for (int i = 0; i < level.Layers.Length; i++)
            {
                Layer _currentLayer = level.Layers[i];

                Data leveldata = _currentLayer.Data;

                String levelData = _currentLayer.Data.innerXML.ToString();

                levelData = levelData.Replace("\n", "");

                int[] _tilesArray = Array.ConvertAll(levelData.Split(','), int.Parse);

                int _columns = level.Layers[i].Width;
                int _rows = level.Layers[i].Height;

                int tileset_number;

                if (i == 3)
                {
                    tileset_number = 1;
                }
                else
                {
                    tileset_number = 0;
                }

                for (int j = 0; j < _tilesArray.Length; j++)
                {
                    if (_tilesArray[j] != 0)
                    {
                        int x = j % _columns;
                        int y = j / _columns;

                        _tiles.Add(new Tile(_tilesArray[j], level.TileSets[tileset_number].Image.FileName, level.TileSets[tileset_number].Columns, level.TileSets[tileset_number].Rows));
                        _tiles[_num_objects].currentFrame = _tilesArray[j] - level.TileSets[tileset_number].FirstGId;
                        _tiles[_num_objects].x = (x * 30) + level.Layers[i].offsetx;
                        _tiles[_num_objects].y = (y * 32) + level.Layers[i].offsety - level.TileSets[tileset_number].TileHeight;
                        AddChild(_tiles[_num_objects]);

                        _num_objects++;
                    }
                }
            }

            _player = new Player(100, 400);
            for (int i = 0; i < level.ObjectGroups[0].Objects.Length; i++)
            {
                TiledObject _object = level.ObjectGroups[0].Objects[i];
                
                switch (_object.Type)
                {
                    case "Coin":
                        Coin _coin = new Coin(_object.X , _object.Y);
                        AddChild(_coin);
                        break;
                    case "Chest":
                        ChestBox _chest = new ChestBox(_object.X, _object.Y);
                        AddChild(_chest);
                        break;
                    case "Trigger":
                        Trigger _trigger = new Trigger(_object.X, _object.Y);
                        AddChild(_trigger);
                        break;
                }
            }

            AddChild(_player);

            for (int i = 0; i < level.ObjectGroups[1].Objects.Length; i++)
            {
                TiledObject _object = level.ObjectGroups[1].Objects[i];

                switch (_object.Type)
                {
                    case "Swordman":
                        Swordman swordman = new Swordman(_object.X, _object.Y);
                        AddChild(swordman);
                        break;
                    case "Fire":
                        Fire fire = new Fire(_object.X, _object.Y);
                        AddChild(fire);
                        break;
                    case "Wizard":
                        Wizard wizard = new Wizard(_object.X, _object.Y);
                        AddChild(wizard);
                        break;
                }
            }

            _hud = new HUD();
            _hud.SetXY(300, 570);
            game.AddChild(_hud);

            _shop = new Shop(175, 50);
            _shop.visible = false;
            game.AddChild(_shop);

            _player.LoadHUD();
        }

        protected override void OnDestroy()
        {
            _shop.Destroy();
            _hud.Destroy();
        }

        public HUD GetHud()
        {
            return _hud;
        }

        void Update()
        {
            if (_player != null)
            {
                //Scrolling
                if ((int)Math.Floor((_player.x + _player.width) / 800) < _scrollIndex)
                {
                    _scrollIndex = (int)Math.Floor(_player.x / 800);
                    Translate(800, 0);
                }
                else if ((int)Math.Floor(_player.x / 800) > _scrollIndex)
                {
                    _scrollIndex = (int)Math.Floor(_player.x / 800);
                    Translate(-800, 0);
                }
            }
        }

        public Shop GetShop()
        {
           return _shop;
        }

        public Player GetPlayer()
        {
            return _player;
        }
    }
}