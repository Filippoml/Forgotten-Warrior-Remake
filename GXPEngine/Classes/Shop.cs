using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GXPEngine.Classes
{
    public class Shop : Sprite
    {
        private Sprite _itemSelectedSprite, _actionSelectedSprite;

        private Items _items;

        private int _indexItems, _indexAction;

        private EasyDraw _easyDraw;

        private readonly Font _font;

        private Player _player;

        private HUD _hud;

        public Shop(float x, float y) : base("Data/HUD/shop.png")
        {
            SetScaleXY(2f);
            this.x = x;
            this.y = y;

            _player = ((MyGame)game).GetPlayer();
            _hud = ((MyGame)game).GetHud();

            Bitmap Bmp = new Bitmap(100, 100);
            Graphics gfx = Graphics.FromImage(Bmp);
            SolidBrush brush = new SolidBrush(Color.White);
            gfx.FillRectangle(brush, 0, 0, 100, 100);
            _itemSelectedSprite = new Sprite(Bmp)
            {
                width = 26,
                height = 26,
                x = 41,
                y = 131
            };
            AddChild(_itemSelectedSprite);

            _actionSelectedSprite = new Sprite("Data/HUD/shop_rect.png");
            _actionSelectedSprite.SetXY(164, 46);
            AddChild(_actionSelectedSprite);

            //Filling items
            for (int i = 0; i < 5; i++)
            {                
                Sprite _itemSprite = new Sprite("Data/Sprites/item" + i + ".png");
                if (i > 2)
                {
                    _itemSprite.SetXY(41 + 1 + (i * 32), 133);
                }
                else
                {
                    _itemSprite.SetXY(41 + 3 + (i * 32), 134);
                }
                AddChild(_itemSprite);
            }

         
            string path = "Data/Items.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(Items));

            StreamReader reader = new StreamReader(path);
            _items = (Items)serializer.Deserialize(reader);
            reader.Close();


            _easyDraw = new EasyDraw(300, 300);
            AddChild(_easyDraw);

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("Data/Font/LCD Solid.ttf");
            _font = new Font(new FontFamily(pfc.Families[0].Name), 10, FontStyle.Regular);

            
        }
        void Update()   
        {
            if (!((MyGame)game).IsPaused())
            {
                if (visible)
                {
                    keyHandler();
                }
            }
        }

        private void keyHandler()
        {
            if (Input.GetKeyDown(Key.A))
            {
                if (_itemSelectedSprite.x > 41)
                {
                    _itemSelectedSprite.x -= 32;
                    _indexItems--;

                    updateValues();
                }

            }
            else if(Input.GetKeyDown(Key.D))
            {
                if (_itemSelectedSprite.x < 169)
                {
                    _itemSelectedSprite.x += 32;
                    _indexItems++;

                    updateValues();
                }
         
            }
            else if (Input.GetKeyDown(Key.W))
            {
                
                if (_actionSelectedSprite.y > 46)
                {
                    _actionSelectedSprite.y -= 18;
                    _indexAction--;
                }

            }
            else if (Input.GetKeyDown(Key.S))
            {
                if(_actionSelectedSprite.y < 82)
                {
                    _actionSelectedSprite.y += 18;
                    _indexAction++;
                }
            }
            else if (Input.GetKeyDown(Key.ENTER))
            {
                Item _item = _items.Item[_indexItems];
                switch(_indexAction)
                {
                    case 0:
                        if (_player.GetCoinsNumber() >= _item.Cost)
                        {
                            switch (_indexItems)
                            {
                                case 0:
                                case 1:
                                case 2:
                                    _player.GetWeapon().SetWeapon(_indexItems + 1);
                                    _player.SetCoinsNumber(false, _item.Cost);
                                    updateValues();
                                    _easyDraw.graphics.DrawString("ITEM PURCHASED", _font, new SolidBrush(Color.White), new PointF(60, 200));
                                    break;

                                case 3:
                                    if(_hud.GetHealthPotionsNumber() < 9)
                                    {
                                        _hud.SetHealthPotionsNumber(true);
                                        _player.SetCoinsNumber(false, _item.Cost);
                                        updateValues();
                                        _easyDraw.graphics.DrawString("ITEM PURCHASED", _font, new SolidBrush(Color.White), new PointF(60, 200));
                                    }

                                    break;

                                case 4:
                                    if (_hud.GetManaPotionsNumber() < 9)
                                    {
                                        _hud.SetManaPotionsNumber(true);
                                        _player.SetCoinsNumber(false, _item.Cost);
                                        updateValues();
                                        _easyDraw.graphics.DrawString("ITEM PURCHASED", _font, new SolidBrush(Color.White), new PointF(60, 200));
                                    }                                        
                                    break;
                            }

                        }
                        else
                        {
                            updateValues();
                            _easyDraw.graphics.DrawString("NO ENOUGH MONEY", _font, new SolidBrush(Color.White), new PointF(55, 200));
                        }
                        break;

                    case 1:

                        updateValues();
                        switch (_item.Type)
                        {
                            case "weapon":
                                _easyDraw.graphics.DrawString("DAMAGE:" + _item.Damage, _font, new SolidBrush(Color.White), new PointF(36, 200));
                                _easyDraw.graphics.DrawString("RANGE:" + _item.Range, _font, new SolidBrush(Color.White), new PointF(125, 200));
                                
                                break;
                            case "potion":
                                _easyDraw.graphics.DrawString("HEALING:(" + _item.Value + "%)", _font, new SolidBrush(Color.White), new PointF(60, 200));
                                break;
                        }
                        break;

                    case 2:
                        visible = false;
                        _player.SetState(Player.State.IDLE);
                        break;
                }
            }
        }

        public void Show()
        {
            updateValues();
            _easyDraw.graphics.DrawString("WELCOME", _font, new SolidBrush(Color.White), new PointF(80, 200));
            visible = true;
        }

        private void updateValues()
        {
            Item _item = _items.Item[_indexItems];
            _easyDraw.Clear(Color.Transparent);
            _easyDraw.graphics.DrawString(_player.GetCoinsNumber().ToString(), _font, new SolidBrush(Color.White), new PointF(172, 172.5f));
            _easyDraw.graphics.DrawString(_item.Cost.ToString(), _font, new SolidBrush(Color.White), new PointF(65, 172.5f));
        }
    }
}