using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Projectile : Sprite
    {
        private readonly bool _hasToMoveRight;

        private readonly int _screenSection;

        private readonly float _speed;

        private Player _player;

        public Projectile (float x, float y, bool hasToMoveRight) : base ("Data/Sprites/projectile.png")
        {
            this.x = x + 20;
            this.y = y + 35;

             //Check in which part of the screen it is
            _screenSection = Convert.ToInt32(Math.Floor(this.x / 800));

            this._hasToMoveRight = hasToMoveRight;

            _speed = 2;

            _player = ((MyGame)game).GetPlayer();
        }

        void Update()
        {
            if (!((MyGame)game).IsPaused())
            {
                //Left or right?
                if (_hasToMoveRight)
                {
                    x += _speed;
                }
                else
                {
                    x -= _speed;
                }

                if (_screenSection != Convert.ToInt32(Math.Floor((this.x) / (800))) || _screenSection != Convert.ToInt32(Math.Floor((this.x + width) / (800))))
                {
                    this.Destroy();
                }

                if (_player != null)
                {
                    if (_player.GetState() != Player.State.HIDING && (_player.GetYClimb() < 13 || _player.GetYClimb() > 95))
                    {
                        if (this.HitTest(_player))
                        {
                            _player.Attacked(25);
                            this.Destroy();
                        }
                    }
                }
            }
        }
    }
}
