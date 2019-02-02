using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Dialog : GameObject
    {
        private Font _font;

        private EasyDraw _easyDraw;

        private Player _player;

        private HUD _hud;

        public Dialog()
        {
            Bitmap Bmp = new Bitmap(800, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#A57300"));
            gfx.FillRectangle(brush, 0, 0, 800, 100);
            Sprite _background = new Sprite(Bmp);
            _background.width = 800;
            _background.height = 100;
            _background.y = 8;
            AddChild(_background);

            _player = ((MyGame)game).GetPlayer();


            for (int i = 0; i < 8; i++)
            {
                Sprite _concon = new Sprite("Data/concon.png");
               
                
              
                if(i > 3)
                {
                    _concon.y = 100;
                    _concon.x = (i - 4) * 240;
                }
                else
                {
                    _concon.x = i * 240;
                }


                AddChild(_concon);

            }


            _easyDraw = new EasyDraw(800, 100);
            AddChild(_easyDraw);

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("Data/LCD Solid.ttf");
            _font = new Font(new FontFamily(pfc.Families[0].Name), 15, FontStyle.Regular);
        }

        public void SetVisible (bool visible, int lootQuantity, String lootType)
        {
            ((MyGame)game).SetPaused(visible);
            this.visible = visible;

            if (lootQuantity != 0)
            {
                Sprite _item = new Sprite("Data/" + lootType + ".png");

                if(lootType == "coin")
                {
                    _easyDraw.graphics.DrawString(lootQuantity.ToString(), _font, new SolidBrush(Color.White), new PointF(380, 50));
                    _item.x = 415;
                    _player.SetCoinsNumber(true, lootQuantity);
                }
                else
                {
                    if(lootType == "item3")
                    {
                        _player.SetHealthPotionsNumber(true);
                    }
                    else
                    {

                       _player.SetManaPotionsNumber(true);
                    }

                    _item.x = 400 - (_item.width / 2);
                }
                _item.y = 48;
                AddChild(_item);
            }

        }

    }
}
