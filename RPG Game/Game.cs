using RPG_Game.Common;
using RPG_Game.Data;
using RPG_Game.Data.Enums;
using RPG_Game.Data.Models;

namespace RPG_Game
{
    using static RPG_Game.Data.Constants.GameConstants;
    public class Game
    {
        public Character Character { get; set; }
        public List<Monster> Monsters { get; set; } = new List<Monster>();
        public int KilledMonsters { get; set; }
        public char[,] GameBoard { get; set; }
        public GameState CurrentState { get; set; }

        public Game()
        {
            GameBoard = new char[BoardWidth, BoardHeight];
            CurrentState = GameState.MainMenu;

        }

        public void Start()
        {
            while (CurrentState != GameState.Exit)
            {
                switch (CurrentState)
                {
                    case GameState.MainMenu:
                        ShowMainMenu();
                        break;
                    case GameState.CharacterSelect:
                        CharacterSelect();
                        break;
                    case GameState.InGame:
                        InGame();
                        break;
                    case GameState.Exit:
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome!\nPress any key to play.");
            Console.ReadKey();
            CurrentState = GameState.CharacterSelect;
        }

        private void CharacterSelect()
        {
            Console.Clear();
            Console.WriteLine("Choose character type:\nOptions:\n1) Warrior\n2) Archer\n3) Mage\nYour pick:");
            int choice = GetValidInput(1, 3);

            switch (choice)
            {
                case 1:
                    Character = new Warrior();
                    break;
                case 2:
                    Character = new Archer();
                    break;
                case 3:
                    Character = new Mage();
                    break;
            }

            DistributePoints();

            SaveCharacterToDatabase();
            CurrentState = GameState.InGame;
        }

        private void DistributePoints()
        {
            int pointsRemaining = 3;
            while (pointsRemaining > 0)
            {
                Console.Clear();
                Console.WriteLine($"Remaining Points: {pointsRemaining}\n1) Add to Strength: {Character.Strenght}\n2) Add to Agility: {Character.Agility}\n3) Add to Intelligence: {Character.Intelligence}");
                int choice = GetValidInput(1, 3);

                Console.WriteLine("Enter number of points to add:");
                int points = GetValidInput(1, pointsRemaining);

                switch (choice)
                {
                    case 1:
                        Character.Strenght += points;
                        break;
                    case 2:
                        Character.Agility += points;
                        break;
                    case 3:
                        Character.Intelligence += points;
                        break;
                }
                pointsRemaining -= points;
            }

            if (Character is IPlayer player)
            {
                player.Points = 3 - pointsRemaining;
            }

        }

        private int GetValidInput(int min, int max)
        {
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max}:");
            }
        }

        private void InGame()
        {
            while (Character.Health > 0)
            {
                Console.Clear();

                if (Character is Mage mage)
                {
                    Console.WriteLine($"Health: {Character.Health}, Mana: {mage.Mana}");
                }
                else
                {
                    Console.WriteLine($"Health: {Character.Health}, Mana: 0");
                }

                PrintBoard();

                Console.WriteLine("Choose action:");
                Console.WriteLine("1) Attack");
                Console.WriteLine("2) Move");

                int action = GetValidInput(1, 2);

                switch (action)
                {
                    case 1:
                        AttackAction();
                        break;
                    case 2:
                        MoveAction();
                        break;
                }

                AddMonsterIfValid();
                HandleCombat();
                MoveMonstersTowardsPlayer();
                CheckGameOver();
            }
        }

        private void AttackAction()
        {
            List<Monster> monstersInRange = GetMonstersInRange(Character.Position);
            if (monstersInRange.Count > 0)
            {
                Console.WriteLine("\nMonsters in range:");
                for (int i = 0; i < monstersInRange.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Monster at ({monstersInRange[i].Position.X}, {monstersInRange[i].Position.Y}) with {monstersInRange[i].Health} health.");
                }
                Console.WriteLine("Choose a monster to attack:");

                int monsterIndex = GetValidInput(1, monstersInRange.Count) - 1;
                Character.Attack(monstersInRange[monsterIndex]);
                if (monstersInRange[monsterIndex].IsDead)
                {
                    Monsters.Remove(monstersInRange[monsterIndex]);
                    KilledMonsters++;
                }
            }
            else
            {
                Console.WriteLine("\nNo monsters in range.");
            }
        }


