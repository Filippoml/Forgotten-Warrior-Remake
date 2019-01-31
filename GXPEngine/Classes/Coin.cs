using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Coin : Sprite
    {
        public Coin(float x, float y) : base("Data/coin.png")
        {
            this.x = x;
            this.y = y - 32 - this.height;
        }
    }
}
