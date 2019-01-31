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
        Tile[] Tiles;

        bool prova = false;
        private string _filename;
        private GameObject[] arrayObjects;
        Swordman swordman;
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
            arrayObjects = new GameObject[10];
        }

        public Player GetPlayer() => _player;


        public void generateLevel(int screenIndex)
        {
            int _num_objects = 0;
   

            for (int i = 0; i < Tiles.Length; i++)
            {
                if (Tiles[i] != null)
                {
                    RemoveChild(Tiles[i]);
                    RemoveChild(_player);
                    RemoveChild(arrayObjects[0]);
                }
                
            }

            

            int screenIndex2 = screenIndex + 1;
           
            Map level = MapParser.ReadMap(_filename);

   
            if (!prova)
            {
            

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
                    int test = 0;

                    for (int j = 0; j < 4000; j++)
                    {
                        test++;


                        /*
                        if(screenIndex2 == 1)
                        {
                            if (test > 26 * screenIndex2)
                            {
                                tiles[j] = 0;
                            }
                        }
                        else
                        {
                            if (test < 26 * screenIndex2 - 1 || test > 26 * screenIndex2)
                            {
                                tiles[j] = 0;
                            }
                        }
                        */
                        if (test < (27) * (screenIndex2 - 1) || test > 28 * screenIndex2)
                        {
                            tiles[j] = 0;
                        }


                        if (test == _currentLayer.Width)
                        {
                            test = 0;

                        }



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
            }

            if (_player == null)
            {
                _player = new Player(100, 1000);
            }
                for (int i = 0; i < level.ObjectGroups[0].Objects.Length; i++)
                {
                    TiledObject _object = level.ObjectGroups[0].Objects[i];

                    switch (_object.Type)
                    {
                        case "Swordman":

                        if (swordman == null)
                        {
                            swordman = new Swordman(_object.X, _object.Y);
                        
                        arrayObjects[0] = swordman;
                
                        }
                        if ((int)Math.Floor(_object.X / 810) == screenIndex)
                            {
                        
                                Console.WriteLine("added to child");
                            AddChild(swordman);
                        }
                            else
                            {
                                Console.WriteLine(_object.X + "-" + _object.Y);
                            }

                            break;
                        /*
                        case "Fire":
                            Fire fire = new Fire(_object.X, _object.Y);
                            AddChild(fire);
                            break;
                        case "Wizard":
                            Wizard wizard = new Wizard(_object.X, _object.Y);
                            AddChild(wizard);
                            break;
                        case "Coin":
                            Coin _coin = new Coin(_object.X, _object.Y);
                            AddChild(_coin);
                            break;
                            */
                    }

                }

            

            AddChild(_player);
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
            if ((int)Math.Floor((_player.x + _player.width) / 810) < _scrollIndex)
            {
                int differenceScroll = Math.Abs((int)Math.Floor((_player.x + _player.width) / 810) - _scrollIndex);
                _scrollIndex = (int)Math.Floor(_player.x / 810);
                generateLevel(_scrollIndex);
                Translate(810 * differenceScroll, 0);

            }
            else if ((int)Math.Floor(_player.x / 810) > _scrollIndex)
            {
                int differenceScroll = Math.Abs((int)Math.Floor((_player.x + _player.width) / 810) - _scrollIndex);
                _scrollIndex = (int)Math.Floor(_player.x / 810);
                generateLevel(_scrollIndex);
                Translate(-810 * differenceScroll, 0);
            }
            
        }
    }

}
