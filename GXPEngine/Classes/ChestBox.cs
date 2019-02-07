using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Classes
{
    public class ChestBox : AnimationSprite
    {
        private Player _player;
        Dialog _dialog;
        private readonly int _lootQuantity;
        private readonly String _lootType;

        public ChestBox (float x, float y): base ("Data/Sprites/chest.png", 2, 1)
        {
            this.x = x;
            //32 is the height of the level tile 
            this.y = y - 32 - this.height;

            _player = ((MyGame)game).GetPlayer();

            _dialog = new Dialog();
            ((MyGame)game).AddChild(_dialog);
            _dialog.visible = false;
     
            
            //Random Loot Generation
            //If generated loot are not coin, 1 potion will be generated
            _lootQuantity = 1;
            switch (Utils.Random(1, 4))
            {
                case 1:
                    _lootType = "item3"; //health filename
                    break;
                case 2:
                    _lootType = "item4"; //mana filename
                    break;
                case 3:
                    _lootType = "coin"; //coin filename
                    _lootQuantity = Utils.Random(20, 51);
                    break;
            }
        }

        void Update()
        {
            //Check if dialog is not visible and the chest box is still closed
            if (!_dialog.visible && currentFrame == 0)
            {
                //Collision with the player
                if (this.HitTest(_player.getCollider()) && _player.GetState() == Player.State.MOVING)
                {
                    Sound _sound = new Sound("Data/Sounds/chest.wav", false, false);
                    _sound.Play();
                    //Show dialog
                    _dialog.SetVisible(true, _lootQuantity, _lootType);
                }
            }
            else if (_dialog.visible)
            {
                if (Input.GetKeyDown(Key.ENTER))
                {
                    //Close dialog on Enter Key
                    _dialog.SetVisible(false, 0, null);
                    
                    //Close the chest box
                    currentFrame = 1;

                    //Set coords to preview sprite coords
                    y-= 2;
                    x += 3;
                }
            }
        }
    }
}
