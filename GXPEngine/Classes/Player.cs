using System;

namespace GXPEngine.Classes
{
    /// <summary>
    /// Defines the <see cref="Player" />
    /// </summary>
    internal class Player : AnimationSprite
    {
        /// <summary>
        /// Defines the counter
        /// </summary>
        internal int counter = 0;

        /// <summary>
        /// Defines the frameRate
        /// </summary>
        internal int frameRate = 12;

        /// <summary>
        /// Defines the speed
        /// </summary>
        internal int speed = 3;

        /// <summary>
        /// Defines the _velocity
        /// </summary>
        private float _velocity = 0;

        /// <summary>
        /// Defines the test
        /// </summary>
        internal ElapsedGameTime test;

        /// <summary>
        /// Defines the _moving, _colliding, _jumping
        /// </summary>
        private bool _moving = false, _colliding = false, _jumping = false;

        private Weapon _currentWeapon;
        /// <summary>
        /// Defines the State
        /// </summary>
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
            ATTACKING
        }

        /// <summary>
        /// Defines the _currentState
        /// </summary>
        private State _currentState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player() : base("Data/player.png", 2, 8)
        {
            SetFrame(9);
            SetScaleXY(1.2f);
            currentFrame = 0;
            test = new ElapsedGameTime();
            x = 100;
            y = 100;

            _currentWeapon = new Weapon();
            AddChild(_currentWeapon);
        }

        /// <summary>
        /// The SetState
        /// </summary>
        /// <param name="state">The state<see cref="State"/></param>
        public void SetState(State state)
        {
            _currentState = state;
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            float old_x = x;
            float old_y = y;



            if (_currentState == State.MOVING)
            {
                SetState(State.IDLE);
            }

            counter++;


            //if (!_colliding)
            //{
            if (Input.GetKey(Key.A))
            {
                x = x - speed;

                if (!_mirrorX)
                {
                    Mirror(true, false);
                }

                if (_currentState == State.IDLE)
                {
                    SetState(State.MOVING);
                }



                if (x % game.width <= 3)
                {
                    game.Translate(800, 0);
                }
            }
            if (Input.GetKey(Key.D))
            {
                x = x + speed;

                if (_mirrorX)
                {
                    Mirror(false, false);
                }

                if (_currentState == State.IDLE)
                {
                    SetState(State.MOVING);
                }
                Console.WriteLine(x % game.width);
                
                if(x % game.width >= 799)
                {
                    game.Translate(-800, 0);
                }

            }
            if (Input.GetKey(Key.W))
            {

            }
            if (Input.GetKey(Key.S))
            {


            }
            if (Input.GetKey(Key.SPACE) && _colliding)
            {

                _currentState = State.JUMPING;
                _velocity = 5;

            }
            if(Input.GetMouseButtonDown(0) && (_currentState == State.IDLE || _currentState == State.MOVING) && _currentState != State.ATTACKING)
            {
                _currentState = State.ATTACKING;

            }

            //}

            if (counter == (60 / frameRate))
            {

                _currentWeapon.SetVisible(false, false);
                switch (_currentState)
                {
                    case State.IDLE:
                        if (currentFrame > 0)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            NextFrame();
                        }
                        break;
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
                    case State.JUMPING:
                            currentFrame = 6;
                        break;
                    case State.FALLING:
                            currentFrame = 7;
                        break;
                    case State.ATTACKING:
                        currentFrame = 4;
                        _currentWeapon.SetVisible(true, _mirrorX);
                        _currentState = State.IDLE;
                        break;
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

                        if (MyGame.Id_Tiles[i] >= 1 && MyGame.Id_Tiles[i] <= 3)
                        {
                            _colliding = true;
                        }
                        else
                        {
                            _colliding = false;
                        }



                    }
                    else
                    {
                        
                    }
                }
            }





            if (_currentState == State.JUMPING)
            {
                _velocity -= 0.3f;
                y -= _velocity;
            }

            if (_velocity < 0 && _currentState == State.JUMPING)
            {
                _currentState = State.IDLE;
            }

            if (_colliding && _currentState != State.ATTACKING)
            {
                _currentState = State.IDLE;
            }
            
            if(!_colliding)
            {
                if (_currentState != State.JUMPING)
                {
                    _velocity += 0.2f;
                    y += _velocity;
                    _currentState = State.FALLING;

                }
            }



            if (y > 500)
            {
                x = 100;
                y = 100;
                _velocity = 0;
            }
        }
    }
}
