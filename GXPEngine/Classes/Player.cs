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
        /// Defines the frameRate
        /// </summary>
        private int _frameCounter, _frameRate, _yClimb, _lifePoints, _yClimbdone;

        private float _speed, _yVelocity, _stairs_x;

        private bool _grounding, _canClimb, _attacked;

        private Collider _colliderBox;

        private Weapon _currentWeapon;

        GameObject[] _collisions;

        private Sprite _hitSprite;

        private enum State
        {
            IDLE,
            MOVING,
            JUMPING,
            FALLING, 
            ATTACKING,
            CLIMBING
        }

        private State _currentState;

        public Player(float x, float y) : base("Data/player.png", 2, 8)
        {
            this.x = x;
            this.y = y;

            //Init
            SetScaleXY(1.2f);
            currentFrame = 0;
            _lifePoints = 100;
            _frameRate = 12;
            _speed = 3;


            //Creation Child Objects
            _currentWeapon = new Weapon();
            AddChild(_currentWeapon);

            _colliderBox = new Collider("Data/HitBox.png", this);
            _colliderBox.SetOrigin(-12, 0);
            AddChild(_colliderBox);

            _hitSprite = new Sprite("Data/hit.png");
            _hitSprite.visible = false;
            _hitSprite.SetScaleXY(0.8f);
            _hitSprite.SetXY(15, 15);
            AddChild(_hitSprite);

            _yClimbdone = 110;
        }




        void Update()
        {
            //???
            // TODO: structure this code with a switch case based on state (easier to understand, though it probably introduces some code duplication)
            if (_currentState == State.MOVING && _currentState != State.CLIMBING)
            {

                _currentState = State.IDLE;
            }

            _frameCounter++;



            keyMovement();
            Animate();

            _grounding = false;
            _canClimb = false;


            checkCollisions();
            checkJumping();

            if (_yVelocity < 0 && _currentState == State.JUMPING)
            {
                _currentState = State.IDLE;
            }

            if (_grounding && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.IDLE;
            }

            applyGravity();

            //Respawn: just for testing
            if (y > 1100)
            {
                x = 100;
                y = 1000;
                _yVelocity = 0;
            }

        }

        private void checkJumping()
        {
             if (_currentState == State.JUMPING)
            {
                _yVelocity -= 0.2f;
                y -= _yVelocity;
            }
        }

        private void checkCollisions()
        {
            _collisions = _colliderBox.GetCollisions();

            for (int i = 0; i < _collisions.Length; i++)
            {
                if (_collisions[i] is Tile)
                {
                    Tile _tile = _collisions[i] as Tile;

                    //TODO check this condition
                    if ((_tile.GetId() == 20 && (_yClimb < _yClimbdone)) || (_tile.GetId() == 20 && _currentState == State.IDLE))
                    {
                        _stairs_x = _tile.x - 12;
                        _canClimb = true;
                    }

                    //Check if player is colliding with ground tiles
                    if (_tile.GetId() >= 1 && _tile.GetId() <= 3 && _currentState != State.JUMPING && _currentState != State.CLIMBING)
                    {
                        float old_y = y - (_tile.y - height + 9);

                        if (old_y < 10)
                        {
                            y = _tile.y - height + 9;
                            _grounding = true;
                        }
                    }
                }
            }

            Console.WriteLine(_yClimb);
        }

        private void Animate()
        {
            if (_frameCounter == (60 / _frameRate))
            {

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
                            if (_collisions[i] is Collider)
                            {
                                Collider _collision = _collisions[i] as Collider;

                                if (_collision.getOwner().GetType() == typeof(Swordman))
                                {

                                    Swordman _swordman = _collision.getOwner() as Swordman;
                                    _swordman.Attacked(50);
                                    i = _collisions.Length;


                                }
                                else if (_collision.getOwner().GetType() == typeof(Wizard))
                                {
                                    Wizard wizard = _collision.getOwner() as Wizard;
                                    wizard.Attacked(50);
                                    i = _collisions.Length;
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

                //Damaged effect
                if (!visible)
                {

                    visible = true;

                }
                else if (visible && _attacked)
                {
                    visible = false;
                    _attacked = false;
                    _hitSprite.visible = false;
                }

                _frameCounter = 0;
            }
        }

        private void applyGravity()
        {
            if (!_grounding)
            {
                if (_currentState != State.JUMPING && _currentState != State.CLIMBING)
                {
                    if (_yVelocity < 8)
                    {
                        _yVelocity += 0.2f;
                    }
                    y += _yVelocity;
                    _currentState = State.FALLING;
                    
                    //Check if fallen on lower floor or jumped
                    if (_yVelocity > 5)
                    {
                        _yClimb = 0;
                    }
                }
            }
        }

        private void keyMovement()
        {
            if (Input.GetKey(Key.A) && _currentState != State.CLIMBING && !Input.GetKey(Key.D))
            {
                Move(-_speed, 0);

                if (_currentState == State.IDLE)
                {
                    _currentState = State.MOVING;
                }

                if (!_mirrorX)
                {
                    Mirror(true, false);
                }
            }
  
            if (Input.GetKey(Key.D) && _currentState != State.CLIMBING && !Input.GetKey(Key.A))
            {
                Move(_speed, 0);
                if (_mirrorX)
                {
                    Mirror(false, false);
                }

                if (_currentState == State.IDLE)
                {
                    _currentState = State.MOVING;
                }
            }


            if (Input.GetKey(Key.W) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {
                if (_canClimb && _yClimb < _yClimbdone)
                {
                    _currentState = State.CLIMBING;
                    y -= 1.5f;
                    _yClimb++;
                }

                //TODO check this
                if (_yClimb == _yClimbdone)
                {
                    _currentState = State.IDLE;
                }

            }

            if (Input.GetKey(Key.S) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {

                if (_canClimb && _yClimb > 0)
                {
                    _yClimb--;
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
                _currentState = State.JUMPING;
                _yVelocity = 5;

            }
            if (Input.GetMouseButtonDown(0) && (_currentState == State.IDLE || _currentState == State.MOVING) && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.ATTACKING;
            }
        }

        public Collider getCollider()
        {
            return _colliderBox;
        }
        
        public void Attacked (int damage)
        {
            _hitSprite.visible = true;
            if(_lifePoints - damage < 0)
            {
                _lifePoints = 0;
            }
            else
            {
                _lifePoints -= damage;
            }

            visible = false;
            _attacked = true;
        }

        public int GetLifePoints()
        {
            return _lifePoints;
        }
    }
}
