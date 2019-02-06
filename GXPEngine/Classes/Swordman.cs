using System;
using System.Timers;


namespace GXPEngine.Classes
{
    /// <summary>
    /// Defines the <see cref="Swordman" />
    /// </summary>
    internal class Swordman : Enemy
    {
        /// <summary>
        /// Defines the _colliding
        /// </summary>
        private bool _colliding = false;

        private float _velocity = 0, _speed = 2;

        private int _counter = 0, _frameRate = 12, _screenSection;

        private bool _movingRight = false, _playerRight = false, _canFlip = true, delay = true, _attacked, _whereGo;

        private System.Timers.Timer _timer, _timer2, _timer3, _timer4;

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
            FOLLOWING,
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
        public Swordman(float x, float y) : base("Data/swordman.png", 5, 1)
        {
            SetScaleXY(1.3f);
            this.x = x;
            this.y = y - 32 - this.height;
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
            _timer3.Interval = 1500;
            _timer3.Enabled = false;


            _timer3 = new System.Timers.Timer();
            _timer3.Elapsed += new ElapsedEventHandler(OnTimedEvent3);
            _timer3.Interval = 1500;
            _timer3.Enabled = false;


            _timer4 = new System.Timers.Timer();
            _timer4.Elapsed += new ElapsedEventHandler(OnTimedEvent4);
            _timer4.Interval = 1500;
            _timer4.Enabled = false;

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

        private void OnTimedEvent4(object sender, ElapsedEventArgs e)
        {
            _currentState = State.FOLLOWING;
            _timer4.Enabled = false;
            _attacked = false;

        }

        private void OnTimedEvent3(object sender, ElapsedEventArgs e)
        {

            delay = true;
            _timer3.Enabled = false;
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

                Console.WriteLine(_currentState);
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
                            if (((_tile.GetId() >= 1 && _tile.GetId() <= 3) || (_tile.GetId() >= 7 && _tile.GetId() <= 9)))
                            {

                                y = _tile.y - this.height + 14;
                                _colliding = true;
                            }

                            if (_tile.GetId() == 1 || _tile.GetId() == 3 || _tile.GetId() == 7 || _tile.GetId() == 9 || _screenSection != Convert.ToInt32(Math.Floor((this.x - 10) / (800))) || _screenSection != Convert.ToInt32(Math.Floor((this.x + width + 1) / (800))))
                            {
                                if (_canFlip && !_attacked)
                                {
                                    _movingRight = !_movingRight;
                                }
                  
                                _canFlip = false;
                                break;
                            }
                            else
                            {
                              
                                _canFlip = true;
                            }


                            if (_currentState == State.ATTACKING)
                            {
                                _currentState = State.IDLE;
                            }


                        }
                        else if(collisions[i] is Sprite)
                        {
                            Sprite _tile = collisions[i] as Sprite;
                            if(_tile.texture.filename == "Data/blue_wave.png" && Math.Floor(_player.x / 800) == _screenSection)
                            {
                                Attacked(100);
                            }
                        }


                    }


                    if (_currentState == State.MOVING && _currentState != State.FOLLOWING)
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
                    else if (_currentState == State.FOLLOWING)
                    {
                        if (_playerRight)
                        {
                            x += _speed;
                        }
                        else
                        {
                            x -= _speed;
                        }
                    }

                    bool attacking = false;
                    if (_player.GetState() != Player.State.HIDING && ((_player.x < this.x && _mirrorX) || _player.x > this.x && !_mirrorX))
                    { 
                        collisions = _currentWeapon.GetCollisions();
                        for (int i = 0; i < collisions.Length; i++)
                        {
                            if (collisions[i] is Collider)
                            {
                                if (collisions[i] == _player.getCollider())
                                {
                                    _currentState = State.ATTACKING;
                                    attacking = true;
                                    break;
                                }
                            }
                        }
                    }
                    if(!attacking && _currentState == State.ATTACKING)
                    {
                        _currentState = State.IDLE;
                    }

                    int value = 50;
                    if (_player.GetState() == Player.State.JUMPING || _player.GetState() == Player.State.FALLING)
                    {
                        value = 100;
                    }

