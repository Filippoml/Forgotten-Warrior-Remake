using System;
using System.Timers;
using GXPEngine.GXPEngine;

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

        private bool _movingRight = false, _playerRight = false, _canFlip = true;

        private System.Timers.Timer _timer;
        private Status _status;

        private Ray _colliderBox, _colliderBox2;

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
            FOLLOWING
        }

        /// <summary>
        /// Defines the _currentState
        /// </summary>
        private State _currentState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Swordman"/> class.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// 
        bool test = true;
        public Swordman(float x, float y) : base("Data/swordman.png", 2, 2)
        {
            SetScaleXY(1.2f);
            this.x = x;
            this.y = y;
            currentFrame = 1;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Interval = 100;
            _timer.Enabled = true;

            _status = new Status();
            AddChild(_status);

            _screenSection = Convert.ToInt32(Math.Floor(this.x / 800));


            _colliderBox = new Ray("Data/HitBox.png");
            _colliderBox2 = new Ray("Data/HitBox2.png");
            _colliderBox2.SetOrigin(-20, -20);
            AddChild(_colliderBox);
            AddChild(_colliderBox2);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            test = true;

        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
          


            _counter++;

            _colliding = false;


            // TODO: use this instead:
            GameObject[] collisions = _colliderBox.GetCollisions();
            for (int i = 0; i < collisions.Length; i++)
            {

                if (collisions[i] is Tile)
                {

                    Tile _tile = collisions[i] as Tile;
                    //Check if player is colliding with ground tiles
                    if (_tile.GetId() >= 1 && _tile.GetId() <= 3)
                        {
                        y = _tile.y - this.height + 2;
                        _colliding = true;
                        }
                        if(_screenSection != Convert.ToInt32(Math.Floor(this.x / 800))){
                            _movingRight = !_movingRight;
                        }

                        if(_tile.GetId() == 1 || _tile.GetId() == 3 || _screenSection != Convert.ToInt32(Math.Floor(this.x / 800)))
                        {
                            
                        if (_canFlip)
                            {
                                _canFlip = false;
                                _movingRight = !_movingRight;
                            }
                        }
                    if (_currentState == State.ATTACKING)
                    {
                        _currentState = State.IDLE;
                    }


                }
                else if(collisions[i] is Player)
                {
                    _currentState = State.ATTACKING;
                }
     
            }

            collisions = _colliderBox2.GetCollisions();
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i] is Ray)
                {
                    if (collisions[i] == ((MyGame)game).Player.getCollider())
                    {
                        _currentState = State.ATTACKING;
                    }
                }
            }


                if (_currentState != State.SLEEPING && Math.Floor(this.x / 800) == Math.Floor(((MyGame)game).Player.x / 800) && Math.Abs(this.y - ((MyGame)game).Player.y) < 100)
            {
                //Detect player
                if (((MyGame)game).Player.x > this.x && !_mirrorX && _currentState != State.ATTACKING)
                {
                    _playerRight = true;
                    _currentState = State.FOLLOWING;
                }
                else if (((MyGame)game).Player.x < this.x && _mirrorX && _currentState != State.ATTACKING)
                {
                    _playerRight = false;
                    _currentState = State.FOLLOWING;
                }
                else
                {
                    if (_currentState == State.FOLLOWING)
                    {
                        _currentState = State.IDLE;
                    }
                }
            }
            else
            {
       
                if(_currentState == State.FOLLOWING)
                {
                    _currentState = State.IDLE;
                }
                
 
            }

            
            //I NEED TO EDIT THE NAME OF THE VARIABLE
            if (test && _currentState  != State.FOLLOWING) {
                test = false;
                Random rnd = new Random();
                _timer.Interval = rnd.Next(250, 1250);
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

                            if(_randomStatus == 0) { 
                                _status.SetVisible(true);
                                _status.SetFrame(_randomStatus);
                            }
                            break;

                        case 3:
                            _currentState = State.MOVING;
                            int _direction = rnd.Next(0, 2);
                            _canFlip = true;
                            if (_direction == 1)
                            {
                                _movingRight = false;
                            }
                            else
                            {
                                _movingRight = false;
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
            else if(_currentState == State.FOLLOWING)
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


            if (_counter == (60 / _frameRate))
            {
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
                if (_currentState != State.ATTACKING)
                {
                    _frameRate = 12;
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
                        Console.WriteLine(currentFrame);
                        _frameRate = 8;
                        if (currentFrame == 2)
                        {
                            currentFrame = 1;
                        }
                        else
                        {
                            currentFrame = 2;
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
}
