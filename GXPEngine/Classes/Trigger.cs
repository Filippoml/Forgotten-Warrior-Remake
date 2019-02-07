using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Trigger : Sprite
    { 
        public Trigger(float x, float y) : base("Data/Sprites/trigger.png")
        {
            this.x = x;
            //32 is the height of the level tile 
            this.y = y - 32 - this.height;
        }
    }
}
