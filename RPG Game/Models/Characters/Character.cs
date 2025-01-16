using RPG_Game.Data.Constants;

namespace RPG_Game
{
    public class Character
    {
        public Character()
        {
            Health = Strenght * 5;
            Damage = Agility * 2;
        }

        public int Id { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public Position Position { get; set; } = new Position(1, 1);
        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public char Symbol { get; set; }


        public void Move(int x, int y)
        {
            Position.X = Math.Clamp(Position.X + x, 0, GameConstants.BoardWidth - 1);
            Position.Y = Math.Clamp(Position.Y + y, 0, GameConstants.BoardHeight - 1);
        }

        public void Attack(Monster monster)
        {
            monster.Health -= Damage;
            if (monster.Health <= 0)
            {
                Console.WriteLine($"Monster at ({monster.Position.X}, {monster.Position.Y}) has been killed!");
                monster.IsDead = true;
            }
            else
            {
                Console.WriteLine($"Monster at ({monster.Position.X}, {monster.Position.Y}) has {monster.Health} health left.");
            }
        }


    }
}
