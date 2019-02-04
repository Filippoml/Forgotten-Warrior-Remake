using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Collider : Sprite
    {
        private GameObject _owner;
        public Collider(String filename, GameObject owner) : base(filename)
        {
            visible = false;
            this._owner = owner;
        }

        public GameObject getOwner()
        {
            return _owner;
        }

        public bool isColliding()
        {
            bool colliding = false;
            GameObject[] collisions = GetCollisions();


            for (int i = 0; i < collisions.Length; i++)
            {

                if (collisions[i] is Tile)
                {
                    Tile _tile = collisions[i] as Tile;
                    if (_tile.GetId() == 5)
                    {
                        return true;
                    }
                }

        
            }
            return colliding;

        }

            
        }
    }

    
    

