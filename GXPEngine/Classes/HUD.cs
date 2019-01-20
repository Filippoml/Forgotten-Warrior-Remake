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
        
        public HUD()
        {
            
            Sprite stats = new Sprite("Data/stats.png");
            AddChild(stats);

            _player = ((MyGame)game).GetPlayer();

            Bitmap Bmp = new Bitmap(300, 200);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#FF969F"));
            gfx.FillRectangle(brush, 97, 14.5f, 46, 3);
            Sprite _background = new Sprite(Bmp);
            AddChild(_background);

            Bmp = new Bitmap(200, 200);
            gfx = Graphics.FromImage(Bmp);
            brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#E70052"));
            gfx.FillRectangle(brush, 97, 17.5f, 46, 3);
            _background = new Sprite(Bmp);
            AddChild(_background);

            //TODO check the same order

            Bmp = new Bitmap(300, 200);
            gfx = Graphics.FromImage(Bmp); 
             brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#0080FF"));
            gfx.FillRectangle(brush, 176, 17.5f, 56, 3);
            _background = new Sprite(Bmp);
            AddChild(_background);

            Bmp = new Bitmap(300, 200);
            gfx = Graphics.FromImage(Bmp);
            brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#6DCFF6"));
            gfx.FillRectangle(brush, 176, 14.5f, 56, 3);
            _background = new Sprite(Bmp);
            AddChild(_background);

            for (int i = 0; i< 3; i++)
            {
                Sprite notch = new Sprite("Data/notch.png");
                notch.SetXY(189 + 19 * i, 9);
                AddChild(notch);
            }
        }

        public void Update()
        {
            //Todo refresh problem
            SetXY(300 + game.width * Convert.ToInt32(Math.Floor(_player.x / 800)), 1170);
        }
    }
}
