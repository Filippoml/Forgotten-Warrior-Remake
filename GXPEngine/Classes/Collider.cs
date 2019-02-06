using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Collider : Sprite
    {
        private readonly GameObject _owner;

        public Collider(String filename, GameObject owner) : base(filename)
        {
            visible = false;
            this._owner = owner;
        }

        public GameObject GetOwner()
        {
            return _owner;
        }
    }
}

    
    

