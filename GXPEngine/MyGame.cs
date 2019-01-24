using System;                                   // System contains a lot of default C# libraries 
using System.Drawing;
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Classes;
using GXPEngine.Core;
using GXPEngine.GXPEngine;
using TiledMapParser;
using System.Windows;

/// <summary>
/// Defines the <see cref="MyGame" />
/// </summary>
public class MyGame : Game
{


    private HUD _hud;
    private Level _level;
    private Shop _shop;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyGame"/> class.
    /// </summary>
    public MyGame() : base(800, 600, false, false, pPixelArt: true)		// Create a window that's 800x600 and NOT fullscreen
    {
        

        
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

        _level = new Level(1);
        _level.generateLevel();
        AddChild(_level);


        _hud = new HUD();
        _hud.SetXY(300, 1170);
        AddChild(_hud);

        _shop = new Shop(175, 650);
        game.Translate(0, -600);
        _shop.visible = true;
        AddChild(_shop);


    }

    public void ShowShop(bool value)
    {
        _shop.visible = value;        
    }

    public Player GetPlayer()
    {
        return _level.GetPlayer();
    }

    public Level GetLevel()
    {
        return _level;
    }

    /// <summary>
    /// The Update
    /// </summary>
    void Update()
    {
        _hud.Update();
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
