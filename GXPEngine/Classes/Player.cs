using System;
using System.Drawing;
using GXPEngine.GXPEngine;

namespace GXPEngine.Classes
{
    /// <summary>
    /// Defines the <see cref="Player" />
    /// </summary>
    public class Player : AnimationSprite
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
        /// Defines the _moving, _grounding, _jumping
        /// </summary>
        private bool _grounding = false, _canClimb = false;

        private float _stairs_x;

        private Ray _colliderBox;

        private Weapon _currentWeapon;

        private bool _wasMovingRight = false, _wasMovingLeft = false;

        private LifeBar _lifeBar;

        private int _lifePoints;

        GameObject[] _collisions;


        private Sprite _hit_sprite;
        /// <summary>
        /// Defines the State
        /// </summary>
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

            _colliderBox = new Ray("Data/HitBox.png", this);
            _colliderBox.SetOrigin(-12, 0);
            AddChild(_colliderBox);

             _lifeBar = new LifeBar();
            AddChild(_lifeBar);

            _lifePoints = 100;

            _hit_sprite = new Sprite("Data/hit.png");
            AddChild(_hit_sprite);
            _hit_sprite.visible = false;
            _hit_sprite.SetScaleXY(0.8f);
        }
        float old_y = 0;
        int e = 0;
        bool up = false, down = false;
        /// <summary>
        /// The SetState
        /// </summary>
        /// <param name="state">The state<see cref="State"/></param>


        /// <summary>
        /// The Update
        /// </summary>
        void Update()
        {



            // TODO: structure this code with a switch case based on state (easier to understand, though it probably introduces some code duplication)
            if (_currentState == State.MOVING && _currentState != State.CLIMBING)
            {

                _currentState = State.IDLE;
            }

            float old_x = x;
    


            counter++;


            keyMovement();

            // TODO: put this in an Animate method:
            if (counter == (60 / frameRate))
            {
                _hit_sprite.visible = false;
                _currentWeapon.SetVisible(false, false, -10, 10);
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
                        _currentWeapon.SetWeapon(0);
                        _currentWeapon.SetVisible(true, _mirrorX, -10, 10);
                        _currentState = State.IDLE;

                        _collisions = _currentWeapon.GetCollisions();
                        for (int i = 0; i < _collisions.Length; i++)
                        {
                            if (_collisions[i] is Ray)
                            {
                                Ray _collision = _collisions[i] as Ray;
                                if(_collision.getOwner().GetType() == typeof(Swordman))
                                {
                                    if(_collision.getOwner() is Swordman)
                                    {
                                        Swordman _swordman = _collision.getOwner() as Swordman;
                                        _swordman.Attacked(50);
                                        _hit_sprite.visible = true;
                                        if (_mirrorX)
                                        {
                                            _hit_sprite.SetXY(-20, 15);
                                        }
                                        else
                                        {
                                            _hit_sprite.SetXY(40, 15);
                                        }
                                    }
                                    
                                }

                                
                                
                            }
                        }
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
            
            _grounding = false;

            _canClimb = false;

            // TODO: use this instead:
            _collisions = _colliderBox.GetCollisions();
            
            for (int i = 0; i < _collisions.Length; i++)
            {
  
                if (_collisions[i] is Tile)
                {
                    Tile _tile = _collisions[i] as Tile;
                    if ((_tile.GetId() == 7 && (e < 94)) || (_tile.GetId() == 7 && _currentState == State.IDLE))
                    {
                        _stairs_x = _tile.x - 12;

                        _canClimb = true;
                    }

         

                    //Check if player is colliding with ground tiles
                    if (_tile.GetId() >= 1 && _tile.GetId() <= 3 && _currentState != State.JUMPING && _currentState != State.CLIMBING)
                    {
                        _grounding = true;
                        
                        y = _tile.y - height + 2;
                    }





                    if (_tile.GetId() == 5 && _currentState != State.IDLE && _currentState != State.MOVING)
                    {
                       //_grounding = false;
                    }



                    /*
                    if(_canClimb && !_grounding)
                    {
                        _currentState = State.CLIMBING;
                    }
                    */

                }
                    
                }
            



            if (_grounding) {
                old_y = y;
            }

            if (_currentState == State.JUMPING)
            {
                _velocity -= 0.2f;
                y -= _velocity;
            }

            if (_velocity < 0 && _currentState == State.JUMPING)
            {
                _currentState = State.IDLE;
            }

            if (_grounding && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.IDLE;

            }

            

            //Gravity
            if (!_grounding)
            {
                if (_currentState != State.JUMPING && _currentState != State.CLIMBING)
                {

                    if (_velocity < 8)
                    {
                        _velocity += 0.2f;
                    }
                    y += _velocity;
                    _currentState = State.FALLING;

                    if (_wasMovingLeft)
                    {
                        move(-speed, 0);
                    }
                    else if(_wasMovingRight)
                    {
                        move(speed, 0);
                    }
                    

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

            _lifeBar.Update(_lifePoints);
        }

        private void keyMovement()
        {
            if (Input.GetKey(Key.A) && _currentState != State.CLIMBING && _currentState != State.FALLING)
            {

                _wasMovingLeft = true;
                move(-speed, 0);
                    if (_currentState == State.IDLE)
                    {
                        _currentState = State.MOVING;
                    }

                    if (!_mirrorX)
                    {
                        Mirror(true, false);
                    }
         
                /*
                x = x - speed;



                if (_currentState == State.IDLE)
                {
                    _currentState = State.MOVING;
                }

                
                 
                 */
                // TODO: move this

                

            }
            else if (_currentState != State.FALLING)
            {
                _wasMovingLeft = false;
            }




            /*
                if (x % game.width <= 2 && _canScroll)
                {
                    _canScroll = false;
                    game.Translate(800, 0);
                }
                else if (x % game.width >= 797 && _canScroll)
                {
                    _canScroll = false;
                    game.Translate(-800, 0);
                }
                else
                {
                    _canScroll = true;
                }
            */
     
            if (Input.GetKey(Key.D) && _currentState != State.CLIMBING && _currentState != State.FALLING)
            {
                _wasMovingRight = true;
                move(speed, 0);
                if (_mirrorX)
                {
                    Mirror(false, false);
                }

                if (_currentState == State.IDLE)
                {
                    _currentState = State.MOVING;
                }

            }
            else if (_currentState != State.FALLING)
            {
                _wasMovingRight = false;
            }

            if (Input.GetKey(Key.W) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {
                if (_canClimb && e < 94)
                {
                    _currentState = State.CLIMBING;
                    y -= 1.5f;
                    e++;

                }

                if (e > 93)
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
            if (Input.GetKey(Key.SPACE) && _grounding && _currentState != State.CLIMBING)
            {
                /*if (Input.GetKey(Key.A)){
                    _wasMovingLeft = true;
                    _wasMovingRight = false;
                }
                else if (Input.GetKey(Key.D))
                {
                    _wasMovingLeft = false;
                    _wasMovingRight = true;
                }
                else
                {
                    _wasMovingLeft = false;
                    _wasMovingRight = false;
                }
                */
                _currentState = State.JUMPING;
                _velocity = 5;

            }
            if (Input.GetMouseButtonDown(0) && (_currentState == State.IDLE || _currentState == State.MOVING) && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.ATTACKING;
            }
        }


        protected void move(float moveX, float moveY)
        {

            x = x + moveX;
            y = y + moveY;
           
        }

        public Ray getCollider()
        {
            return _colliderBox;
        }
        
        public void Attacked (int damage)
        {
            _lifePoints -= damage;
        }
    }
}
