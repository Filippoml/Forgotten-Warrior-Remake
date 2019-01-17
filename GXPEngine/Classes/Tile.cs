using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.GXPEngine
{
    class Tile : AnimationSprite
    {
        private int _id;

        public Tile(int id, string filename, int cols, int rows) : base (filename, cols, rows)
        {
            _id = id;
 
        }

        public int GetId()
        {
            return _id;
        }
    }
}
