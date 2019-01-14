using System;
using System.Timers;

namespace GXPEngine.Classes
{
    /// <summary>
    /// Defines the <see cref="Swordman" />
    /// </summary>
    internal class Swordman : AnimationSprite
    {
        /// <summary>
        /// Defines the _colliding
        /// </summary>
        private bool _colliding = false;

        private float _velocity = 0, _speed = 2;

        private int _counter = 0, _frameRate = 12;

        private bool _movingRight = false, _canFlip = true;

        private System.Timers.Timer _timer;
        private Status _status;

        public enum State
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
            SLEEPING
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


            for (int i = 0; i < MyGame.Objects.Length; i++)
            {
                if (MyGame.Objects[i] != null)
                {
                    if (this.HitTest(MyGame.Objects[i]))
                    {


                        //Check if player is colliding with ground tiles
                        if (MyGame.Id_Tiles[i] >= 1 && MyGame.Id_Tiles[i] <= 3)
                        {
                            _colliding = true;
                        }

                        if(MyGame.Id_Tiles[i] == 1 || MyGame.Id_Tiles[i] == 3)
                        {
                            if (_canFlip)
                            {
                                _canFlip = false;
                                _movingRight = !_movingRight;
                            }
                        }
                    }
                }
            }

            if (test) {
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
                                _movingRight = true;
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
            Console.WriteLine(_movingRight);

            if (_currentState == State.MOVING)
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



            if (_counter == (60 / _frameRate))
            {
                if (_movingRight)
                {
                    if (_mirrorX)
                    {
                        Mirror(false, false);
                    }
                }
                else
                {
                    if (!_mirrorX)
                    {
                        Mirror(true, false);
                    }
                }

                switch (_currentState)
                {
                    case State.IDLE:
                    case State.MOVING:
                    
                        if (currentFrame > 0)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            NextFrame();
                        }

                        break;
                    case State.SLEEPING:
                        _status.floating();
                        if (currentFrame > 0)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            NextFrame();
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
