using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Weapon : AnimationSprite
    {
        private int _range = 2;

        private float _xVelocity = 2;

        private bool _returning = false;
        public Weapon () : base("Data/weapons.png",5,1)
        {
            y = 15;
            visible = false;
            currentFrame = 0;

        }

        public void SetVisible(bool value, bool mirrored, int mirroredOffset1, int mirroredOffset2)
        {
            visible = value;

                Mirror(mirrored, false);
                if (mirrored)
                {
                    x = mirroredOffset1;
                }
                else
                {
                   x = mirroredOffset2;
                }
        }
        public void SetWeapon(int weapon_number)
        {
            currentFrame = weapon_number;
        }

        void Update()
        {

            if(Math.Abs(x) < _range * 20 && !_returning && visible)
            {
                _xVelocity += 0.5f;
                if (_mirrorX)
                {
                    x -= _xVelocity;
                }
                else
                {
                    x += _xVelocity;
                }
          
                _returning = false;
            }
            else if(x > 0 && visible && !_mirrorX)
            {
 

                    x -= _xVelocity;
   
                _returning = true;


            }
            else if(x < 0 && visible && _mirrorX)
            {
                x += _xVelocity;
                _returning = true;

            }
            else
            {
                _xVelocity = 1;
                _returning = false;
                visible = false;

            }

        }

    }
}
