using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.GXPEngine;

namespace GXPEngine.Classes
{
    public class Fire : AnimationSprite
    {
        private bool _movingRight, _onGround, _canDamage;

        private readonly int _screenSection;

        private int _frameCounter, _frameRate;

        private float _speed, _yVelocity;

        private Player _player;

        public Fire(float x, float y) : base("Data/fire.png", 2 , 1)
        {
            this.x = x;
            this.y = y;

            //Init
            _frameRate = 12;
            _speed = 2.5f;
            _player = ((MyGame)game).GetPlayer();

            //Check in which part of the screen it is
            _screenSection = Convert.ToInt32(Math.Floor(this.x / 800));

            Random _rnd = new Random();
            _movingRight = Convert.ToBoolean(_rnd.Next(0, 2));


        }

        void Update()
        {
            //Variables setted every frame
            _onGround = false;
            _frameCounter++;

            Animate();

            checkCollisions();

            move();

            applyGravity();
        }

        private void Animate()
        {
            if (_frameCounter == (60 / _frameRate))
            {

                if(DistanceTo(_player) > 100)
                {
                    _canDamage = true;
                }

                if (currentFrame == 1)
                {
                    currentFrame = 0;
                }
                else
                {
                    NextFrame();
                }
                _frameCounter = 0;
            }
        }

        private void checkCollisions()
        {
            //TODO check for swordman
            if (_screenSection != Convert.ToInt32(Math.Floor(this.x / 780)))
            {
                _movingRight = !_movingRight;
            }

            GameObject[] collisions = this.GetCollisions();
            for (int i = 0; i < collisions.Length; i++)
            {

                if (collisions[i] is Tile)
                {

                    Tile _tile = collisions[i] as Tile;

                    //Check if is colliding with ground tiles
                    if (_tile.GetId() >= 1 && _tile.GetId() <= 3)
                    {

                        y = _tile.y - this.height + 10;
                        _onGround = true;
                    }

                    if (_tile.GetId() == 1 || _tile.GetId() == 3)
                    {
                        _movingRight = !_movingRight;


                    }
                }
                else if (collisions[i] == _player.getCollider())
                {    
                    if (_canDamage)
                    {
                        
                        _canDamage = false;
                        _player.Attacked(10);                     
                    }
                }
            }
        }

        private void move()
        {
            if (_movingRight)
            {
                x += _speed;
            }
            else
            {
                x -= _speed;
            }
        }

        private void applyGravity()
        {
            if (!_onGround)
            {
                _yVelocity += 0.2f;
                y += _yVelocity;
            }
        }
    }
}
