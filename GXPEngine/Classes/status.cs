using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    class Status : AnimationSprite
    {
        private bool _floating = false;
        public Status () : base("Data/status.png", 3, 1) {
            currentFrame = 3;
            x = 20;
            y = -5;
            SetScaleXY(0.6f);
            visible = false;
        }

        public void SetVisible(bool value)
        {
            visible = value;
        }

        //Visual effect
        public void floating()
        {
            _floating = !_floating;
            if (_floating)
            {
                y -= 2;
                
            }
            else
            {
                y += 2;
            }
        }
    }
}
