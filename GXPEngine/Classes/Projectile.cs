using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Projectile : Sprite
    {
        private bool _hasToMoveRight;

        private int _screenSection;

        private float _speed;

        private Player _player;

        public Projectile (float x, float y, bool hasToMoveRight) : base ("Data/projectile.png")
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

                if (_hasToMoveRight)
                {
                    x += _speed;
                }
                else
                {
                    x -= _speed;
                }



                if (_screenSection != Convert.ToInt32(Math.Floor(this.x / 780)))
                {
                    this.Destroy();
                }

                if (_player != null)
                {
                    if (_player.GetState() != Player.State.HIDING)
                    {
                        if (this.HitTest(_player))
                        {
                            //_player.Attacked(25);
                            this.Destroy();
                        }
                    }
                }
            }

        }
    }
}
