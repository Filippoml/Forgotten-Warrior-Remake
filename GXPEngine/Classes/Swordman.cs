using System;
using System.Timers;


namespace GXPEngine.Classes
{
    public class Swordman : AnimationSprite
    {
        private readonly float _speed;
        private float _velocity;

        private int _counter, _lifePoints;
        private readonly int _frameRate, _screenSection;

        private bool _colliding, _movingRight, _playerRight, _canFlip, _delay, _attacked, _canGerateRandomAction;

        private System.Timers.Timer _timerRandomAction, _timerHideLifeBar, _timerCanAttack, _timerWaitBeforeFollowing;

        private Status _status;

        private Collider _colliderBox, _colliderBox2;

        private Weapon _currentWeapon;

        private LifeBar _lifeBar;

        private State _currentState;

        private Sprite _hitSprite;

        private Player _player;

        private enum State
        {
            IDLE,
            MOVING,
            JUMPING,
            FALLING,
            ATTACKING,
            CLIMBING,
            SLEEPING,
            FOLLOWING,
            DYING
        }

        public Swordman(float x, float y) : base("Data/AnimationSprites/swordman.png", 5, 1)
        {
            SetScaleXY(1.3f);
            this.x = x;
            this.y = y - 32 - this.height;
            currentFrame = 1;

            _movingRight = true;
            _canFlip = true;
            _delay = true;
            _frameRate = 12;
            _speed = 2;
            _canGerateRandomAction = true;

           _timerRandomAction = new System.Timers.Timer();
            _timerRandomAction.Elapsed += new ElapsedEventHandler(timerRandomActionCompleted);
            _timerRandomAction.Interval = 100;
            _timerRandomAction.Enabled = true;

            _timerHideLifeBar = new System.Timers.Timer();
            _timerHideLifeBar.Elapsed += new ElapsedEventHandler(timerHideLifeBarCompleted);
            _timerHideLifeBar.Interval = 500;
            _timerHideLifeBar.Enabled = false;

            _timerCanAttack = new System.Timers.Timer();
            _timerCanAttack.Elapsed += new ElapsedEventHandler(timerCanAttackCompleted);
            _timerCanAttack.Interval = 1500;
            _timerCanAttack.Enabled = false;

            _timerWaitBeforeFollowing = new System.Timers.Timer();
            _timerWaitBeforeFollowing.Elapsed += new ElapsedEventHandler(timerWaitBeforeFollowingCompleted);
            _timerWaitBeforeFollowing.Interval = 1500;
            _timerWaitBeforeFollowing.Enabled = false;

            _status = new Status();
            AddChild(_status);

            _currentWeapon = new Weapon();
            _currentWeapon.SetWeapon(4);
            AddChild(_currentWeapon);

            _screenSection = Convert.ToInt32(Math.Floor(this.x / 800));

            _colliderBox = new Collider("Data/Sprites/HitBox.png", this);
            _colliderBox2 = new Collider("Data/Sprites/HitBox2.png", this);
            _colliderBox2.SetOrigin(-20, -20);
            AddChild(_colliderBox);
            AddChild(_colliderBox2);

            _lifeBar = new LifeBar();
            _lifeBar.SetXY(-10, -5);
            _lifeBar.visible = false;
            AddChild(_lifeBar);

            _lifePoints = 100;

            _hitSprite = new Sprite("Data/Sprites/hit.png");
            AddChild(_hitSprite);
            _hitSprite.visible = false;
            _hitSprite.SetScaleXY(0.8f);
            _hitSprite.SetXY(0, 15);

            _player = ((MyGame)game).GetPlayer();
        }

        private void timerWaitBeforeFollowingCompleted(object sender, ElapsedEventArgs e)
        {
            _currentState = State.FOLLOWING;
            _timerWaitBeforeFollowing.Enabled = false;
            _attacked = false;
        }

        private void timerCanAttackCompleted(object sender, ElapsedEventArgs e)
        {
            _delay = true;
            _timerCanAttack.Enabled = false;
        }

        private void timerHideLifeBarCompleted(object sender, ElapsedEventArgs e)
        {
            _lifeBar.visible = false;
            _timerHideLifeBar.Enabled = false;
        }

        private void timerRandomActionCompleted(object sender, ElapsedEventArgs e)
        {
            _canGerateRandomAction = true;
        }

