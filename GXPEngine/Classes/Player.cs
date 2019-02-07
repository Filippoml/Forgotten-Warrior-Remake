using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using GXPEngine.Core;
using GXPEngine.Properties;

namespace GXPEngine.Classes
{
    public class Player : AnimationSprite
    {
        private readonly int _frameRate, _yClimbdone;
        private int _frameCounter,  _yClimb, _lifePoints, _manaPoints, _coinsNumber, _specialPowerIndex, _time_passed;

        private readonly float _speed;
        private float _yVelocity, _stairs_x, _hideX, _xRelativePosition;

        private bool _grounding, _canClimb, _attacked, _canHide, _canBuy, _caPressWToClimb, _collidingWithStairs, _specialPowerInUse;

        private Collider _colliderBox;

        private Weapon _currentWeapon;

        private Sprite _hitSprite, _circlePotionEffect, _ray;

        private HUD _hud;

        private Items _items;

        private GameObject[] _collisions;

        private readonly List<Sprite> _spritesSpecialPower;

        private State _currentState;

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

        public Player(float x, float y, bool flipped) : base("Data/AnimationSprites/player.png", 2, 8)
        {
            this.x = x;
            //32 is the height of the level tile 
            this.y = y - 32 - this.height;
            _mirrorX = flipped;

            //Init
            SetScaleXY(1.2f);
            currentFrame = 0;

            _coinsNumber = Data.Default.coins;
            _lifePoints = Data.Default.lifepoints;
            _manaPoints = Data.Default.manapoints;

            _frameRate = 12;
            _speed = 3;

            //Creation Child Objects
            _currentWeapon = new Weapon();
            _currentWeapon.SetWeapon(Data.Default.weapon);
            AddChild(_currentWeapon);

            _colliderBox = new Collider("Data/Sprites/HitBox.png", this);
            _colliderBox.SetOrigin(-12, 0);
            AddChild(_colliderBox);

            _hitSprite = new Sprite("Data/Sprites/hit.png")
            {
                visible = false
            };
            _hitSprite.SetScaleXY(0.8f);
            _hitSprite.SetXY(15, 15);
            AddChild(_hitSprite);

            //Visible when pressing 1 or 2
            _circlePotionEffect = new Sprite("Data/Sprites/red_circle.png")
            {
                visible = false,
                x = 2
            };
            _circlePotionEffect.y = height - (_circlePotionEffect.height * 2);
            AddChild(_circlePotionEffect);

            _ray = new Sprite("Data/Sprites/ray.png")
            {
                y = this.height,
                x = this.height / 2,
                visible = false
            };
            AddChild(_ray);

            //End of stairs
            _yClimbdone = 110;

            string path = "Data/Items.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(Items));
            StreamReader reader = new StreamReader(path);
            _items = (Items)serializer.Deserialize(reader);
            reader.Close();

            //TODO change with dynamic array
            _spritesSpecialPower = new List<Sprite>();
        }

