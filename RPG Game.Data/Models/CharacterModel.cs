namespace RPG_Game.Data.Models
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int PointsAssigned { get; set; }

        public ICollection<GameModel> Games { get; set; }
    }
}