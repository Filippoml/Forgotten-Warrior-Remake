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

        private Sprite _background;

        private int _indexButton;

        private Font _font;

        private EasyDraw _easyDraw;


        public Menu() : base()
        {


            Sprite _menu1 = new Sprite("Data/mm1.png");

            Bitmap Bmp = new Bitmap(100, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(Color.Yellow);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _background = new Sprite(Bmp);
            _background.width = 100;
            _background.height = 12;
            _background.x = 300 + (_menu1.width / 2);
            _background.y = 200 + 5;
            AddChild(_background);


            _menu1.x = 300 + (_menu1.width / 2);
            _menu1.y = 200;
            AddChild(_menu1);

            _easyDraw = new EasyDraw(800, 600);
            AddChild(_easyDraw);

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("Data/LCD Solid.ttf");
            _font = new Font(new FontFamily(pfc.Families[0].Name), 8, FontStyle.Regular);

            _easyDraw.graphics.DrawString("Game Programming Assigment", _font, new SolidBrush(Color.White), new PointF(615, 575));
            _easyDraw.graphics.DrawString("by Filippo Maria Leonardi", _font, new SolidBrush(Color.White), new PointF(622, 585));
        }

        void Update()
        {
            if(Input.GetKeyDown(Key.DOWN))
            {
                if (_indexButton < 1)
                {
                    _background.y += 24;
                    _indexButton++;
                }

            }
            if(Input.GetKeyDown(Key.UP))
            {
                if (_indexButton > 0)
                {
                    _background.y -= 24;
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
