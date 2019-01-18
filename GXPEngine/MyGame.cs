using System;                                   // System contains a lot of default C# libraries 
using System.Drawing;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Classes;
using GXPEngine.Core;
using GXPEngine.GXPEngine;
using TiledMapParser;

/// <summary>
/// Defines the <see cref="MyGame" />
/// </summary>
public class MyGame : Game
{
    /// <summary>
    /// Defines the sprite
    /// </summary>
    internal AnimationSprite sprite;

    // TODO: no public static! (certainly no static)

    /// <summary>
    /// Defines the objects
    /// </summary>
    private static AnimationSprite[] objects;

    private Tile[] Tiles; 

    /// <summary>
    /// Defines the id_tiles
    /// </summary>
    private static int[] id_tiles;

    public Player Player
    {
        get
        {
            return player;
        }
    }
    Player player;


    /// <summary>
    /// Defines the num_objects
    /// </summary>
    internal int num_objects = 0;

    /// <summary>
    /// Gets or sets the Objects
    /// </summary>
    public static AnimationSprite[] Objects
    {
        get { return objects; }
        set { objects = value; }
    }

    /// <summary>
    /// Gets or sets the Id_Tiles
    /// </summary>
    public static int[] Id_Tiles
    {
        get { return id_tiles; }
        set { id_tiles = value; }
    }

    private int _scrollIndex = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyGame"/> class.
    /// </summary>
    public MyGame() : base(800, 600, false, false, pPixelArt: true)		// Create a window that's 800x600 and NOT fullscreen
    {
        
        objects = new AnimationSprite[1000];
        id_tiles = new int[1000];
        Tiles = new Tile[1000];
        
        //Background creation
        Bitmap Bmp = new Bitmap(width, height);
        using (Graphics gfx = Graphics.FromImage(Bmp))
        using (SolidBrush brush = new SolidBrush(Color.FromArgb(135, 206, 235)))
        {
            gfx.FillRectangle(brush, 0, 0, width, height);
        }

        Sprite _background = new Sprite(Bmp);
        AddChild(_background);
        _background.width = 5000;
        _background.height = 5000;


        Map level = MapParser.ReadMap("Data/level.tmx");

        for (int i = 0; i<level.Layers.Length; i++)
        {
            Layer _currentLayer = level.Layers[i];

            Data leveldata = _currentLayer.Data;
  

         


            String levelData = _currentLayer.Data.innerXML.ToString();
            levelData = levelData.Replace("\n", "");

            int[] tiles = Array.ConvertAll(levelData.Split(','), int.Parse);

            int columns = level.Layers[0].Width;
            int rows = level.Layers[0].Height;
            
            for (int j = 0; j < tiles.Length; j++)
            {

                if (tiles[j] != 0)
                {

                    int x = j % columns;
                    int y = j / columns;
                    //sprite = new AnimationSprite(texture.bitmap, level.TileSets[0].Columns, level.TileSets[0].Rows);
                    //sprite.currentFrame = tiles[j] - 1;

                    Tiles[num_objects] = new Tile(tiles[j], level.TileSets[0].Image.FileName, level.TileSets[0].Columns, level.TileSets[0].Rows);
                    Tiles[num_objects].currentFrame = tiles[j] - 1;

                    objects[num_objects] = sprite;
                    id_tiles[num_objects] = tiles[j];


                    
                    Tiles[num_objects].x = x * 30;
                    Tiles[num_objects].y = y * 30;
                    AddChild(Tiles[num_objects]);
                    num_objects++;
                }

            }
        }


        player = new Player();

        Swordman swordman = new Swordman(1000, 1000);

        AddChild(swordman);
        AddChild(player);


        game.Translate(0, -600);


    }

    /// <summary>
    /// The Update
    /// </summary>
     void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                if (player.HitTest(objects[i]))
                {
                    i = objects.Length;
                    //Console.WriteLine("test");
                }
            }
        }

        if ((int)Math.Floor(Player.x / 800) < _scrollIndex)
        {
            _scrollIndex = (int)Math.Floor(Player.x / 800);
            game.Translate(800, 0);
        }
        else if ((int)Math.Floor(Player.x / 800) > _scrollIndex)
        {
            _scrollIndex = (int)Math.Floor(Player.x / 800);
            game.Translate(-800, 0);
        }
  

    }

    /// <summary>
    /// The Main
    /// </summary>
    internal static void Main()							// Main() is the first method that's called when the program is run
    {
        new MyGame().Start();
        
        
        // Create a "MyGame" and start it
    }
}
