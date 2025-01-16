
using RPG_Game.Common;

namespace RPG_Game
{
    public class Archer : Character, IPlayer
    {
        public Archer()
        {
            Strenght = 2;
            Agility = 4;
            Intelligence = 0;
            Range = 2;
            Symbol = '#';
            Health = Strenght * 5;
            Damage = Agility * 2;
        }

        public int Points { get; set; }
    }
}
