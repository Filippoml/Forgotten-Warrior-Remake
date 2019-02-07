using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Coin : Sprite
    {
        public Coin(float x, float y) : base("Data/Sprites/coin.png")
        {
            this.x = x;
            //32 is the height of the level tile 
            this.y = y - 32 - this.height;
        }
    }
}
