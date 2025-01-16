
using RPG_Game.Data.Constants;

namespace RPG_Game
{
    public class Monster : Character
    {
        public Monster(int x, int y)
        {
            Random rand = new Random();
            Strenght = rand.Next(1, 4);
            Agility = rand.Next(1, 4);
            Intelligence = rand.Next(1, 4);
            Position = new Position(x, y);
            Symbol = 'o';

            Health = Strenght * 5;
            Mana = Intelligence * 3;
            Damage = Agility * 2;
        }

        public int Mana { get; set; }
        public bool IsDead { get; set; } = false;

        public void MoveToPlayer(Position playerPosition)
        {
            int xDirection = playerPosition.X > Position.X ? 1 : playerPosition.X < Position.X ? -1 : 0;
            int yDirection = playerPosition.Y > Position.Y ? 1 : playerPosition.Y < Position.Y ? -1 : 0;

            Position.X = Math.Clamp(Position.X + xDirection, 0, GameConstants.BoardWidth - 1);
            Position.Y = Math.Clamp(Position.Y + yDirection, 0, GameConstants.BoardHeight - 1);
        }
    }
}