                    if (_currentState != State.SLEEPING && Math.Floor(this.x / 800) == Math.Floor(_player.x / 800) && Math.Abs(this.y - _player.y) < value && Math.Abs(this.x - _player.x) < 200 && _player.GetState() != Player.State.HIDING)
                    {
                        //Detect player
                        if (_player.x > this.x && !_mirrorX && _currentState != State.ATTACKING)
                        {
                            _playerRight = true;
                            if (_attacked)
                            {
                                _timer4.Enabled = true;

                            }
                            else
                            {
                                _currentState = State.FOLLOWING;
                            }

                        }
                        else if (_player.x < this.x && _mirrorX && _currentState != State.ATTACKING)
                        {
                            _playerRight = false;
                            if (_attacked)
                            {
                                _timer4.Enabled = true;
                            }
                            else
                            {
                                _currentState = State.FOLLOWING;
                            }

                        }
                        else
                        {
                            if (_currentState == State.FOLLOWING)
                            {

                                _currentState = State.IDLE;
                            }
                            _attacked = false;
                        }
                    }
                    else
                    {

                        if (_currentState == State.FOLLOWING)
                        {
                            _currentState = State.IDLE;
                        }
                        _attacked = false;

                    }


                    //I HAVE TO EDIT THE NAME OF THE VARIABLE
                    if (test && _currentState != State.FOLLOWING)
                    {
                        test = false;
                        Random rnd = new Random();
                        _timer.Interval = Utils.Random(1000, 2500);
                        if (_currentState != State.ATTACKING && _colliding && !_attacked)
                        {



                            int _randomAction = Utils.Random(0, 5);
                            _status.SetVisible(false);

                            switch (_randomAction)
                            {
                                case 1:
                                case 2:
                                    _currentState = State.IDLE;

                                    int _randomStatus = Utils.Random(0, 2);

                                    if (_randomStatus == 0)
                                    {
                                        _status.SetVisible(true);
                                        _status.SetFrame(_randomStatus);
                                    }
                                    break;

                                case 3:
                                    _currentState = State.MOVING;
                                    if (_canFlip)
                                    {
                                        int _direction = Utils.Random(0, 2);

                                        if (_direction == 1)
                                        {
                                            _movingRight = true;
                                        }
                                        else
                                        {
                                            _movingRight = false;
                                        }
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

                    if (_colliderBox.HitTest(_player.GetWeapon()) && _player.GetWeapon().visible && Math.Floor(_player.x / 800) == _screenSection)
                    {
                        if(_player._mirrorX)
                        {
                            if(_player.x > this.x)
                            {
                                Attacked(_player.GetWeapon().GetDamage());
                                _player.GetWeapon().SetReturing();
                            }
                        }
                        else
                        {
                            Attacked(_player.GetWeapon().GetDamage());
                
                            _player.GetWeapon().SetReturing();
                        }

                    }
                    
                    else
                    {

                        _hitSprite.visible = false;
                    }

                    _currentWeapon.SetVisible(false, _mirrorX, 0, 10);
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
                    if (_currentState != State.ATTACKING && _currentState != State.DYING)
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
                        case State.FOLLOWING:
                            _status.SetVisible(true);
                            _status.SetFrame(2);
                            break;
                        case State.ATTACKING:
                            if (delay)
                            {

                                currentFrame = 2;
                                _currentWeapon.SetVisible(true, _mirrorX, 0, 10);
                                _player.Attacked(25);
                                delay = false;
                                _timer3.Enabled = true;


                            }
                            else
                            {
                                if (currentFrame > 0)
                                {
                                    currentFrame = 0;
                                }
                                else
                                {
                                    NextFrame();
                                }
                                _currentWeapon.SetVisible(false, _mirrorX, 0, 10);
                            }
                            _status.SetVisible(false);
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
        
            _attacked = true;

            _currentState = State.IDLE;
            _status.visible = false;
            if (_canFlip) //Check if the player is not in the border or is going to fall
            {
                if (_player.x > this.x)
                {
                    x -= 10;
                }
                else if (_player.x < this.x)
                {
                    x += 10;
                }
            }


            _lifePoints -= damage;
            _lifeBar.visible = true;
            _lifeBar.Update(_lifePoints);
            _timer2.Enabled = true;
            
            _hitSprite.visible = true;
            Sound _sound = new Sound("Data/Sounds/hit.ogg", false, false);
            _sound.Play();

            if (_player.x > this.x)
            {
                _movingRight = true;
            }
            else if (_player.x < this.x)
            {
                _movingRight = false;
            }

            if(_lifePoints <= 0)
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
