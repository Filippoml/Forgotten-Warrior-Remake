using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using GXPEngine.GXPEngine;

namespace GXPEngine.Classes
{
    public class Wizard : AnimationSprite
    {

        /// <summary>
        /// Defines the _colliding
        /// </summary>
        private bool _colliding = false;

        private float _velocity = 0, _speed = 2, _xCanFlip;

        private int _counter = 0, _frameRate = 12, _screenSection;

        private bool _movingRight = false, _canFlip = true, _canSpawnProjectile;

        private System.Timers.Timer _timer, _timer2, _timer3;
        private Status _status;

        private Collider _colliderBox, _colliderBox2;

        private Weapon _currentWeapon;

        private int _lifePoints;

        private LifeBar _lifeBar;

        private State _currentState;

        private Sprite _hitSprite;

        private Player _player;

        private enum State
        {
            /// <summary>
            /// Defines the IDLE
            /// </summary>
            IDLE,
            /// <summary>
            /// Defines the MOVING
            /// </summary>
            MOVING,
            /// <summary>
            /// Defines the JUMPING
            /// </summary>
            JUMPING,

            FALLING,
            ATTACKING,
            CLIMBING,
            SLEEPING,
            SEEING,
            DYING
        }
        
        /// <summary>
        /// Defines the _currentState
        /// </summary>

        /// <summary>
        /// Initializes a new instance of the <see cref="Swordman"/> class.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// 
        bool test = true;
        public Wizard(float x, float y) : base("Data/wizard.png", 5, 1)
        {
            SetScaleXY(1.2f);
            this.x = x;
            this.y = y;
            currentFrame = 1;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Interval = 100;
            _timer.Enabled = true;

            _timer2 = new System.Timers.Timer();
            _timer2.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
            _timer2.Interval = 500;
            _timer2.Enabled = false;

            _timer3 = new System.Timers.Timer();
            _timer3.Elapsed += new ElapsedEventHandler(OnTimedEvent3);
            _timer3.Interval = 1000;
            _timer3.Enabled = false;

            _status = new Status();
            AddChild(_status);


            _currentWeapon = new Weapon();
            _currentWeapon.SetWeapon(4);
            AddChild(_currentWeapon);

            _screenSection = Convert.ToInt32(Math.Floor(this.x / 800));


            _colliderBox = new Collider("Data/HitBox.png", this);
            _colliderBox2 = new Collider("Data/HitBox2.png", this);
            _colliderBox2.SetOrigin(-20, -20);
            AddChild(_colliderBox);
            AddChild(_colliderBox2);


            _lifeBar = new LifeBar();
            _lifeBar.SetXY(-10, -5);
            _lifeBar.visible = false;
            AddChild(_lifeBar);

            _lifePoints = 100;

            _hitSprite = new Sprite("Data/hit.png");
            AddChild(_hitSprite);
            _hitSprite.visible = false;
            _hitSprite.SetScaleXY(0.8f);
            _hitSprite.SetXY(0, 15);

            _player = ((MyGame)game).GetPlayer();
        }

        private void OnTimedEvent3(object sender, ElapsedEventArgs e)
        {
            _timer3.Enabled = false;
            _canSpawnProjectile = true;

            _status.SetVisible(false);
        }

        private void OnTimedEvent2(object sender, ElapsedEventArgs e)
        {
            _lifeBar.visible = false;
            _timer2.Enabled = false;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            test = true;

        }

