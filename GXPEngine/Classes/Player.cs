using System;
using System.Drawing;

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
        private int counter = 0;

        /// <summary>
        /// Defines the frameRate
        /// </summary>
        private int frameRate = 12;

        /// <summary>
        /// Defines the speed
        /// </summary>
        private int speed = 3;

        /// <summary>
        /// Defines the _velocity
        /// </summary>
        private float _velocity = 0;

        /// <summary>
        /// Defines the _moving, _colliding, _jumping
        /// </summary>
        private bool _colliding = false, _canClimb = false;

        private float _stairs_x;

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
            ATTACKING,
            CLIMBING
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
            
            x = 100;
            y = 1000;

            _currentWeapon = new Weapon();
            AddChild(_currentWeapon);

            
            
        }
        int e = 0;
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
        void Update()
        {


            if (_currentState == State.MOVING && _currentState != State.CLIMBING)
            {

                _currentState = State.IDLE;
            }

            counter++;
            

            if (Input.GetKey(Key.A) && _currentState != State.CLIMBING)
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



                if (x % game.width <= 2)
                {
                    game.Translate(800, 0);
                }
                
            }
   
            if (Input.GetKey(Key.D) && _currentState != State.CLIMBING)
            {
                x = x + speed;

                if (_mirrorX)
                {
                    Mirror(false, false);
                }

                if (_currentState == State.IDLE)
                {
                    _currentState = State.MOVING;
                }
                
                if(x % game.width >= 797)
                {
                    game.Translate(-800, 0);
                }
            }

            if (Input.GetKey(Key.W) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {
                if (_canClimb && e < 94)
                {
                    _currentState = State.CLIMBING;
                    y -= 1.5f;
                    e++;
                   
                }

                if(e > 93)
                {
                    _currentState = State.IDLE;
                }
                
            }

            if (Input.GetKey(Key.S) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {
               
                    if (_canClimb && e > 0)
                    {
                    e--;
                    _currentState = State.CLIMBING;
                        y += 1.5f;
                     
                    }
                else
                {
                    _currentState = State.IDLE;
                }
                
            }
            if (Input.GetKey(Key.SPACE) && _colliding && _currentState != State.CLIMBING)
            {

                _currentState = State.JUMPING;
                _velocity = 5;

            }
            if(Input.GetMouseButtonDown(0) && (_currentState == State.IDLE || _currentState == State.MOVING) && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.ATTACKING;
            }

            
            if (counter == (60 / frameRate))
            {

                _currentWeapon.SetVisible(false, false);
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
                    case State.CLIMBING:
                        if (currentFrame > 10 || currentFrame < 10)
                        {
                            currentFrame = 10;
                        }
                        else
                        {
                            NextFrame();
                        }
                        x = _stairs_x;
                        break;
                    
                }


                counter = 0;


            }
            
            _colliding = false;

            _canClimb = false;
            
            for (int i = 0; i < MyGame.Objects.Length; i++)
            {
                if (MyGame.Objects[i] != null)
                {
                    if (this.HitTest(MyGame.Objects[i]))
                    {
                        if ((MyGame.Id_Tiles[i] == 7 && (e < 94)) || (MyGame.Id_Tiles[i] == 7 && _currentState == State.IDLE))
                        {
                            _stairs_x = MyGame.Objects[i].x - 12;

                            _canClimb = true;


                        }


                        //Check if player is colliding with ground tiles
                        if (MyGame.Id_Tiles[i] >= 1 && MyGame.Id_Tiles[i] <= 3)
                        {
                            _colliding = true;
                        }

                        
                        
                        
                        if(MyGame.Id_Tiles[i] == 5)
                        {
                            _colliding = false;
                        }
                        



                        /*
                        if(_canClimb && !_colliding)
                        {
                            _currentState = State.CLIMBING;
                        }
                        */

    
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

            if (_colliding && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.IDLE;

            }

            

            //Gravity
            if (!_colliding)
            {
                if (_currentState != State.JUMPING && _currentState != State.CLIMBING)
                {
                    _velocity += 0.2f;
                    y += _velocity;
                    _currentState = State.FALLING;

                    

                    //Check if fallen on lower floor or jumped
                    if (_velocity > 5)
                    {
                        e = 0;
                    }

                }
            }



            if (y > 1100)
            {
                x = 100;
                y = 100;
                _velocity = 0;
            }
        }
    }
}
