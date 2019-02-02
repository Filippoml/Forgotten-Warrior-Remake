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

    private bool _paused;

    private float _slideVelocity;

    private Sprite _background2, _background3;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyGame"/> class.
    /// </summary>
    public MyGame() : base(800, 600, false, false, pPixelArt: true)		// Create a window that's 800x600 and NOT fullscreen
    {
        

        
        //Background creation
        Bitmap Bmp = new Bitmap(width, height);
        Graphics gfx = Graphics.FromImage(Bmp);
        SolidBrush brush = new SolidBrush(Color.FromArgb(135, 206, 235));
        gfx.FillRectangle(brush, 0, 0, width, height);

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
        _shop.visible = false;
        AddChild(_shop);

        _level.GetPlayer().LoadHUD();


        Bmp = new Bitmap(800, 200);
        gfx = Graphics.FromImage(Bmp);
        brush = new SolidBrush(Color.Black);
        gfx.FillRectangle(brush, 0, 0, 800, 200);


        _background2 = new Sprite(Bmp);
        _background2.y = 400;
        AddChild(_background2);



        _background3 = new Sprite(Bmp);
        _background3.y = 1200;

        AddChild(_background3);

        _slideVelocity = 1;
    }

    public void SetPaused(bool value)
    {
        _paused = value;
    }

    public bool IsPaused()
    {
        return _paused;
    }

    public HUD GetHud()
    {
        return _hud;
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
        Console.WriteLine(_background2.y);
        if((_background2.y + _slideVelocity + 1) < 600)
        {
            _slideVelocity += 0.1f;
            _background2.y += _slideVelocity;

            _background3.y -= _slideVelocity;
        }
        else
        {
            Sprite _game = new Sprite("Data/game.png");
            _game.y = 800;
            _game.x = (width/2) - (_game.width/2);
            AddChild(_game);

            Sprite _over = new Sprite("Data/over.png");
            _over.y = 830;
            _over.x = (width / 2) - (_over.width / 2) + 5;
            AddChild(_over);

            _paused = true;
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
