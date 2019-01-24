using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Weapon : AnimationSprite
    {
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
            
        }

    }
}