        void Update()
        {
            Console.WriteLine(_yClimb);
            if (!((MyGame)game).IsPaused() && currentFrame != 15)
            {
                _frameCounter++;

                if (_specialPowerInUse)
                {
                    SpecialPower();
                }

                circlePotionEffect();

                if (!(_specialPowerInUse))
                {
                    keyHandler();
                }
                animate();

                _grounding = false;
                _canClimb = false;
                _caPressWToClimb = true;
                _canHide = false;
                _canBuy = false;
                _collidingWithStairs = false;

                checkCollisions();
                checkJumping();

                if (_yVelocity < 0 && _currentState == State.JUMPING)
                {
                    _currentState = State.IDLE;
                }

                if (_grounding && _currentState == State.FALLING)
                {
                    _currentState = State.IDLE;
                }

                applyGravity();

                //Check if player is dead
                if ((y > 700 || _lifePoints <= 0) && currentFrame != 15)
                {
                    Sound _sound = new Sound("Data/Sounds/died.wav", false, false);
                    _sound.Play();
                    //Setting Died frame
                    currentFrame = 15;

                    ((MyGame)game).PlayerDied();
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

                    if ((_tile.GetId() == 20 && (_yClimb < _yClimbdone)) || (_tile.GetId() == 20 && _currentState == State.IDLE))
                    {
                        _stairs_x = _tile.x - 12;
                        _canClimb = true;
                    }

                    if (_tile.GetId() == 20)
                    {
                        _collidingWithStairs = true;
                    }

                    //Check if player is colliding with ground tiles
                    if (((_tile.GetId() >= 1 && _tile.GetId() <= 3) || (_tile.GetId() >= 7 && _tile.GetId() <= 9) || (_tile.GetId() >= 13 && _tile.GetId() <= 15)) && _currentState != State.JUMPING && _currentState != State.CLIMBING)
                    {
                        float old_y = y - (_tile.y - height + 12);

                        if (old_y < 10)
                        {
                            //12 is because I need the player a little bit lower
                            y = _tile.y - height + 12;
                            _grounding = true;
                        }
                    }

                    //Tile without text
                    if(_tile.GetId() == 33)
                    {
                        //Center player
                        _hideX = _tile.x - (_tile.width / 4);
                        _canHide = true;
                    }
                    //Shop
                    else if (_tile.GetId() == 34)
                    {
                        //Center player
                        _hideX = _tile.x - (_tile.width / 4);
                        _canHide = true;
                        _canBuy = true;
                    }
                }
                else if (_collisions[i] is Coin)
                {
                    Coin _collision = _collisions[i] as Coin;
                    _coinsNumber += 10;
                    Sound _introSound = new Sound("Data/Sounds/coin.wav", false, false);
                    _introSound.Play();
                    _collision.Destroy();
                }
                
                else if (_collisions[i] is Trigger)
                {
                    Data.Default.healthpotions = _hud.GetHealthPotionsNumber();
                    Data.Default.manapotions = _hud.GetManaPotionsNumber();
                    Data.Default.coins = _coinsNumber;
                    Data.Default.lifepoints = _lifePoints;
                    Data.Default.manapoints = _manaPoints;
                    Data.Default.weapon = _currentWeapon.GetWeapon();

                    Sound _sound = new Sound("Data/Sounds/level_completed.wav", false, false);
                    _sound.Play();

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
                            break;
                        }
                    }
                }
            }

            //Check if player is not climbing and ray is not colliding
            if (_currentState != State.CLIMBING && _caPressWToClimb && !_collidingWithStairs)
            {
                //Reset yClimb
                _yClimb = 0;
            }

