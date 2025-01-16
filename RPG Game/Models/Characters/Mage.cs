
using RPG_Game.Common;

namespace RPG_Game
{
    public class Mage : Character, IPlayer
    {
        public Mage()
        {
            Strenght = 2;
            Agility = 1;
            Intelligence = 3;
            Range = 3;
            Symbol = '*';
            Mana = Intelligence * 3;

            Health = Strenght * 5;
            Damage = Agility * 2;
        }
        public int Mana { get; set; }
        public int Points { get; set; }
    }
}
