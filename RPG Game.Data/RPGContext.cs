using Microsoft.EntityFrameworkCore;
using RPG_Game.Data.Models;

namespace RPG_Game.Data
{
    public class RPGContext : DbContext
    {

        public DbSet<CharacterModel> Characters { get; set; }
        public DbSet<GameModel> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-2LCR62F\\SQLEXPRESS02;Database=RPG;Integrated Security=true;TrustServerCertificate=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameModel>()
                .HasOne(g => g.Character)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CharacterId);
        }
    }
}