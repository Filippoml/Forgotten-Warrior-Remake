using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Classes;
using GXPEngine.Core;
using TiledMapParser;

/// <summary>
/// Defines the <see cref="MyGame" />
/// </summary>
public class MyGame : Game
{
    /// <summary>
    /// Defines the mapPath
    /// </summary>
    AnimationSprite sprite;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyGame"/> class.
    /// </summary>
    public MyGame() : base(800, 600, false, false, pPixelArt: true)		// Create a window that's 800x600 and NOT fullscreen
    {

        scale = 0.8f;
        Player player = new Player();
        AddChild(player);

        Map level = MapParser.ReadMap("level.tmx");

        Data leveldata = level.Layers[0].Data;


        Texture2D texture = new Texture2D(level.TileSets[0].Image.FileName);
     

       // sprite.x = width / 2;
       // sprite.y = height / 2;

        String levelData = level.Layers[0].Data.innerXML.ToString();
        levelData = levelData.Replace("\n", "");

        int[] tiles = Array.ConvertAll(levelData.Split(','), int.Parse);

      //  sprite.currentFrame = -1+tiles[3 * level.Layers[0].Width + 5];
       // sprite.currentFrame = 0;

        int columns = level.Layers[0].Width;
        int rows = level.Layers[0].Height;
        for (int j = 0; j < tiles.Length; j++)
        {

            if (tiles[j] != 0)
            {

                int x = j % columns;
                int y = j / columns;
                Console.WriteLine(x + "---" + y);
                sprite = new AnimationSprite(texture.bitmap, level.TileSets[0].Columns, level.TileSets[0].Rows);
                AddChild(sprite);
                sprite.x = x * 70;
                sprite.y = y * 70;
                sprite.currentFrame = tiles[j] - 1;

            }

        }
    }

    /// <summary>
    /// The Update
    /// </summary>
    internal void Update()
    {
     //   sprite.NextFrame();
    }

    /// <summary>
    /// The Main
    /// </summary>
    internal static void Main()							// Main() is the first method that's called when the program is run
    {
        new MyGame().Start();					// Create a "MyGame" and start it
    }
}
