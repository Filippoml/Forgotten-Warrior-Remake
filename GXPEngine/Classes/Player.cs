using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GXPEngine.Classes
{
    class Player : AnimationSprite
    {
        int counter=0;
        int frameRate = 13;

        
        public Player() : base("Data/player.png", 8, 7)
        {
            SetFrame(9);

        }

        void Update()
        { 
            counter++;
            if (counter == 60/frameRate)
            {



                //_currentFrame = 100;/*
                NextFrame();
                if (currentFrame > 15)
                {
                    currentFrame = 9;
                }
                counter = 0;
            }
        }
    }
}
