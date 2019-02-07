using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GXPEngine.Classes
{
    public class Weapon : AnimationSprite
    {
        private int _range, _damage;

        private float _xVelocity = 2, _startX;
        

        private bool _returning = false;
        public Weapon () : base("Data/AnimationSprites/weapons.png", 5,1)
        {
            y = 15;
            visible = false;
            _damage = 20;
        }

        public void SetVisible(bool value, bool mirrored, int mirroredOffset1, int mirroredOffset2)
        {
            visible = value;

                Mirror(mirrored, false);
                if (mirrored)
                {
                    x = mirroredOffset1;
                }
                else
                {
                   x = mirroredOffset2;
                }
            _startX = x;
        }
        public void SetWeapon(int weapon_number)
        {
            currentFrame = weapon_number;

            if (weapon_number > 0)
            {
                string path = "Data/Items.xml";

                XmlSerializer serializer = new XmlSerializer(typeof(Items));

                StreamReader reader = new StreamReader(path);
                Items _items = (Items)serializer.Deserialize(reader);
                _damage = Convert.ToInt32(_items.Item[weapon_number - 1].Damage);
                _range = Convert.ToInt32(_items.Item[weapon_number - 1].Range);
                reader.Close();
            }
        }

        void Update()
        {
            if (!((MyGame)game).IsPaused())
            {
                if (currentFrame > 0 && currentFrame < 4)
                {
                    if (Math.Abs(x) < _range * 20 && !_returning && visible)
                    {
                        _xVelocity += 0.2f;
                        if (_mirrorX)
                        {
                            x -= _xVelocity;
                        }
                        else
                        {
                            x += _xVelocity;
                        }

                        _returning = false;

                    }
                    else if (x > _startX && visible && !_mirrorX)
                    {


                        x -= _xVelocity;

                        _returning = true;


                    }
                    else if (x < _startX && visible && _mirrorX)
                    {
                        x += _xVelocity;
                        _returning = true;

                    }
                    else
                    {
                        _xVelocity = 2;
                        _returning = false;
                        visible = false;
                    }
                }
            }
        }

        public void SetReturing()
        {
            if (currentFrame != 0)
            {
                _returning = true;
            }
        }

        public int GetWeapon()
        {
            return currentFrame;
        }

        public int GetDamage()
        {
            return _damage;
        }

    }
}
