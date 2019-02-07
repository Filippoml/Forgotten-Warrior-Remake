using System;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;                                
using GXPEngine.Classes;
using GXPEngine.OpenGL;
using TiledMapParser;

public class MyGame : Game
{
    private Level _level;

    private Sprite _game, _over;

    private bool _paused, _playerDied, _gameCompleted;

    private float _slideVelocity;

    private int _timePassed;

    private Sprite _gameOverRectangleUp, _gameOverRectangleDown;

    private SoundChannel _soundChannel;

    private readonly Font _font;

    private EasyDraw _easyDraw;


    public MyGame() : base(800, 600, false, false, pPixelArt: true)
    {
        Sound _sound = new Sound("Data/Sounds/theme.mp3", true, true);
        _soundChannel = _sound.Play();
        Menu _mainMenu = new Menu();
        AddChild(_mainMenu);

        _level = new Level();

        Bitmap Bmp = new Bitmap(800, 200);
        Graphics gfx = Graphics.FromImage(Bmp);
        SolidBrush brush = new SolidBrush(Color.Black);
        gfx.FillRectangle(brush, 0, 0, 800, 200);

        _gameOverRectangleUp = new Sprite(Bmp)
        {
            y = -200
        };

        _gameOverRectangleDown = new Sprite(Bmp)
        {
            y = 600
        };

        _slideVelocity = 1;

        _easyDraw = new EasyDraw(800, 600);
        AddChild(_easyDraw);

        PrivateFontCollection pfc = new PrivateFontCollection();
        pfc.AddFontFile("Data/Font/LCD Solid.ttf");
        _font = new Font(new FontFamily(pfc.Families[0].Name), 12, FontStyle.Regular);
    }

    public void ChangeLevel()
    {
        _level.Destroy();
        _level = new Level();
        AddChild(_level);

        if (GXPEngine.Properties.Data.Default.level < 3)
        {
            GXPEngine.Properties.Data.Default.level++;
            GXPEngine.Properties.Data.Default.Save();
            _level.generateLevel(GXPEngine.Properties.Data.Default.level);
        }
        else
        {
            _easyDraw.graphics.DrawString("Game Completed!", _font, new SolidBrush(Color.White), new PointF(width / 2 - 70, height / 2 - 100));
            _gameCompleted = true;
        }
    }

    void Update()
    {
        if (_playerDied)
        {
            _timePassed += Time.deltaTime;

            if ((_gameOverRectangleUp.y + _slideVelocity + 1) < 0)
            {
                _slideVelocity += 0.1f;
                _gameOverRectangleUp.y += _slideVelocity;

                _gameOverRectangleDown.y -= _slideVelocity;

                if(!_paused)
                {
                    _paused = true;
                }
            }
            else
            {
                if(_game == null)
                {
                    _game = new Sprite("Data/HUD/game.png");
                }

                if(_over == null)
                {
                    _over = new Sprite("Data/HUD/over.png");
                }
                if (_timePassed >= 5000)
                {
                    _playerDied = false;
                    _level.Destroy();
                    _level = new Level();

                    RemoveChild(_game);
                    RemoveChild(_over);

                    Menu _mainMenu = new Menu();
                    AddChild(_mainMenu);

                    _timePassed = 0;
                    _paused = false;
                   
                    _gameOverRectangleUp.y = -200;
                    _gameOverRectangleDown.y = 600;
                    _slideVelocity = 1;

                    Sound _sound = new Sound("Data/Sounds/theme.mp3", true, true);
                    _soundChannel = _sound.Play();
                }
                else if (_timePassed >=  2000)
                {
                    _over.y = 290;
                    _over.x = (width / 2) - (_over.width / 2) + 5;
                    AddChild(_over);
                }
                else if (_timePassed >= 1000)
                {
                    _game.y = 260;
                    _game.x = (width / 2) - (_game.width / 2);
                    AddChild(_game);
                }
            }
        }
        
        else if(_gameCompleted)
        {
            _timePassed += Time.deltaTime;
            if(_timePassed > 1500)
            {
                _timePassed = 0;
                _gameCompleted = false;

                _easyDraw.graphics.Clear(Color.Transparent);

                Menu _mainMenu = new Menu();
                AddChild(_mainMenu);
            }
            
        }
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
        HUD test = _level.GetHud();
        return _level.GetHud();
    }

    public Player GetPlayer()
    {
        return _level.GetPlayer();
    }

    public Level GetLevel()
    {
        return _level;
    }

    public void PlayerDied()
    {
        _soundChannel.Stop();
        AddChild(_gameOverRectangleUp);
        AddChild(_gameOverRectangleDown);
        _playerDied = true;
    }

    // Main() is the first method that's called when the program is run
    internal static void Main()	
    {
        // Create a "MyGame" and start it
        new MyGame().Start();
    }
}