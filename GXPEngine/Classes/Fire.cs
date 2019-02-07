using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GXPEngine.Classes
{
    public class Fire : AnimationSprite
    {
        private bool _movingRight, _onGround, _canDamage;

        private readonly int _screenSection, _frameRate;
        private int _frameCounter;

        private readonly float _speed;
        private float _yVelocity;
        private Player _player;

        public Fire(float x, float y) : base("Data/AnimationSprites/fire.png", 2 , 1)
        {
            this.x = x;
            //32 is the height of the level tile 
            this.y = y - 32 - this.height;

            _frameRate = 12;
            _speed = 5.5f;

            _player = ((MyGame)game).GetPlayer();

            //Check in which part of the screen it is
            _screenSection = Convert.ToInt32(Math.Floor(this.x / 800));

            _movingRight = Convert.ToBoolean(Utils.Random(0, 2));
        }

        void Update()
        {
            if (!((MyGame)game).IsPaused())
            {
                //Variables setted every frame
                _onGround = false;
                _frameCounter++;

                Animate();

                checkCollisions();

                move();

                applyGravity();
            }
        }

        private void Animate()
        {
            if (_frameCounter == (60 / _frameRate))
            {
                //Fo fix: player damaged too many times
                if(DistanceTo(_player) > _player.width)
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
            //Change direction if colliding with the border of the screen
            if (_screenSection != Convert.ToInt32(Math.Floor((this.x) / (800))) || _screenSection != Convert.ToInt32(Math.Floor((this.x + width) / (800))))
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
                    if (((_tile.GetId() >= 1 && _tile.GetId() <= 3) || (_tile.GetId() >= 7 && _tile.GetId() <= 9)))
                    {
                        y = _tile.y - this.height + 10;
                        _onGround = true;
                    }

                    //Check if is colliding with border tiles
                    if (_tile.GetId() == 1 || _tile.GetId() == 3 || _tile.GetId() == 7 || _tile.GetId() == 9)
                    {
                        _movingRight = !_movingRight;
                    }
                }
                //Check collision with player
                else if (collisions[i] == _player.getCollider() && _player.GetState() != Player.State.HIDING && (_player.GetYClimb() < 13 || _player.GetYClimb() > 95))
                {    
                    if (_canDamage)
                    {
                        _canDamage = false;
                        _player.Attacked(25);                     
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
