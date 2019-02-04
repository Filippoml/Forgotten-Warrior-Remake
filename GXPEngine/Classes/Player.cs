using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using GXPEngine.Core;
using GXPEngine.Properties;

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
        private int _frameCounter, _frameRate, _yClimb, _lifePoints, _manaPoints, _yClimbdone, _coinsNumber; 

        private float _speed, _yVelocity, _stairs_x, _hideX;

        private bool _grounding, _canClimb, _attacked, _canHide, _canBuy, _caPressWToClimb;

        private Collider _colliderBox;

        private Weapon _currentWeapon;

        GameObject[] _collisions;

        private Sprite _hitSprite, _redCircle, _ray;

        private HUD _hud;

        private Items _items;

        public enum State
        {
            IDLE,
            MOVING,
            JUMPING,
            FALLING, 
            ATTACKING,
            CLIMBING,
            HIDING
        }

        private State _currentState;


        public Player(float x, float y) : base("Data/player.png", 2, 8)
        {
            this.x = x;
            this.y = y;

            //Init
            SetScaleXY(1.2f);
            currentFrame = 0;
            _lifePoints = 50;
            _manaPoints = 50;
            _frameRate = 12;
            _speed = 3;

            //Creation Child Objects
            _currentWeapon = new Weapon();
            _currentWeapon.SetWeapon(0);
            AddChild(_currentWeapon);

            _colliderBox = new Collider("Data/HitBox.png", this);
            _colliderBox.SetOrigin(-12, 0);
            AddChild(_colliderBox);

            _hitSprite = new Sprite("Data/hit.png");
            _hitSprite.visible = false;
            _hitSprite.SetScaleXY(0.8f);
            _hitSprite.SetXY(15, 15);
            AddChild(_hitSprite);

            _redCircle = new Sprite("Data/red_circle.png");
            _redCircle.visible = false;
            _redCircle.x = 2;
            _redCircle.y = height - (_redCircle.height * 2);
            AddChild(_redCircle);

            _ray = new Sprite("Data/ray.png");
            _ray.y = this.height;
            _ray.x = this.height / 2;
            _ray.visible = false;
            AddChild(_ray);

            _yClimbdone = 110;

            string path = "Data/Items.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(Items));

            StreamReader reader = new StreamReader(path);
            _items = (Items)serializer.Deserialize(reader);
            reader.Close();


            
            /*GXPEngine.Properties.Data.Default.weapon = "prova";
            GXPEngine.Properties.Data.Default.Save();
            */
        }




        void Update()
        {
            if (!((MyGame)game).IsPaused())
            {
                



                if (_redCircle.y >= 0 && _redCircle.visible)
                {
                    _redCircle.y--;
                }
                else
                {
                    _redCircle.visible = false;
                }

                //???
                // TODO: structure this code with a switch case based on state (easier to understand, though it probably introduces some code duplication)
                if (_currentState == State.MOVING && _currentState != State.CLIMBING)
                {

                    _currentState = State.IDLE;


                    //TODO why?
                    _yClimb = 0;

                }

                _frameCounter++;



                keyHandler();
                Animate();

                _grounding = false;
                _canClimb = false;
                _caPressWToClimb = true;
                _canHide = false;
                _canBuy = false;

                checkCollisions();
                checkJumping();

                if (_yVelocity < 0 && _currentState == State.JUMPING)
                {
                    _currentState = State.IDLE;
                }

                //Maybe I've to add _currentState == State.JUMPING
                if (_grounding && _currentState == State.FALLING)
                {
                    _currentState = State.IDLE;
                }

                applyGravity();

                //Respawn: just for testing
                if (y > 500)
                {
                    x = 100;
                    y = 400;
                    _yVelocity = 0;
                }
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
                        float old_y = y - (_tile.y - height + 12);

                        if (old_y < 10)
                        {
                            y = _tile.y - height + 12;
                            _grounding = true;
                        }
                    }

                    if(_tile.GetId() == 22)
                    {
                        _hideX = _tile.x;
                        _canHide = true;

                    }

                    if (_tile.GetId() == 34)
                    {
                        _canBuy = true;
                    }
                }
                else if (_collisions[i] is Coin)
                {
                    Coin _collision = _collisions[i] as Coin;
                    _coinsNumber += 10;
                    _collision.Destroy();
                }

                else if (_collisions[i] is Trigger)
                {
                    GXPEngine.Properties.Data.Default.healthpotions = 10;
                    GXPEngine.Properties.Data.Default.Save();
                  ((MyGame)game).ChangeLevel();
                }
            }

            if (_currentState != State.CLIMBING)
            {
                _collisions = _ray.GetCollisions();
                for (int i = 0; i < _collisions.Length; i++)
                {
                    if (_collisions[i] is Tile)
                    {
                        Tile _tile = _collisions[i] as Tile;


                        if (_tile.GetId() == 20)
                        {
                            _caPressWToClimb = false;
                            i = _collisions.Length;

                            //TODO maybe can be replaced with return
                        }
                    }
                }
            }

            if(!_caPressWToClimb)
            {
                _yClimb = _yClimbdone;
            }
        }

        private void Animate()
        {
            if (_frameCounter == (60 / _frameRate))
            {

                if(_currentWeapon.GetWeapon() == 0)
                {
                    _currentWeapon.SetVisible(false, false, 0, 0);
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

                    case State.JUMPING:
                        currentFrame = 6;
                        break;
                    case State.FALLING:
                        currentFrame = 7;
                        break;
                    case State.ATTACKING:
                        currentFrame = 4;

                        if (!(_currentWeapon.visible))
                        {
              
                      
                            if(_currentWeapon.GetWeapon() == 0)
                            {
                                _currentWeapon.SetOrigin(0, 0);
                                _currentWeapon.SetVisible(true, _mirrorX, 0, 20);
                                
                            }
                            else
                            {
                                _currentWeapon.SetOrigin(-12, 0);
                                _currentWeapon.SetVisible(true, _mirrorX, -30, 30);
                            }

                        }
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
                                    //_swordman.Attacked(0);
                                    i = _collisions.Length;
                                }
                                else if (_collision.getOwner().GetType() == typeof(Wizard))
                                {
                                    Wizard wizard = _collision.getOwner() as Wizard;
                                    wizard.Attacked(25);
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
                    case State.HIDING:
                        currentFrame = 12;
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

        private void keyHandler()
        {
            if (Input.GetKey(Key.A) && _currentState != State.CLIMBING && !Input.GetKey(Key.D) && _currentState != State.HIDING && x > 0)
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
  
            if (Input.GetKey(Key.D) && _currentState != State.CLIMBING && !Input.GetKey(Key.A) && _currentState != State.HIDING)
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
                float test = ((float)_yClimb / (float)_yClimbdone) ;



                if (_canClimb && _yClimb < _yClimbdone && _caPressWToClimb)
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
                

                if (_canHide)
                {
                    this.x = _hideX + 2;
                    Mirror(false, false);
                    _currentState = State.HIDING;
                    if (_canBuy)
                    {
                        ((MyGame)game).GetLevel().ShowShop(true);

                    }
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
                    else if(!_canBuy)
                    {
                        _currentState = State.IDLE;
                    }
            }
            if (Input.GetKey(Key.SPACE) && _grounding && _currentState != State.CLIMBING && _currentState != State.HIDING)
            {
                _currentState = State.JUMPING;
                _yVelocity = 5;

            }
            if (Input.GetMouseButtonDown(0) && (_currentState == State.IDLE || _currentState == State.MOVING) && _currentState != State.ATTACKING && _currentState != State.CLIMBING)
            {
                _currentState = State.ATTACKING;
            }

            if(Input.GetKeyDown(Key.ONE))
            {
                if (_lifePoints < 100)
                {
                    int _healthPotionValue = Convert.ToInt32(_items.Item[3].Value);
                    incrementLife(_healthPotionValue);
                    _hud.SetHealthPotionsNumber(false);
                }
            }

            if (Input.GetKeyDown(Key.TWO))
            {
                if (_manaPoints < 100)
                {
                    int _manaPotionValue = Convert.ToInt32(_items.Item[4].Value);
                    incrementMana(_manaPotionValue);
                    _hud.SetManaPotionsNumber(false);
                }
            }

            if (Input.GetKeyDown(Key.THREE))
            {
                int _manaPotionValue = -20;
                incrementMana(_manaPotionValue);
                _hud.SetManaPotionsNumber(false);
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

        private void incrementLife(int value)
        {
            if (_hud.GetHealthPotionsNumber() > 0)
            {
                if (_lifePoints + value > 100)
                {
                    _lifePoints = 100;
                }
                else
                {
                    _lifePoints += value;
                }
                _redCircle.y = height - (_redCircle.height * 2);

                _redCircle.texture = new Texture2D("Data/red_circle.png");
                _redCircle.visible = true;


            }


        }
        
        private void incrementMana(int value)
        {
            if (_hud.GetManaPotionsNumber() > 0)
            {
                if (_manaPoints + value > 100)
                {
                    _manaPoints = 100;
                }
                else
                {
                    _manaPoints += value;
                }
                _redCircle.y = height - (_redCircle.height * 2);

                _redCircle.texture = new Texture2D("Data/blue_circle.png");
                _redCircle.visible = true;

            }
        }

        public int GetLifePoints()
        {
            return _lifePoints;
        }

        public int GetManaPoints()
        {
            return _manaPoints;
        }

        public int GetCoinsNumber()
        {
            return _coinsNumber;
        }

        public void SetCoinsNumber(bool add, int value)
        {
            if(add)
            {
                _coinsNumber += value;
            }
            else
            {
                _coinsNumber -= value;
            }

        }

        public void SetState(State state)
        {
            _currentState = state;
        }

        public Weapon GetWeapon()
        {
            return _currentWeapon;
        }

        public State GetState()
        {
            return _currentState;
        }

        public void LoadHUD()
        {
            _hud = ((MyGame)game).GetHud();
        }

        public void SetHealthPotionsNumber(bool increment)
        {
            _hud.SetHealthPotionsNumber(increment);
        }

        public void SetManaPotionsNumber(bool increment)
        {
            _hud.SetManaPotionsNumber(increment);
        }
    }
}