        void Update()
        {
            if (!((MyGame)game).IsPaused())
            {
                _counter++;

                _colliding = false;

                if (_currentState != State.DYING)
                {
                    checkCollisions();

                    if (_currentState != State.DYING)
                    {
                        move();

                        detectPlayer();

                        generateAction();
                    }
                }
                else
                {
                    //Died
                    _colliding = true;
                }

                if (_counter == (60 / _frameRate))
                {
                    checkPlayerAttack();

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

                    animate();

                    _counter = 0;
                }

                //Gravity
                if (!_colliding)
                {
                    _velocity += 0.2f;
                    y += _velocity;
                }
            }
        }

        private void checkCollisions()
        {
            GameObject[] collisions = _colliderBox.GetCollisions();
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i] is Tile)
                {
                    Tile _tile = collisions[i] as Tile;
                    //Check if player is colliding with ground tiles
                    if (((_tile.GetId() >= 1 && _tile.GetId() <= 3) || (_tile.GetId() >= 7 && _tile.GetId() <= 9) || (_tile.GetId() >= 13 && _tile.GetId() <= 15)))
                    {
                        y = _tile.y - this.height + 14;
                        _colliding = true;
                    }

                    if (_tile.GetId() == 1 || _tile.GetId() == 3 || _tile.GetId() == 7 || _tile.GetId() == 9 || _tile.GetId() == 13 || _tile.GetId() == 15 || _screenSection != Convert.ToInt32(Math.Floor((this.x - 10) / (800))) || _screenSection != Convert.ToInt32(Math.Floor((this.x + width + 1) / (800))))
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
                else if (collisions[i] is Sprite)
                {
                    Sprite _tile = collisions[i] as Sprite;
                    if (_tile.texture.filename == "Data/Sprites/blue_wave.png" && Math.Floor(_player.x / 800) == _screenSection)
                    {
                        Attacked(100);
                    }
                }
            }

            bool attacking = false;

            if (_player.GetState() != Player.State.HIDING && ((_player.x < this.x && _mirrorX) || _player.x > this.x && !_mirrorX) && (_player.GetYClimb() < 13 || _player.GetYClimb() > 95))
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

            if (!attacking && _currentState == State.ATTACKING)
            {
                _currentState = State.IDLE;
            }
        }

        private void move()
        {
            if (_currentState == State.MOVING && _currentState != State.FOLLOWING)
            {
                if (_movingRight)
                {
                    Move(_speed, 0);
                }
                else
                {
                    Move(-_speed, 0);
                }
            }
            else if (_currentState == State.FOLLOWING)
            {
                if (_playerRight)
                {
                    Move(_speed, 0);
                }
                else
                {
                    Move(-_speed, 0);
                }
            }
        }

        private void detectPlayer()
        {
            int _distance = 50;

            //Keep follow player if is jumping
            if (_player.GetState() == Player.State.JUMPING || _player.GetState() == Player.State.FALLING)
            {
                _distance = 100;
            }


            if (_currentState != State.SLEEPING && Math.Floor(this.x / 800) == Math.Floor(_player.x / 800) && Math.Abs(this.y - _player.y) < _distance && Math.Abs(this.x - _player.x) < 200 && _player.GetState() != Player.State.HIDING && (_player.GetYClimb() < 13 || _player.GetYClimb() > 95))
            {
                //Detect player
                if (_player.x > this.x && !_mirrorX && _currentState != State.ATTACKING)
                {
                    _playerRight = true;
                    if (_attacked)
                    {
                        _timerWaitBeforeFollowing.Enabled = true;
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
                        _timerWaitBeforeFollowing.Enabled = true;
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
                    _status.SetVisible(false);
                }
                
                _attacked = false;
                
            }
        }

        private void generateAction()
        {
            if (_canGerateRandomAction && _currentState != State.FOLLOWING)
            {
                _canGerateRandomAction = false;

                _timerRandomAction.Interval = Utils.Random(1000, 2500);
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

        private void checkPlayerAttack()
        {
            if (_colliderBox.HitTest(_player.GetWeapon()) && _player.GetWeapon().visible && Math.Floor(_player.x / 800) == _screenSection)
            {
                if (_player._mirrorX)
                {
                    if (_player.x > this.x)
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
        }

        private void animate()
        {
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
                    if (_delay)
                    {
                        currentFrame = 2;
                        _currentWeapon.SetVisible(true, _mirrorX, 0, 10);
                        _player.Attacked(25);
                        _delay = false;
                        _timerCanAttack.Enabled = true;
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
            _timerHideLifeBar.Enabled = true;
            
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