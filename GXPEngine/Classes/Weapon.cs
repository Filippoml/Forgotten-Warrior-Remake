using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    class Weapon : AnimationSprite
    {
        public Weapon () : base("Data/weapons.png",4,1)
        {
            y = 15;
            visible = false;
            currentFrame = 0;

        }

        public void SetVisible(bool value, bool mirrored)
        {
            visible = value;
            if (value)
            {
                Mirror(mirrored, false);
                if (mirrored)
                {
                    x = -10;
                }
                else
                {
                    x = 10;
                }
            }
        }
        public void SetWeapon()
        {

        }

        void Update()
        {
            
        }

    }
}
