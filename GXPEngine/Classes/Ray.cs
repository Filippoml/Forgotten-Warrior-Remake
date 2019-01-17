using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.GXPEngine;

namespace GXPEngine.Classes
{
    class Ray : Sprite
    {
        public Ray() : base("Data/HitBox.png")
        {

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

    
    

