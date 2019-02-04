using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Weapon : AnimationSprite
    {
        private int _range = 4;

        private float _xVelocity = 2, _startX;

        private bool _returning = false;
        public Weapon () : base("Data/weapons.png",5,1)
        {
           
            y = 15;
            visible = false;
  
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
            _startX = x;
        }
        public void SetWeapon(int weapon_number)
        {
            currentFrame = weapon_number;
        }

        void Update()
        {
            if (!((MyGame)game).IsPaused())
            {
                if (currentFrame != 0)
                {
                    if (Math.Abs(x) < _range * 20 && !_returning && visible)
                    {
                        _xVelocity += 0.2f;
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
                    else if (x > _startX && visible && !_mirrorX)
                    {


                        x -= _xVelocity;

                        _returning = true;


                    }
                    else if (x < _startX && visible && _mirrorX)
                    {
                        x += _xVelocity;
                        _returning = true;

                    }
                    else
                    {
                        _xVelocity = 2;
                        _returning = false;
                        visible = false;
                    }
                }
            }
        }

        public void SetReturing()
        {
            if (currentFrame != 0)
            {
                _returning = true;
            }
        }

        public int GetWeapon()
        {
            return currentFrame;
        }

    }
}
