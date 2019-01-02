using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GXPEngine.Classes
{
    class Player : AnimationSprite
    {
        int counter = 0;
        int frameRate = 15;

        int speed = 2;
        float velocity = 0;
        ElapsedGameTime test;

        private bool _moving = false, _colliding = false;

        public Player() : base("Data/player.png", 5, 7)
        {
            SetFrame(9);
            SetScaleXY(0.2f);
            currentFrame = 0;
            test = new ElapsedGameTime();
        }

        
        void Update()
        {
            Console.WriteLine(test.getTotalSeconds());
            velocity += 0.5f;
            y += velocity;
            float old_x = x;
            float old_y = y;

            counter++;

            _moving = false;
            if (!_colliding)
            {
                if (Input.GetKey(Key.A))
                {
                    x = x - speed;

                    if (!_mirrorX)
                    {
                        Mirror(true, false);
                    }

                    _moving = true;
                }
                else if (Input.GetKey(Key.D))
                {
                    x = x + speed;

                    if (_mirrorX)
                    {
                        Mirror(false, false);
                    }

                    _moving = true;
                }
                else if (Input.GetKey(Key.W))
                {
                    y = y - speed;


                    _moving = true;
                }
                else if (Input.GetKey(Key.S))
                {
                    y = y + speed;

                    _moving = true;

                }


            }

            if (counter == (60 / frameRate))
            {

                if (_moving)
                {

                    NextFrame();

                    if (currentFrame > 9 || currentFrame < 5)
                    {
                        currentFrame = 5;
                    }

                }
                else
                {
                    NextFrame();
                    if (currentFrame > 4)
                    {
                        currentFrame = 0;
                    }
                }
                counter = 0;

            }

            _colliding = false;


            for (int i = 0; i < MyGame.Objects.Length; i++)
            {
                if (MyGame.Objects[i] != null)
                {
                    if (this.HitTest(MyGame.Objects[i]))
                    {
                        i = MyGame.Objects.Length;
                        _colliding = true;
                        x = old_x;
                        y = old_y;
                    }
                }
            }
        }
    }
}