        void Update()
        {

            if (!((MyGame)game).IsPaused())
            {

                _counter++;

                _colliding = false;


                if (_currentState != State.DYING)
                {
                    GameObject[] collisions = _colliderBox.GetCollisions();
                    for (int i = 0; i < collisions.Length; i++)
                    {

                        if (collisions[i] is Tile)
                        {

                            Tile _tile = collisions[i] as Tile;
                            //Check if player is colliding with ground tiles
                            if (_tile.GetId() >= 1 && _tile.GetId() <= 3)
                            {

                                y = _tile.y - this.height + 10;
                                _colliding = true;
                            }

                            if (_tile.GetId() == 1 || _tile.GetId() == 3 || _screenSection != Convert.ToInt32(Math.Floor((this.x) / (800))) || _screenSection != Convert.ToInt32(Math.Floor((this.x + width + 1) / (800))))
                            {

                                if (_canFlip)
                                {
                                    _canFlip = false;
                                    _movingRight = !_movingRight;
                                    _xCanFlip = this.x;
                                }
                            }
                            else
                            {
                                _canFlip = true;
                            }




                        }


                    }


                    if (_currentState == State.MOVING && _currentState != State.ATTACKING)
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



                    if (_currentState != State.SLEEPING && Math.Floor((this.x + this.width) / 800) == Math.Floor(_player.x / 800) && Math.Abs(this.y - _player.y) < 60 && _player.GetState() != Player.State.HIDING)
                    {
                        //Detect player
                        if (_player.x > this.x && !_mirrorX)
                        {



                            _status.SetVisible(true);
                            _status.SetFrame(2);

                            _currentState = State.ATTACKING;


                        }
                        else if (_player.x < this.x && _mirrorX)
                        {

                            _status.SetVisible(true);
                            _status.SetFrame(2);


                            _currentState = State.ATTACKING;
                        }
                        else
                        {
                            _currentState = State.IDLE;
                        }
                    }
                    else
                    {

                        if (_currentState == State.ATTACKING)
                        {
                            _currentState = State.IDLE;
                        }


                    }


                    //I HAVE TO EDIT THE NAME OF THE VARIABLE
                    if (test && _currentState != State.ATTACKING)
                    {
                        test = false;
                        Random rnd = new Random();
                        _timer.Interval = rnd.Next(1000, 2500);
                        if (_currentState != State.ATTACKING && _colliding)
                        {



                            int _randomAction = rnd.Next(0, 5);
                            _status.SetVisible(false);

                            switch (_randomAction)
                            {
                                case 1:
                                case 2:
                                    _currentState = State.IDLE;

                                    int _randomStatus = rnd.Next(0, 2);

                                    if (_randomStatus == 0)
                                    {
                                        _status.SetVisible(true);
                                        _status.SetFrame(_randomStatus);
                                    }
                                    break;

                                case 3:
                                    _currentState = State.MOVING;
                                    int _direction = rnd.Next(0, 2);

                                    if (_direction == 1)
                                    {
                                        _movingRight = true;
                                    }
                                    else
                                    {
                                        _movingRight = true;
                                    }
                                    break;

                                case 4:
                                    _currentState = State.SLEEPING;
                                    _status.SetVisible(true);
                                    _status.SetFrame(1);

                                    break;
                            }

                        }
                    }

                }
                else
                {

                    _colliding = true;
                }

                if (_counter == (60 / _frameRate))
                {
                    /*
                    if (_currentState == State.ATTACKING)
                    {
                        if (_player.x > this.x)
                        {
                            _movingRight = true;
                        }
                        else if (_player.x < this.x)
                        {
                            _movingRight = false;
                        }

                    }
                    */

                    _currentWeapon.SetVisible(false, _mirrorX, -25, 15);

                    if (_movingRight)
                    {
                        if (_mirrorX)
                        {
                            _colliderBox2.SetOrigin(-20, -20);
                            Mirror(false, false);
                        }
                    }
                    else
                    {
                        if (!_mirrorX)
                        {
                            _colliderBox2.SetOrigin(20, -20);
                            Mirror(true, false);
                        }
                    }
                    if (_currentState != State.DYING)
                    {
                        if (currentFrame > 0)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            NextFrame();
                        }
                    }

                    switch (_currentState)
                    {

                        case State.SLEEPING:
                            _status.floating();
                            break;
                        case State.ATTACKING:

                            _timer3.Enabled = true;

                            if (_canSpawnProjectile)
                            {


                                Projectile _projectile = new Projectile(x, y, !_mirrorX);



                                ((MyGame)game).GetLevel().AddChild(_projectile);

                                _canSpawnProjectile = false;
                            }




                            break;
                        case State.DYING:
                            if (currentFrame == 3)
                            {
                                currentFrame = 4;
                            }
                            else if (currentFrame == 4)
                            {
                                this.Destroy();
                            }
                            else
                            {
                                currentFrame = 3;
                            }
                            break;
                    }

                    _counter = 0;
                    _hitSprite.visible = false;
                }



                if (!_colliding)
                {
                    _velocity += 0.2f;
                    y += _velocity;
                }
            }
        }

        public void Attacked(int damage)
        {
            if (_player.x > this.x)
            {
                x -= 10;
            }
            else if (_player.x < this.x)
            {
                x += 10;
            }

            _lifePoints -= damage;
            _lifeBar.visible = true;
            _lifeBar.Update(_lifePoints);
            _timer2.Enabled = true;

            
            _hitSprite.visible = true;

            if (_player.x > this.x)
            {
                _movingRight = true;
            }
            else if (_player.x < this.x)
            {
                _movingRight = false;
            }

            if (_lifePoints <= 0)
            {
                _currentState = State.DYING;
            }
        }

        public Collider getCollider()
        {
            return _colliderBox;
        }

    }
}