            if (!_caPressWToClimb && _currentState != State.JUMPING && _currentState != State.FALLING)
            {
                _yClimb = _yClimbdone;
            }
        }

        private void animate()
        {
            //Check framerate
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
                        //Center player position with stairs x
                        x = _stairs_x;
                        break;
                    case State.HIDING:
                        currentFrame = 12;
                        break;
                }

                //Damaged flashing effect
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
            if ((Input.GetKey(Key.A) || Input.GetKey(Key.LEFT)) && _currentState != State.CLIMBING && !Input.GetKey(Key.D) && _currentState != State.HIDING && x > 0)
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
  
            if ((Input.GetKey(Key.D) || Input.GetKey(Key.RIGHT)) && _currentState != State.CLIMBING && !Input.GetKey(Key.A) && _currentState != State.HIDING && x < 1600)
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
            

            if ((Input.GetKey(Key.W) || Input.GetKey(Key.UP)) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {
                if (_canClimb && _yClimb < _yClimbdone && _caPressWToClimb)
                {
                    _currentState = State.CLIMBING;
                    y -= 1.5f;
                    _yClimb++;
                }

                //End of climbing
                if (_yClimb == _yClimbdone)
                {
                    _currentState = State.IDLE;
                }

                if (_canHide && _currentState != State.HIDING)
                {
                    this.x = _hideX + 2;
                    Mirror(false, false);
                    _currentState = State.HIDING;
                    if (_canBuy)
                    {
                        ((MyGame)game).GetLevel().GetShop().Show();
                    }
                }
            }

            if ((Input.GetKey(Key.S) || Input.GetKey(Key.DOWN)) && _currentState != State.FALLING && _currentState != State.JUMPING)
            {
                if (_canClimb && _yClimb > 0)
                {
                    _yClimb--;
                    _currentState = State.CLIMBING;
                    y += 1.5f;
                }
                else if (!_canBuy)
                {
                    _currentState = State.IDLE;
                }
            }
            if (Input.GetKey(Key.SPACE) && _grounding && _currentState != State.CLIMBING && _currentState != State.HIDING)
            {
                Sound _introSound = new Sound("Data/Sounds/jump.wav", false, false);
                _introSound.Play();
                _currentState = State.JUMPING;
                _yVelocity = 5;
            }
            if (Input.GetMouseButtonDown(0) && (_currentState == State.IDLE || _currentState == State.MOVING) && _currentState != State.ATTACKING && _currentState != State.CLIMBING && _currentWeapon.visible == false)
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

            if (Input.GetKeyDown(Key.THREE) && _manaPoints >= 33)
            {
                _specialPowerInUse = true;

                //Cost of superpower
                _manaPoints -= 33;
                _hud.UpdateManaBar(_manaPoints);
            }
        }

        public Collider getCollider()
        {
            return _colliderBox;
        }
        
        public void Attacked (int damage)
        {
            if(_lifePoints - damage < 0)
            {
                _lifePoints = 0;
            }
            else
            {
                _lifePoints -= damage;
            }

            if(_lifePoints > 0)
            {
                visible = false;
                _hitSprite.visible = true;
                Sound _sound = new Sound("Data/Sounds/hit.ogg", false, false);
                _sound.Play();
            }

            _hud.UpdateLifeBar(_lifePoints);
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
                _hud.UpdateLifeBar(_lifePoints);
                _circlePotionEffect.y = height - (_circlePotionEffect.height * 2);

                _circlePotionEffect.texture = new Texture2D("Data/Sprites/red_circle.png");
                _circlePotionEffect.visible = true;
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

                _hud.UpdateManaBar(_manaPoints);
                _circlePotionEffect.y = height - (_circlePotionEffect.height * 2);

                _circlePotionEffect.texture = new Texture2D("Data/Sprites/blue_circle.png");
                _circlePotionEffect.visible = true;
            }
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

        public void SetManaPotionsNumber(bool increment)
        {
            _hud.SetManaPotionsNumber(increment);
        }

        private void SpecialPower()
        {
            if (_time_passed == 0)
            {
                Sprite _blueWave = new Sprite("Data/Sprites/blue_wave.png");

                if (_mirrorX)
                {
                    _blueWave.x = _blueWave.width * (-_specialPowerIndex);
                    _blueWave.SetOrigin(20, 0);
                }
                else
                {
                    _blueWave.x = _blueWave.width * _specialPowerIndex;
                    _blueWave.SetOrigin(-40, 0);

                }

                _xRelativePosition = _blueWave.x + this.x;
                _blueWave.y = 15;

                _spritesSpecialPower.Add(_blueWave);

                AddChild(_blueWave);

                _specialPowerIndex++;
            }

            if (Math.Floor((_xRelativePosition) / 800) != Math.Floor(this.x / 800))
            {
                //Delete wave after some time
                if (_time_passed >= 200)
                {
                    foreach (Sprite sprite in _spritesSpecialPower)
                    {
                        if (sprite != null)
                        {
                            sprite.Destroy();
                        }
                    }
                    _specialPowerInUse = false;
                    _specialPowerIndex = 0;
                    _time_passed = 0;
                }
                if (_specialPowerInUse)
                {
                    _time_passed += Time.deltaTime;
                }
            }
        }

        private void circlePotionEffect()
        {
            if (_circlePotionEffect.y >= 0 && _circlePotionEffect.visible)
            {
                _circlePotionEffect.y--;
            }
            else
            {
                _circlePotionEffect.visible = false;
            }
        }

        public int GetYClimb()
        {
            return _yClimb;
        }
    }
}
