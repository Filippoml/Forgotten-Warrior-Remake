using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Properties;

namespace GXPEngine.Classes
{
    public class Menu : GameObject
    {
        private Level _level;

        private Sprite _buttonSelectedSprite;

        private int _indexButton;

        private readonly Font _font;

        private EasyDraw _easyDraw;

        public Menu() : base()
        {
            Sprite _menuSprite = new Sprite("Data/HUD/mm1.png");

            Bitmap Bmp = new Bitmap(100, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(Color.Yellow);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _buttonSelectedSprite = new Sprite(Bmp)
            {
                width = 100,
                height = 12,
                x = 300 + (_menuSprite.width / 2)
            };

            if (Data.Default.level == 1)
            {
                _buttonSelectedSprite.y = 200 + 5;
            }
            else
            {
                _buttonSelectedSprite.y = 200 + 5 + 24;
                _indexButton++;
            }

            AddChild(_buttonSelectedSprite);

            _menuSprite.x = 300 + (_menuSprite.width / 2);
            _menuSprite.y = 200;
            AddChild(_menuSprite);

            _easyDraw = new EasyDraw(800, 600);
            AddChild(_easyDraw);

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("Data/Font/LCD Solid.ttf");
            _font = new Font(new FontFamily(pfc.Families[0].Name), 8, FontStyle.Regular);

            _easyDraw.graphics.DrawString("Game Programming Assigment", _font, new SolidBrush(Color.White), new PointF(615, 575));
            _easyDraw.graphics.DrawString("by Filippo Maria Leonardi", _font, new SolidBrush(Color.White), new PointF(622, 585));

            _easyDraw.graphics.DrawString("Controls", _font, new SolidBrush(Color.White), new PointF(0, 555));
            _easyDraw.graphics.DrawString("Press 1 to use health postions", _font, new SolidBrush(Color.White), new PointF(0, 565));
            _easyDraw.graphics.DrawString("Press 2 to use mana postions", _font, new SolidBrush(Color.White), new PointF(0, 575));
            _easyDraw.graphics.DrawString("Press 3 to use the special attack", _font, new SolidBrush(Color.White), new PointF(0, 585));
        }

        void Update()
        {
            if((Input.GetKeyDown(Key.DOWN) || Input.GetKeyDown(Key.S)) && Data.Default.level != 1)
            {
                if (_indexButton < 1)
                {
                    _buttonSelectedSprite.y += 24;
                    _indexButton++;
                }
            }

            if(Input.GetKeyDown(Key.UP) || Input.GetKeyDown(Key.W))
            {
                if (_indexButton > 0)
                {
                    _buttonSelectedSprite.y -= 24;
                    _indexButton--;
                }
            }

            if (Input.GetKeyDown(Key.ENTER))
            {
                switch (_indexButton)
                {
                    case 0:
                        Data.Default.Reset();

                        _level = ((MyGame)game).GetLevel();
                        ((MyGame)game).AddChild(_level);
                        _level.generateLevel(1);
                        this.Destroy();
                        break;
                    case 1:
                        _level = ((MyGame)game).GetLevel();
                        ((MyGame)game).AddChild(_level);
                        _level.generateLevel(Data.Default.level);
                        this.Destroy();
                        break;
                }
            }
        }

        public Level GetLevel()
        {
            return _level;
        }
    }
}