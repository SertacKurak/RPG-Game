namespace RPG_Game.Data.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public CharacterModel Character { get; set; }
        public int KilledMonsters { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
