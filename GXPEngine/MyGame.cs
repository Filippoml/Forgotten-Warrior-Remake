using System.Drawing;
using GXPEngine;                                
using GXPEngine.Classes;
using GXPEngine.OpenGL;
using TiledMapParser;

public class MyGame : Game
{
    private Level _level;
    private Sprite _game, _over;

    private bool _paused, _playerDied;

    private float _slideVelocity;

    private Sprite _background2, _background3;

    private int _timePassed;

    private SoundChannel _soundChannel;

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


        _background2 = new Sprite(Bmp);
 



        _background3 = new Sprite(Bmp);


        _background2.y = -200;
        _background3.y = 600;




        _slideVelocity = 1;
    }

    public void ChangeLevel()
    {
        _level.Destroy();
        _level = new Level();
        AddChild(_level);
        
        GXPEngine.Properties.Data.Default.level++;

        GXPEngine.Properties.Data.Default.Save();
        _level.generateLevel(GXPEngine.Properties.Data.Default.level);
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

    /// <summary>
    /// The Update
    /// </summary>
    void Update()
    {
        
        if (_playerDied)
        {

            _timePassed += Time.deltaTime;

            if ((_background2.y + _slideVelocity + 1) < 0)
            {
                _slideVelocity += 0.1f;
                _background2.y += _slideVelocity;

                _background3.y -= _slideVelocity;

                if(!_paused)
                {
                    _paused = true;
                }
            }
            else
            {
                if(_game == null)
                {
                    _game = new Sprite("Data/game.png");
                }

                if(_over == null)
                {
                    _over = new Sprite("Data/over.png");
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
                   
                    _background2.y = -200;
                    _background3.y = 600;
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
        
    }

    public void PlayerDied()
    {
        _soundChannel.Stop();
        AddChild(_background2);
        AddChild(_background3);
        _playerDied = true;
    }

    // Main() is the first method that's called when the program is run
    internal static void Main()	
    {
        // Create a "MyGame" and start it
        new MyGame().Start();


    }
}
