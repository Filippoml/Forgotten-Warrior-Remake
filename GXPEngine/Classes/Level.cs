using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.GXPEngine;
using TiledMapParser;

namespace GXPEngine.Classes
{
    public class Level : GameObject
    {
        private Player _player;
        private int _scrollIndex = 0;

        private Tile[] Tiles;

        private int _num_objects;

        private string _filename;

        public Level(int index)
        {
            _filename = "Data/";
            Tiles = new Tile[2000];

            switch (index)
            {
                case 1:
                    _filename += "level.tmx";
                    break;
            }

        }

        public Player GetPlayer() => _player;


        public void generateLevel()
        {
            Map level = MapParser.ReadMap(_filename);


            for (int i = 0; i < level.Layers.Length; i++)
            {
                Layer _currentLayer = level.Layers[i];

                Data leveldata = _currentLayer.Data;


                String levelData = _currentLayer.Data.innerXML.ToString();


                levelData = levelData.Replace("\n", "");

                int[] tiles = Array.ConvertAll(levelData.Split(','), int.Parse);

                int columns = level.Layers[i].Width;
                int rows = level.Layers[i].Height;

                int tileset_number;

                if (i == 3)
                {
                    tileset_number = 1;
                }
                else
                {
                    tileset_number = 0;
                }


                for (int j = 0; j < tiles.Length; j++)
                {

                    if (tiles[j] != 0)
                    {

                        int x = j % columns;
                        int y = j / columns;



                        

                        Tiles[_num_objects] = new Tile(tiles[j], level.TileSets[tileset_number].Image.FileName, level.TileSets[tileset_number].Columns, level.TileSets[tileset_number].Rows);
                        Tiles[_num_objects].currentFrame = tiles[j] - level.TileSets[tileset_number].FirstGId;
                        


                        Tiles[_num_objects].x = (x * 30) + level.Layers[i].offsetx;
                        Tiles[_num_objects].y = (y * 32) + level.Layers[i].offsety - level.TileSets[tileset_number].TileHeight;
                        AddChild(Tiles[_num_objects]);
                        _num_objects++;
                    }

                }
            }

            _player = new Player(100, 1000);
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
                        Chest _chest = new Chest(_object.X, _object.Y);
                        AddChild(_chest);
                        break;
                }
                
            }
            AddChild(_player);
            //TODO create two object groups
            for (int i = 0; i < level.ObjectGroups[0].Objects.Length; i++)
            {
                TiledObject _object = level.ObjectGroups[0].Objects[i];

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
        }

        void Update()
        {
            if (Input.GetKeyUp(Key.F)){
                Translate(-800, 0);
            }
            if (Input.GetKeyUp(Key.R))
            {
                Translate(800, 0);
            }
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

}
