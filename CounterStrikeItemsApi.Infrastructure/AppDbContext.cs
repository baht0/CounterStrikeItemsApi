using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CounterStrikeItemsApi.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        #region Таблицы Предметов
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCommon> ItemCommons { get; set; }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionType> CollectionTypes { get; set; }

        public DbSet<ProfessionalPlayer> ProfessionalPlayers { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Subtype> Subtypes { get; set; }

        public DbSet<Exterior> Exteriors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Quality> Qualities { get; set; }

        public DbSet<GraffitiColor> GraffitiColors { get; set; }

        public DbSet<ItemOrdersHistory> ItemOrdersHistories { get; set; }

        public DbSet<Found> Founds { get; set; }
        public DbSet<ItemTypeSubtype> ItemTypeSubtypes { get; set; }
        #endregion

        public DbSet<SteamUser> SteamUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Found>()
                .HasOne(f => f.ItemCommon)
                .WithMany(i => i.FoundsAsItem)
                .HasForeignKey(f => f.ItemCommonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Found>()
                .HasOne(f => f.Container)
                .WithMany(i => i.FoundsAsContainer)
                .HasForeignKey(f => f.ContainerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemCommon)
                .WithMany(ic => ic.Items)
                .HasForeignKey(i => i.ItemCommonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