        private void MoveAction()
        {
            Console.WriteLine("\nW - Move up\nS - Move down\nD - Move right\nA - Move left");
            Console.WriteLine("Q - Move diagonally up & left\nE - Move diagonally up & right");
            Console.WriteLine("Z - Move diagonally down & left\nX - Move diagonally down & right");
            Console.WriteLine("Choose action:");

            char moveAction = Console.ReadKey().KeyChar;

            switch (char.ToUpper(moveAction))
            {
                case 'W':
                    Character.Move(0, -1);
                    break;
                case 'S':
                    Character.Move(0, 1);
                    break;
                case 'D':
                    Character.Move(1, 0);
                    break;
                case 'A':
                    Character.Move(-1, 0);
                    break;
                case 'Q':
                    Character.Move(-1, -1);
                    break;
                case 'E':
                    Character.Move(1, -1);
                    break;
                case 'Z':
                    Character.Move(-1, 1);
                    break;
                case 'X':
                    Character.Move(1, 1);
                    break;
                default:
                    Console.WriteLine("\nInvalid action, try again!");
                    return;
            }

            MoveMonstersTowardsPlayer();
        }

        private void AddMonsterIfValid()
        {
            Random rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(BoardWidth);
                y = rand.Next(BoardHeight);
            } while (GameBoard[y, x] != '▒');

            Monster monster = new Monster(x, y);
            Monsters.Add(monster);
        }

        private void HandleCombat()
        {
            foreach (var monster in Monsters)
            {
                if (!monster.IsDead)
                {
                    if (Math.Abs(Character.Position.X - monster.Position.X) <= 1 &&
                        Math.Abs(Character.Position.Y - monster.Position.Y) <= 1)
                    {
                        Character.Health -= monster.Damage;
                        Console.WriteLine($"Monster attacked! Health is now: {Character.Health}");
                    }
                    else
                    {
                        monster.MoveToPlayer(Character.Position);
                    }
                }
            }

        }

        private List<Monster> GetMonstersInRange(Position playerPosition)
        {
            List<Monster> monstersInRange = new List<Monster>();
            foreach (var monster in Monsters)
            {
                if (Math.Abs(playerPosition.X - monster.Position.X) <= Character.Range && Math.Abs(playerPosition.Y - monster.Position.Y) <= Character.Range)
                    monstersInRange.Add(monster);
            }
            return monstersInRange;
        }

        private void MoveMonstersTowardsPlayer()
        {
            foreach (var monster in Monsters)
            {
                if (!monster.IsDead)
                {
                    monster.MoveToPlayer(Character.Position);
                }
            }
        }

        private void CheckGameOver()
        {
            if (Character.Health <= 0)
            {
                Console.WriteLine("\nGame Over! You have been defeated.");
                SaveGameToDatabase();
                Environment.Exit(0);
            }

            if (Monsters.Count == 0)
            {
                Console.WriteLine("\nCongratulations! You have killed all the monsters.");
                SaveGameToDatabase();
                Environment.Exit(0);
            }
        }

        private void PrintBoard()
        {
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    GameBoard[i, j] = '▒';
                }
            }


            foreach (var monster in Monsters)
            {
                if (!monster.IsDead)
                {
                    GameBoard[monster.Position.Y, monster.Position.X] = monster.Symbol;
                }
            }

            GameBoard[Character.Position.Y, Character.Position.X] = Character.Symbol;

            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    Console.Write(GameBoard[i, j]);
                }
                Console.WriteLine();
            }
        }
        private int GetPointsAssigned()
        {
            if (Character is IPlayer player)
            {
                return player.Points;
            }
            return -1;
        }


        private void SaveCharacterToDatabase()
        {
            using (var context = new RPGContext())
            {
                var character = new CharacterModel
                {
                    CreatedAt = DateTime.Now,
                    Health = Character.Health,
                    Attack = Character.Damage,
                    PointsAssigned = GetPointsAssigned()
                };

                context.Characters.Add(character);
                context.SaveChanges();
                Character.Id = character.Id;
            }
        }

        private void SaveGameToDatabase()
        {
            using (var context = new RPGContext())
            {
                GameModel gameSession = new GameModel
                {
                    CharacterId = Character.Id,
                    KilledMonsters = KilledMonsters,
                    CreatedAt = DateTime.Now
                };

                context.Games.Add(gameSession);
                context.SaveChanges();
            }
        }
    }
}