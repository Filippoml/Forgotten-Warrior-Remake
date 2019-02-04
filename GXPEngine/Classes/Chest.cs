using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class Chest : AnimationSprite
    {
        private Player _player;
        Dialog _dialog;
        private int _lootQuantity;
        private String _lootType;
        
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        public Chest (float x, float y): base ("Data/chest.png", 2, 1)
        {
            this.x = x;
            this.y = y - 32 - this.height;

            _player = ((MyGame)game).GetPlayer();

            _dialog = new Dialog();
            ((MyGame)game).AddChild(_dialog);
            _dialog.visible = false;

            _lootQuantity = 1;
            switch (rnd.Next(1, 4))
            {
                case 1:
                    _lootType = "item3"; //health filename
                    break;
                case 2:
                    _lootType = "item4"; //mana filename
                    break;
                case 3:
                    _lootType = "coin"; //coin filename
                    _lootQuantity = rnd.Next(20, 51);
                    break;
            }
            
        }

        void Update()
        {
   
            if (!_dialog.visible && currentFrame == 0)
            {
                if (this.HitTest(_player.getCollider()))
                {
                    _dialog.SetVisible(true, _lootQuantity, _lootType);
  
                }
            }

            if (_dialog.visible)
            {

                if (Input.GetKeyDown(Key.ENTER))
                {
                    
                    _dialog.SetVisible(false, 0, null);
                    currentFrame = 1;
                    y-= 2;
                    x += 3;
                }
            }
        }
    }
}
