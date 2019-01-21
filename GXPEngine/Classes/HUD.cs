using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class HUD : GameObject
    {
        private Player _player;

        private Sprite _life1, _life2, _mana1, _mana2;

        public HUD()
        {
            
            Sprite stats = new Sprite("Data/stats.png");
            AddChild(stats);

            _player = ((MyGame)game).GetPlayer();

            Bitmap Bmp = new Bitmap(100, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#FF969F"));
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _life1 = new Sprite(Bmp);
            AddChild(_life1);
            _life1.x = 97;
            _life1.y = 14.5f;
            _life1.width = 46;
            _life1.height = 3;


            gfx = Graphics.FromImage(Bmp);
            brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#E70052"));
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _life2 = new Sprite(Bmp);
            AddChild(_life2);
            _life2.x = 97;
            _life2.y = 17.5f;
            _life2.width = 46;
            _life2.height = 3;


            //TODO check the same order

            Bmp = new Bitmap(300, 200);
            gfx = Graphics.FromImage(Bmp); 
             brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#0080FF"));
            gfx.FillRectangle(brush, 176, 17.5f, 55, 3);
            _mana1 = new Sprite(Bmp);
            AddChild(_mana1);

            Bmp = new Bitmap(300, 200);
            gfx = Graphics.FromImage(Bmp);
            brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#6DCFF6"));
            gfx.FillRectangle(brush, 176, 14.5f, 55, 3);
            _mana2 = new Sprite(Bmp);
            AddChild(_mana2);

            

            for (int i = 0; i< 3; i++)
            {
                Sprite notch = new Sprite("Data/notch.png");
                notch.SetXY(189 + 19 * i, 9);
                AddChild(notch);
            }
        }

        public void Update()
        {
            //TODO refresh problem
            SetXY(300 + game.width * Convert.ToInt32(Math.Floor(_player.x / 800)), 1170);


            
            
            _life1.width = (46 * _player.GetLifePoints()) / 100;
            _life2.width = (46 * _player.GetLifePoints()) / 100;






        }
    }
}
