using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    
    public class LifeBar : GameObject
    {
        private EasyDraw _easyDraw;
        private Sprite _background, _background2, _background3;

        public LifeBar() : base()
        {
          
            

            Bitmap Bmp = new Bitmap(100, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(Color.Black);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _background = new Sprite(Bmp);
            AddChild(_background);
            _background.width = 40;
            _background.height = 10;
            _background.x = 7;
            _background.y = -12;

            Bmp = new Bitmap(100, 100);
            gfx = Graphics.FromImage(Bmp);
            brush = new SolidBrush(Color.Yellow);
            gfx.FillRectangle(brush, 0, 0, 100, 100);

            _background2 = new Sprite(Bmp);
            AddChild(_background2);
            _background2.width = 36;
            _background2.height = 6;
            _background2.x = 8.75f;
            _background2.y = -9.7f;

            Bmp = new Bitmap(100, 100);
            gfx = Graphics.FromImage(Bmp);
            brush = new SolidBrush(Color.Red);
            gfx.FillRectangle(brush, 0, 0, 100, 100);


            _background3 = new Sprite(Bmp);
            AddChild(_background3);
            _background3.width = 36;
            _background3.height = 6;
            _background3.x = 8.75f;
            _background3.y = -9.7f;


            //AddChild(rectangle);

        }

        public void Update(int lifepoints)
        {

            if(_background3.width != (36 * lifepoints) / 100 && _background3.width > 0)
            {
                //_background3.width--;
                _background3.width = (36 * lifepoints) / 100;
            }
            else
            {
                //_background2.x = 8.6f;
            }
            
        }


        

   
    }
}
