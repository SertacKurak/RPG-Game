
using RPG_Game.Common;

namespace RPG_Game
{
    public class Warrior : Character, IPlayer
    {
        public Warrior()
        {
            Strenght = 3;
            Agility = 3;
            Intelligence = 0;
            Range = 1;
            Symbol = '@';

            Health = Strenght * 5;
            Damage = Agility * 2;
        }

        public int Points { get; set; }
    }
}
