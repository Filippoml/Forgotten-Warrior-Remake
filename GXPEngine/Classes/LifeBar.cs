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
        private Sprite _border, _background, _foreground;

        public LifeBar() : base()
        {
            //Border Rectangle
            Bitmap Bmp = new Bitmap(100, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(Color.Black);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _border = new Sprite(Bmp)
            {
                width = 40,
                height = 10,
                x = 7,
                y = -12
            };
            AddChild(_border);

            //Background Rectangle
            brush = new SolidBrush(Color.Yellow);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _background = new Sprite(Bmp)
            {
                width = 36,
                height = 6,
                x = 8.75f,
                y = -9.7f
            };
            AddChild(_background);

            //Foreground Rectangle
            brush = new SolidBrush(Color.Red);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _foreground = new Sprite(Bmp)
            {
                width = 36,
                height = 6,
                x = 8.75f,
                y = -9.7f
            };
            AddChild(_foreground);
        }

        public void Update(int lifepoints)
        {
            //Avoid negative lifepoints
            if(lifepoints < 0)
            {
                lifepoints = 0;
            }
            //Check if value is changed
            if(_foreground.width != (36 * lifepoints) / 100)
            {
                _foreground.width = (36 * lifepoints) / 100;
            }            
        }
    }
}